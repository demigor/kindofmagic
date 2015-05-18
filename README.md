##Project Description
MSBuild task to simplify implementation of INotifyPropertyChanged interface for Universal Windows Platform, WinRT, Silverlight 2.0+, .NET 2.0+, Windows Phone 7+, Portable Class Libraries. Distributed via Nuget.

During compile time it injects supporting code in property setters: raising PropertyChanged event when value changed. 

In comparison to similar projects, KindOfMagic:

* provides developer with the best control which properties and classes get transformed. 
* injects no complex heuristics which could make your application behave weird. 
* is easy to use and understand. 
* does only what is indeed expected and needed from developer. 
* can be used in build environments. 
* distributed as Nuget package.
* needs only 2 Attributes to control its logic, which developer can define in own code. 
* needs no extra references. 
* can be used with and without your base class. 
* has fire-and-forget mode - with base class developer can apply attributes only once and all derived classes (including those from another assemblies) will get transformed. 
* supports Universal Windows Platform (UWP) Apps, Silverlight as well as .NET FX (client and extended profiles)

Fastest way to get in touch: http://twitter.com/demigor

##What is it?
Take a look at this painfully familiar code:


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

If you need your private field, you may also write like this:

	[Magic]
	public string Name { get { return _name; } set { _name = value; } }
	string _name;

Or you may apply Magic attribute to a class, and all public properties (including all derived classes) will be transformed.

	[Magic]
	public class MyViewModel: INotifyPropertyChanged
	{
	  public string Name { get; set; }
	  public string LastName { get; set; }
	  .....
	  #region Implementation of INotifyPropertyChanged
	  .... // Your custom implementation required
	  #endregion
	}

To disable transformation, apply NoMagic attribute to a class or property:

	[NoMagic] // explicit stop of implicit property transformation
	public class MyExplicitViewModel: MyViewModel
	{
	  public long Age { get; set; } // will not be transformed implicitly

	  [Magic] // explicit transformation
	  public string FirstName { get; set; }
	  .....
	}

That's exactly what Kind Of Magic does! No more, no less.

##How to use?
1. Define somewhere in your project MagicAttribute and NoMagicAttribute (optional) class, derived from Attribute. Neither visibility nor namespace are relevant. Here is an example, just two lines of code:


    class MagicAttribute: Attribute { }
    class NoMagicAttribute: Attribute { }


2. Apply Magic attribute to public properties in your ViewModel classes, implementing INotifyPropertyChanged. Your class must have accessible void RaisePropertyChanged(string) method. 

3. Apply Magic attribute to ViewModel class if you want all public properties to raise PropertyChanged event. To exclude some of them, apply NoMagic attribute to these properties.

##How it works
1. KindOfMagic build task runs just after compilation.
2. Looks for both MagicAttribute and NoMagicAttribute in building assembly and all referenced user assemblies.
3. Only classes, implementing INotifyPropertyChanged, and their descendants are inspected.
4. Looks for void RaisePropertyChanged(string) method.
5. Transforms all available setters of public properties with MagicAttribute explicitly or implicitly applied.
6. Injects supporting code using Mono.Cecil. Corresponding PDB file is also updated if any. Assembly will be resigned if signing is turned on. Use Reflector to see, what KindOfMagic does for you.

###Installation type 1: NuGet Package Manager
In Package Manager Console enter the following command:

    Install-Package KindOfMagic

###Installation type 2: Build Server/Manual
* Download Binaries from download area. 
* Make somewhere folder KindOfMagic and extract content of the binary archive. 
* Remember the path to KindOfMagic.exe (f.e. z:\KindOfMagic\KindOfMagic.targets)

1. Per-Project
	* Insert following Xml snippet somewhere inside your Project element in your csproj-file and adjust path to KindOfMagic.targets.

		&lt;Import Project="z:\KindOfMagic\KindOfMagic.targets" />

2. Global 
	* Locate Microsoft.CSharp.targets for .NET framework 4
	* Insert the same line:

		&lt;Import Project="z:\KindOfMagic\KindOfMagic.targets" />

##Customization
If for some reason you don't like magic, here are some customization hints for you. You may parameterize names of Magic, NoMagic and RaisePropertyChanged identifiers.

In KindOfMagic.targets file, locate MagicTask xml element and add following attributes:

<MagicTask ....  MagicAttribute="NotifyAttribute" NoMagicAttribute="StopNotifyAttribute" RaiseMethod="OnPropertyChanged"/>

This will use Notify/StopNotify instead of Magic/NoMagic attributes and OnPropertyChanged instead of RaisePropertyChanged method during transformation.

###Beacon method call
 Sometimes, when property setter code is relative complex, you may see this warning produced by KindOfMagic:

Property MyViewModel.MyProperty setter is too complex. Use beacon method (static void Raise()) to indicate point of RaisePropertyChanged injection. 

Usually, this could be seen in setters like this:

	public object MyProperty 
	{
	  get { return _field; }
	  set 
	  {
	     _field = value;

	     if (value != null)
	       DoSomething();
	  }
	}

This happens because in IL, produced by C# compiler, it is not more possible to distinguish normal return (with keyword return) from conditional return. In this case, KindOfMagic redirects all returns to injected RaisePropertyChanged section of the setter, so all returns will raise property changed event. This gives you, as a programer, less control over your code. To solve this issue, I came up with the Beacon method idea.

Define somewhere a static void method Raise without any parameters. Here is an example:

	public class MyViewModel : INotifyPropertyChanged
	{
	  ...
	  [MethodImpl(MethodImplOptions.NoInlining)] // to preserve method call 
	  protected static void Raise() { }
	}

In every problematic setter, place a call to this method just before the last curly brace of your setter. Here is an example:

	public object MyProperty 
	{
	  get { return _field; }
	  set 
	  {
	     _field = value;

	     if (value != null)
	       DoSomething();

	     Raise();
	  }
	}

KindOfMagic will automagically replace Raise method call with RaisePropertyChanged section instead. No return statement will be remapped and no warning message will be emitted.

##Notes
To improve KindOfMagic performance, all types with namespace name started with 'System.' are considered core types and not processed. 

