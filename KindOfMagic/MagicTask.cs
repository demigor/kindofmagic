using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.Cecil;
using Mono.Cecil.Pdb;

namespace KindOfMagic
{
  public class MagicTask : Task
  {
    [Required]
    public string Assembly { get; set; }

    [Required]
    public string References { get; set; }
    public string KeyFile { get; set; }

    public string RaiseMethod { get; set; }
    public string BeaconMethod { get; set; }
    public string MagicAttribute { get; set; }
    public string NoMagicAttribute { get; set; }

    class PreloadingAssemblyResolver : DefaultAssemblyResolver
    {
      public PreloadingAssemblyResolver(string references)
      {
        Load(references);
      }

      public bool NoGAC;

      AssemblyDefinition _systemRuntime;

      public override AssemblyDefinition Resolve(AssemblyNameReference name)
      {
        if (name.Name == "System.Runtime")
          return ResolveSystemRuntime(name);

        return base.Resolve(name);
      }

      AssemblyDefinition ResolveSystemRuntime(AssemblyNameReference name)
      {
        return _systemRuntime ?? (_systemRuntime = base.Resolve(name));
      }

      public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
      {
        return NoGAC ? ResolveNoGAC(name, ref parameters) : base.Resolve(name, parameters);
      }

      public readonly List<string> Directories = new List<string> { "." };

      public void AddSilverlightDirectories(int version)
      {
        var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        Directories.Add(Path.Combine(programFiles, string.Format(@"Reference Assemblies\Microsoft\Framework\Silverlight\v{0}.0", version)));
        Directories.Add(Path.Combine(programFiles, string.Format(@"Microsoft SDKs\Silverlight\v{0}.0\Libraries\Client", version)));
        NoGAC = true;
      }

      AssemblyDefinition ResolveNoGAC(AssemblyNameReference name, ref ReaderParameters parameters)
      {
        if (name == null)
          throw new ArgumentNullException("name");

        if (parameters == null)
          parameters = new ReaderParameters();

        var assembly = SearchDirectory(name, Directories, parameters);
        if (assembly != null)
          return assembly;

        throw new AssemblyResolutionException(name);
      }

      AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
      {
        if (parameters.AssemblyResolver == null)
          parameters.AssemblyResolver = this;

        return ModuleDefinition.ReadModule(file, parameters).Assembly;
      }

      AssemblyDefinition SearchDirectory(AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
      {
        var extensions = new[] { ".dll", ".exe" };
        foreach (var directory in directories)
        {
          foreach (var extension in extensions)
          {
            string file = Path.Combine(directory, name.Name + extension);
            if (File.Exists(file))
              return GetAssembly(file, parameters);
          }
        }

        return null;
      }

      public void Load(params string[] references)
      {
        foreach (var dll in references)
        {
          var assembly = ModuleDefinition.ReadModule(dll, new ReaderParameters { AssemblyResolver = this }).Assembly;
          
          if (assembly.Name.Name == "System.Runtime")
            _systemRuntime = assembly;

          RegisterAssembly(assembly);
        }
      }

      public void Load(string references)
      {
        if (references != null)
          Load(references.Split(';'));
      }
    }

    void TaskLogger(LogLevel level, string format, params object[] args)
    {
      switch (level)
      {
        case LogLevel.Error: Log.LogError("Build", "", "", "", -1, -1, -1, -1, format, args); break;
        case LogLevel.Message: Log.LogMessage("Build", "", "", "", -1, -1, -1, -1, format, args); break;
        case LogLevel.Warning: Log.LogWarning("Build", "", "", "", -1, -1, -1, -1, format, args); break;
        case LogLevel.Verbose: Log.LogMessage(format, args); break; // goes into MSBuild log 
      }
    }

    public override bool Execute()
    {
      Log.LogMessage("Magic happens with {0}.", Assembly);
      try
      {
        var resolver = new PreloadingAssemblyResolver(References);

        var parameters = MakeReaderParameters(Assembly, resolver);
        var assembly = AssemblyDefinition.ReadAssembly(Assembly, parameters);

        var processor = new Processor(parameters.AssemblyResolver, assembly.MainModule)
        {
          MagicAttributeName = MagicAttribute,
          NoMagicAttributeName = NoMagicAttribute,
          RaiseMethodName = RaiseMethod,
          BeaconMethodName = BeaconMethod,
          Logger = TaskLogger
        };

        if (!processor.NeedProcessing)
        {
          Log.LogMessage("Assembly is already enchanted.");
          return true;
        }

        Log.LogMessage("Enchanting assembly...");
        var result = processor.Process();

        switch (result)
        {
          case MagicResult.NoMagicFound: Log.LogMessage("No Magic attributes found..."); return true;
          case MagicResult.NoMagicNeeded: Log.LogMessage("Enchanting not needed..."); break;
          case MagicResult.MagicApplied: Log.LogMessage("Enchanted successfully..."); break;
        }

        if (KeyFile != null)
          Log.LogMessage("Resigning enchanted assembly with key from {0}...", KeyFile);

        assembly.Write(Assembly, MakeWriterParameters(Assembly, KeyFile));

        return true;
      }
      catch (FileNotFoundException)
      {
        // no files - no fun
        return false;
      }
      catch (Exception e)
      {
        Log.LogMessage(e.Message);
        return false;
      }
    }

    static void Main(string[] args)
    {
      Console.WriteLine("Kind Of Magic, Assembly enchanter. Copyright (c) Lex Lavnikov");

      if (args == null) return;

      var resolver = new PreloadingAssemblyResolver(null);

      foreach (var file in args)
      {
        if (file.StartsWith("$"))
        {
          resolver.Load(file.Substring(1));
          continue;
        }

        var parameters = MakeReaderParameters(file, resolver);
        var assembly = AssemblyDefinition.ReadAssembly(file, parameters);

        // Silverlight Assembly
        var corlib = (AssemblyNameReference)assembly.MainModule.TypeSystem.Corlib;
        if (corlib.PublicKeyToken[0] == 124 && !resolver.NoGAC)
          resolver.AddSilverlightDirectories(corlib.Version.Major);

        var processor = new Processor(parameters.AssemblyResolver, assembly.MainModule);
        switch (processor.Process())
        {
          case MagicResult.MagicApplied:
          case MagicResult.NoMagicNeeded:
            assembly.Write(file, MakeWriterParameters(file, null));
            break;
        }
      }
    }

    static ReaderParameters MakeReaderParameters(string file, IAssemblyResolver resolver)
    {
      var result = new ReaderParameters
      {
        ReadingMode = ReadingMode.Deferred,
        AssemblyResolver = resolver
      };

      if (File.Exists(Path.ChangeExtension(file, ".pdb")))
      {
        result.ReadSymbols = true;
        result.SymbolReaderProvider = new PdbReaderProvider();
      }

      return result;
    }

    static WriterParameters MakeWriterParameters(string file, string key = null)
    {
      if (File.Exists(Path.ChangeExtension(file, ".pdb")))
        return new WriterParameters
        {
          WriteSymbols = true,
          SymbolWriterProvider = new PdbWriterProvider(),
          StrongNameKeyPair = LoadSnKey(key)
        };

      return new WriterParameters
      {
        WriteSymbols = false,
        StrongNameKeyPair = LoadSnKey(key)
      };
    }

    static StrongNameKeyPair LoadSnKey(string key)
    {
      return key != null ? new StrongNameKeyPair(File.ReadAllBytes(key)) : null;
    }
  }
}
