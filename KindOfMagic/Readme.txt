Project Description

MSBuild task to simplify implementation of INotifyPropertyChanged interface.

Injects supporting code in property setters: raising PropertyChanged event when value changed.


What is it?

Take a look at this painfully familar code:

    public string Name 
    {
      get 
      { 
        return _name; 
      }
      set 
      { 
        if (_name != value) 
        {
          _name = value;
          RaisePropertyChanged("Name");
        }
      }
    }

and now imagine, that you just need to write only this:

    [Magic]
    public string Name { get; set; }


How to use?

1) Define somewhere in your project MagicAttribute class, derived from Attribute.
2) Apply it to properties in your ViewModel classes, implementing INotifyPropertyChanged. Your class must have accessible RaisePropertyChanged(string) method.

How it works?

1) KindOfMagic build task runs after compilation and before assembly signing with strong-name.
2) It looks for MagicAttribute, so only assemblies with this class will be processed (class better be private).
3) Only classes, implementing INotifyPropertyChanged, and their descendants are inspected.
4) It looks for RaisePropertyChanged(string) method.
5) Transforms all setters of properties with MagicAttribute applied.
6) Injects supporting code using Mono.Cecel. Use Reflector to see, what KindOfMagic does for you.


Installation

Right now, there is only manual installation.
a) Make somewhere folder KindOfMagic and extract content of the binary archive. 
b) Remember the path to KindOfMagic.exe (f.e. z:\KindOfMagic\KindOfMagic.targets)

1) Per-Project
a) Unload the project.
b) Right click on the project, select Edit YourProject.
c) Insert following Xml snippet somewhere inside Project element and adjust path to KindOfMagic.targets.

  <Import Project="z:\KindOfMagic\KindOfMagic.targets" />

2) Global 
a) Locate Microsoft.Common.targets for .NET framework 4
b) Insert the same line:

  <Import Project="z:\KindOfMagic\KindOfMagic.targets" />


Release Notes

Beta 6 (6 January 2010)
   - Beacon method Raise implemented (see http://kindofmagic.codeplex.com for description)

Beta 5 (5 January 2011)
   - Assembly resigning implemented (please update KindOfMagic.targets file if you have customized it)
   - Cross-assembly Magic supported (now is possible to declare a magic base class in one assembly and derive it in another assemlby, See http://kindofmagic.codeplex.com/workitem/12773)
   - Magic/NoMagic attributes are not removed after transformation any more for information purposes
   - GeneratedCodeAttribute is injected into main module to label already processed assemblies (to avoid double patching)

Beta 4 (2 January 2011)
   - Equality check fixed for string properties
   - Magic/NoMagic assembly cleanup improved

Beta 3 (23 December 2010)
   - Nullable types comparison operation IL fixed
   - Magic/NoMagic attributes removed after the build (to avoid double patching)

Beta 2 (17 December 2010)
   -Next beta release with several bug fixes.

   Magic attribute applied to a class makes all public properties to implement RaisePropertyChanged pattern implicitly.
   To exclude some properties, apply NoMagic attribute.

Beta 1 (1 December 2010)
   - Initial release


Hope you find this project helpful,
1 December, 2010 Alexey "Lex" Lavnikov