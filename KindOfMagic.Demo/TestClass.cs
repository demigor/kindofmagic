using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace KindOfMagic
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
  class MagicAttribute : Attribute { }

  public enum EnumTest { One, Two, Three }

  [StructLayout(LayoutKind.Sequential)]
  public struct Color : IFormattable
  {
    MILColor sRgbColor;

    public static Color FromArgb(byte a, byte r, byte g, byte b)
    {
      var color = new Color();
      color.sRgbColor.a = a;
      color.sRgbColor.r = r;
      color.sRgbColor.g = g;
      color.sRgbColor.b = b;
      return color;
    }

    public override string ToString()
    {
      return this.ConvertToString(null, null);
    }

    public string ToString(IFormatProvider provider)
    {
      return this.ConvertToString(null, provider);
    }

    string IFormattable.ToString(string format, IFormatProvider provider)
    {
      return this.ConvertToString(format, provider);
    }

    internal string ConvertToString(string format, IFormatProvider provider)
    {
      return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", sRgbColor.a, sRgbColor.r, sRgbColor.g, sRgbColor.b);
    }

    public override int GetHashCode()
    {
      return this.sRgbColor.GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (o is Color)
      {
        Color color = (Color)o;
        return (this == color);
      }
      return false;
    }

    public bool Equals(Color color)
    {
      return (this == color);
    }

    public static bool operator ==(Color color1, Color color2)
    {
      if (color1.R != color2.R)
      {
        return false;
      }
      if (color1.G != color2.G)
      {
        return false;
      }
      if (color1.B != color2.B)
      {
        return false;
      }
      if (color1.A != color2.A)
      {
        return false;
      }
      return true;
    }

    public static bool operator !=(Color color1, Color color2)
    {
      return !(color1 == color2);
    }

    public byte A
    {
      get
      {
        return this.sRgbColor.a;
      }
      set
      {
        this.sRgbColor.a = value;
      }
    }
    public byte R
    {
      get
      {
        return this.sRgbColor.r;
      }
      set
      {
        this.sRgbColor.r = value;
      }
    }
    public byte G
    {
      get
      {
        return this.sRgbColor.g;
      }
      set
      {
        this.sRgbColor.g = value;
      }
    }
    public byte B
    {
      get
      {
        return this.sRgbColor.b;
      }
      set
      {
        this.sRgbColor.b = value;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MILColor
    {
      public byte a;
      public byte r;
      public byte g;
      public byte b;
    }
  }

  interface INotifyPropertyTest : INotifyPropertyChanged
  {
    int PropInt { get; set; }
    int? PropIntN { get; set; }

    long PropLong { get; set; }
    long? PropLongN { get; set; }

    object PropObject { get; set; }
    string PropString { get; set; }

    bool PropBool { get; set; }
    bool? PropBoolN { get; set; }

    float PropFloat { get; set; }
    float? PropFloatN { get; set; }

    double PropDouble { get; set; }
    double? PropDoubleN { get; set; }

    decimal PropDecimal { get; set; }
    decimal? PropDecimalN { get; set; }

    TimeSpan PropTime { get; set; }
    TimeSpan? PropTimeN { get; set; }

    DateTime PropDate { get; set; }
    DateTime? PropDateN { get; set; }

    string PropComplex1 { get; set; }
    string PropComplex2 { get; set; }

    string[] PropStringArray1 { get; set; }
    string[] PropStringArray2 { get; set; }

    EnumTest PropEnum { get; set; }
    EnumTest? PropEnumN { get; set; }

    Color PropColor { get; set; }
    Guid PropGuid { get; set; }
    Guid? PropGuidN { get; set; }

    List<object> List { get; set; }

#if !PCL
    ObservableCollection<object> Collection { get; set; }
#endif
  }

  public class Example : INotifyPropertyTest
  {
    public string LastPropertyChanged;

    protected void RaisePropertyChanged(string propName)
    {
      var e = PropertyChanged;
      if (e != null)
        e(this, new PropertyChangedEventArgs(propName));

      LastPropertyChanged = propName;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #region int
    int _int;
    public int PropInt
    {
      get
      {
        return _int;
      }
      set
      {
        if (_int == value) return;
        _int = value;
        RaisePropertyChanged("PropInt");
      }
    }

    #endregion

    #region int?
    int? _intn;
    public int? PropIntN
    {
      get
      {
        return _intn;
      }
      set
      {
        if (_intn == value) return;
        _intn = value;
        RaisePropertyChanged("PropIntN");
      }
    }

    #endregion

    #region long
    long _long;
    public long PropLong
    {
      get
      {
        return _long;
      }
      set
      {
        if (_long == value) return;
        _long = value;
        RaisePropertyChanged("PropLong");
      }
    }

    #endregion

    #region long?
    long? _longn;
    public long? PropLongN
    {
      get
      {
        return _longn;
      }
      set
      {
        if (_longn == value) return;
        _longn = value;
        RaisePropertyChanged("PropLongN");
      }
    }

    #endregion

    #region object

    object _obj;
    public object PropObject
    {
      get { return _obj; }
      set
      {
        if (_obj == value) return;
        _obj = value;
        RaisePropertyChanged("PropObject");
      }
    }

    #endregion

    #region double
    double _double;
    public double PropDouble
    {
      get { return _double; }
      set
      {
        if (_double == value) return;
        _double = value;
        RaisePropertyChanged("PropDouble");
      }
    }

    #endregion

    #region double?
    double? _doublen;
    public double? PropDoubleN
    {
      get { return _doublen; }
      set
      {
        var hasValue = value.HasValue;

        if ((_doublen.HasValue == hasValue) && (!hasValue || _doublen.GetValueOrDefault() == value.GetValueOrDefault()))
          return;

        _doublen = value;

        RaisePropertyChanged("PropDoubleN");
      }
    }

    #endregion

    #region float
    float _float;
    public float PropFloat
    {
      get { return _float; }
      set
      {
        if (_float == value) return;
        _float = value;
        RaisePropertyChanged("PropFloat");
      }
    }

    #endregion

    #region float?
    float? _floatn;
    public float? PropFloatN
    {
      get { return _floatn; }
      set
      {
        if (_floatn == value) return;
        _floatn = value;
        RaisePropertyChanged("PropFloatN");
      }
    }

    #endregion

    #region object

    string _string;
    public string PropString
    {
      get { return _string; }
      set
      {
        if (_string == value) return;
        _string = value;
        RaisePropertyChanged("PropString");
      }
    }

    #endregion

    #region bool
    bool _bool;
    public bool PropBool
    {
      get { return _bool; }
      set
      {
        if (_bool == value) return;
        _bool = value;
        RaisePropertyChanged("PropBool");
      }
    }

    #endregion

    #region bool?
    bool? _booln;
    public bool? PropBoolN
    {
      get { return _booln; }
      set
      {
        if (_booln == value) return;
        _booln = value;
        RaisePropertyChanged("PropBoolN");
      }
    }

    #endregion

    #region decimal
    decimal _decimal;
    public decimal PropDecimal
    {
      get { return _decimal; }
      set
      {
        if (_decimal == value) return;
        _decimal = value;
        RaisePropertyChanged("PropDecimal");
      }
    }

    #endregion

    #region decimal?
    decimal? _decimaln;
    public decimal? PropDecimalN
    {
      get { return _decimaln; }
      set
      {
        if (_decimaln == value) return;
        _decimaln = value;
        RaisePropertyChanged("PropDecimalN");
      }
    }

    #endregion

    #region TimeSpan
    TimeSpan _time;
    public TimeSpan PropTime
    {
      get
      {
        return _time;
      }
      set
      {
        if (_time == value) return;
        _time = value;
        RaisePropertyChanged("PropTime");
      }
    }

    #endregion

    #region TimeSpan?

    TimeSpan? _timen;
    public TimeSpan? PropTimeN
    {
      get
      {
        return _timen;
      }
      set
      {
        if (_timen == value) return;
        _timen = value;
        RaisePropertyChanged("PropTimeN");
      }
    }

    #endregion

    #region DateTime

    DateTime _date;
    public DateTime PropDate
    {
      get
      {
        return _date;
      }
      set
      {
        if (_date == value) return;
        _date = value;
        RaisePropertyChanged("PropDate");
      }
    }

    #endregion

    #region DateTime?

    DateTime? _daten;
    public DateTime? PropDateN
    {
      get
      {
        return _daten;
      }
      set
      {
        if (_daten == value) return;
        _daten = value;
        RaisePropertyChanged("PropDateN");
      }
    }

    #endregion

    #region Guid

    Guid _guid;
    public Guid PropGuid
    {
      get
      {
        return _guid;
      }
      set
      {
        if (_guid == value) return;
        _guid = value;
        RaisePropertyChanged("PropGuid");
      }
    }


    #endregion

    #region Guid?

    Guid? _guidn;
    public Guid? PropGuidN
    {
      get
      {
        return _guidn;
      }
      set
      {
        if (_guidn == value) return;
        _guidn = value;
        RaisePropertyChanged("PropGuidN");
      }
    }

    #endregion

    #region Complex cases

    string _propComplex1;
    public string PropComplex1
    {
      get { return _propComplex1; }
      set
      {
        if (_propComplex1 == value) return;

        _propComplex1 = value;

        if (value == null)
          PropBool = true;
        else
          PropBoolN = true;

        RaisePropertyChanged("PropComplex1");
      }
    }

    string _propComplex2;
    public string PropComplex2
    {
      get { return _propComplex2; }
      set
      {
        if (_propComplex2 == value) return;

        _propComplex2 = value;

        if (value == null)
          return;

        PropBoolN = true;

        RaisePropertyChanged("PropComplex2");
      }
    }

    #endregion

    #region StringArray casees

    string[] _propStringArray1;
    public string[] PropStringArray1
    {
      get { return _propStringArray1; }
      set
      {
        if (_propStringArray1 == value) return;

        _propStringArray1 = value;

        RaisePropertyChanged("PropStringArray1");
      }
    }

    string[] _propStringArray2;
    public string[] PropStringArray2
    {
      get { return _propStringArray2 ?? (_propStringArray2 = NewStringArray()); }
      set
      {
        if (_propStringArray2 == value) return;

        _propStringArray2 = value;

        RaisePropertyChanged("PropStringArray2");
      }
    }

    string[] NewStringArray()
    {
      return new[] { "bla" };
    }

    #endregion

    List<object> _list;
    public List<object> List
    {
      get
      {
        return _list;
      }
      set
      {
        if (_list == value) return;

        _list = value;
        RaisePropertyChanged("List");
      }
    }

#if !PCL
    ObservableCollection<object> _collection;
    public ObservableCollection<object> Collection
    {
      get
      {
        return _collection;
      }
      set
      {
        if (_collection == value) return;

        _collection = value;
        RaisePropertyChanged("Collection");
      }
    }
#endif

    EnumTest _propEnum;
    public EnumTest PropEnum
    {
      get { return _propEnum; }
      set
      {
        if (_propEnum == value) return;
        _propEnum = value;
        RaisePropertyChanged("PropEnum");
      }
    }

    EnumTest? _propEnumN;
    public EnumTest? PropEnumN
    {
      get { return _propEnum; }
      set
      {
        if (_propEnumN == value) return;
        _propEnumN = value;
        RaisePropertyChanged("PropEnumN");
      }
    }

    Color _color;
    public Color PropColor
    {
      get { return _color; }
      set
      {
        if (_color == value) return;
        _color = value;
        RaisePropertyChanged("PropColor");
      }
    }

  }

  [Magic]
  public class TestMagic : INotifyPropertyTest
  {
    public string LastPropertyChanged;

    protected void RaisePropertyChanged(string propName)
    {
      var e = PropertyChanged;
      if (e != null)
        e(this, new PropertyChangedEventArgs(propName));

      LastPropertyChanged = propName;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public int PropInt { get; set; }
    public int? PropIntN { get; set; }
    public long PropLong { get; set; }
    public long? PropLongN { get; set; }
    public object PropObject { get; set; }
    public string PropString { get; set; }
    public bool PropBool { get; set; }
    public bool? PropBoolN { get; set; }
    public float PropFloat { get; set; }
    public float? PropFloatN { get; set; }
    public double PropDouble { get; set; }
    public double? PropDoubleN { get; set; }
    public decimal PropDecimal { get; set; }
    public decimal? PropDecimalN { get; set; }
    public TimeSpan PropTime { get; set; }
    public TimeSpan? PropTimeN { get; set; }
    public DateTime PropDate { get; set; }
    public DateTime? PropDateN { get; set; }
    public EnumTest PropEnum { get; set; }
    public EnumTest? PropEnumN { get; set; }
    public Color PropColor { get; set; }
    public Guid PropGuid { get; set; }
    public Guid? PropGuidN { get; set; }
    public List<object> List { get; set; }

#if !PCL
    public ObservableCollection<object> Collection { get; set; }
#endif

    string _propComplex1;
    public string PropComplex1
    {
      get { return _propComplex1; }
      set
      {
        _propComplex1 = value;

        if (value == null)
          PropBool = true;
        else
          PropBoolN = true;

        Raise();
      }
    }

    string _propComplex2;
    public string PropComplex2
    {
      get { return _propComplex2; }
      set
      {
        _propComplex2 = value;

        if (value == null)
          return;

        PropBoolN = true;

        Raise();
      }
    }

    public string[] PropStringArray1 { get; set; }

    string[] _propStringArray2;
    public string[] PropStringArray2
    {
      get
      {
        // testing proper detection of lazy initialized field
        return _propStringArray2 ?? (_propStringArray2 = NewStringArray());
      }
      set
      {
        _propStringArray2 = value;
      }
    }

    string[] NewStringArray()
    {
      return new[] { "bla" };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void Raise() { }
  }
}
