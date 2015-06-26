using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/*


This file contains required KindOfMagic attributes : Magic and NoMagic.
If this project has already declared these attributes (or references another project that does), 
you may remove this file completely.

PropertyChangedBase is an default base class for INotifyPropertyChanged implementation.
Notice, this class is optional. Detailed explanation is here: http://kindofmagic.codeplex.com


*/

namespace $rootnamespace$
{
    /// <summary>
    /// Enables automatic RaisePropertyChanged transformation for class or property
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class MagicAttribute : Attribute { }
    
    /// <summary>
    /// Disables automatic RaisePropertyChanged transformation for class or property
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class NoMagicAttribute : Attribute { }
    
    /// <summary>
    /// Base class for automatic INotifyPropertyChanged transformation in property setters
    /// </summary>
    [Magic]
    public class PropertyChangedBase : INotifyPropertyChanged
    {
      /// <summary>
      /// Indicates RaisePropertyChanged injection point 
      /// </summary>
      [MethodImpl(MethodImplOptions.NoInlining)]
      protected static void Raise() { }
    
      protected virtual void RaisePropertyChanged([CallerMemberName] string prop = "")
      {
        var e = PropertyChanged;
        if (e != null)
          e(this, new PropertyChangedEventArgs(prop)); // you may provide different dispatch login 
      }
    
      public event PropertyChangedEventHandler PropertyChanged;
    }
}    
