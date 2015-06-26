using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace KindOfMagic.Tests
{
  [TestClass]
  public class Test
  {
    void DoTest<T>(Action<Example, T> setter1, Action<TestMagic, T> setter2, string propName, T value1, T value2)
    {
      setter1(_test1, value1);
      setter2(_test2, value1);

      setter1(_test1, value2);
      setter2(_test2, value2);

      Assert.AreEqual(_test1.LastPropertyChanged, propName);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);

      Reset();

      setter1(_test1, value2);
      setter2(_test2, value2);

      Assert.AreEqual(_test1.LastPropertyChanged, null);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);

      setter1(_test1, value1);
      setter2(_test2, value1);

      Assert.AreEqual(_test1.LastPropertyChanged, propName);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);
    }

    void DoTestN<T>(Action<Example, T?> setter1, Action<TestMagic, T?> setter2, string propName, T value1, T value2) where T : struct
    {
      setter1(_test1, null);
      setter2(_test2, null);

      Reset();

      setter1(_test1, null);
      setter2(_test2, null);

      Assert.AreEqual(_test1.LastPropertyChanged, null);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);

      setter1(_test1, value2);
      setter2(_test2, value2);

      Assert.AreEqual(_test1.LastPropertyChanged, propName);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);

      Reset();

      setter1(_test1, value2);
      setter2(_test2, value2);

      Assert.AreEqual(_test1.LastPropertyChanged, null);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);

      setter1(_test1, value1);
      setter2(_test2, value1);

      Assert.AreEqual(_test1.LastPropertyChanged, propName);
      Assert.AreEqual(_test1.LastPropertyChanged, _test2.LastPropertyChanged);
    }

    void Reset()
    {
      _test1.LastPropertyChanged = null;
      _test2.LastPropertyChanged = null;
    }

    Example _test1 = new Example();
    TestMagic _test2 = new TestMagic();

    [TestMethod]
    public void TestBool()
    {
      DoTest((i, v) => i.PropBool = v, (i, v) => i.PropBool = v, "PropBool", false, true);
    }

    [TestMethod]
    public void TestInt()
    {
      DoTest((i, v) => i.PropInt = v, (i, v) => i.PropInt = v, "PropInt", 10, 20);
    }

    [TestMethod]
    public void TestLong()
    {
      DoTest((i, v) => i.PropLong = v, (i, v) => i.PropLong = v, "PropLong", 20, 31);
    }

    [TestMethod]
    public void TestFloat()
    {
      DoTest((i, v) => i.PropFloat = v, (i, v) => i.PropFloat = v, "PropFloat", 5F, 10F);
    }

    [TestMethod]
    public void TestDouble()
    {
      DoTest((i, v) => i.PropDouble = v, (i, v) => i.PropDouble = v, "PropDouble", 5D, 10D);
    }

    [TestMethod]
    public void TestDecimal()
    {
      DoTest((i, v) => i.PropDecimal = v, (i, v) => i.PropDecimal = v, "PropDecimal", 5M, 10M);
    }

    [TestMethod]
    public void TestTime()
    {
      DoTest((i, v) => i.PropTime = v, (i, v) => i.PropTime = v, "PropTime", TimeSpan.MinValue, DateTime.Now.TimeOfDay);
    }

    [TestMethod]
    public void TestDate()
    {
      DoTest((i, v) => i.PropDate = v, (i, v) => i.PropDate = v, "PropDate", DateTime.MinValue, DateTime.Now);
    }

    [TestMethod]
    public void TestGuid()
    {
      DoTest((i, v) => i.PropGuid = v, (i, v) => i.PropGuid = v, "PropGuid", Guid.NewGuid(), Guid.NewGuid());
    }

    [TestMethod]
    public void TestEnum()
    {
      DoTest((i, v) => i.PropEnum = v, (i, v) => i.PropEnum = v, "PropEnum", EnumTest.One, EnumTest.Two);
    }

    [TestMethod]
    public void TestColor()
    {
      DoTest((i, v) => i.PropColor = v, (i, v) => i.PropColor = v, "PropColor", Color.FromArgb(1, 2, 3, 4), Color.FromArgb(10, 20, 30, 40));
    }

    [TestMethod]
    public void TestBoolN()
    {
      DoTestN((i, v) => i.PropBoolN = v, (i, v) => i.PropBoolN = v, "PropBoolN", false, true);
    }

    [TestMethod]
    public void TestIntN()
    {
      DoTestN((i, v) => i.PropIntN = v, (i, v) => i.PropIntN = v, "PropIntN", 10, 20);
    }

    [TestMethod]
    public void TestLongN()
    {
      DoTestN((i, v) => i.PropLongN = v, (i, v) => i.PropLongN = v, "PropLongN", 20L, 31L);
    }

    [TestMethod]
    public void TestFloatN()
    {
      DoTestN((i, v) => i.PropFloatN = v, (i, v) => i.PropFloatN = v, "PropFloatN", 5F, 10F);
    }

    [TestMethod]
    public void TestDoubleN()
    {
      DoTestN((i, v) => i.PropDoubleN = v, (i, v) => i.PropDoubleN = v, "PropDoubleN", 5D, 10D);
    }

    [TestMethod]
    public void TestDecimalN()
    {
      DoTestN((i, v) => i.PropDecimalN = v, (i, v) => i.PropDecimalN = v, "PropDecimalN", 5M, 10M);
    }

    [TestMethod]
    public void TestTimeN()
    {
      DoTestN((i, v) => i.PropTimeN = v, (i, v) => i.PropTimeN = v, "PropTimeN", TimeSpan.MinValue, DateTime.Now.TimeOfDay);
    }

    [TestMethod]
    public void TestDateN()
    {
      DoTestN((i, v) => i.PropDateN = v, (i, v) => i.PropDateN = v, "PropDateN", DateTime.MinValue, DateTime.Now);
    }

    [TestMethod]
    public void TestGuidN()
    {
      DoTestN((i, v) => i.PropGuidN = v, (i, v) => i.PropGuidN = v, "PropGuidN", Guid.NewGuid(), Guid.NewGuid());
    }

    [TestMethod]
    public void TestEnumN()
    {
      DoTestN((i, v) => i.PropEnumN = v, (i, v) => i.PropEnumN = v, "PropEnumN", EnumTest.One, EnumTest.Two);
    }

    [TestMethod]
    public void TestString()
    {
      DoTest((i, v) => i.PropString = v, (i, v) => i.PropString = v, "PropString", string.Empty, "TestValue");
    }

    [TestMethod]
    public void TestComplex1()
    {
      DoTest((i, v) => i.PropComplex1 = v, (i, v) => i.PropComplex1 = v, "PropComplex1", string.Empty, "TestValue");
    }

    [TestMethod]
    public void TestComplex2()
    {
      DoTest((i, v) => i.PropComplex2 = v, (i, v) => i.PropComplex2 = v, "PropComplex2", string.Empty, "TestValue");
    }

    [TestMethod]
    public void TestStringArray1()
    {
      var array = new[] { "blabla" };

      DoTest((i, v) => i.PropStringArray1 = v, (i, v) => i.PropStringArray1 = v, "PropStringArray1", null, array);
    }

    [TestMethod]
    public void TestStringArray2()
    {
      var array = new[] { "blabla" };

      DoTest((i, v) => i.PropStringArray2 = v, (i, v) => i.PropStringArray2 = v, "PropStringArray2", null, array);
    }

    [TestMethod]
    public void TestObject()
    {
      DoTest<object>((i, v) => i.PropObject = v, (i, v) => i.PropObject = v, "PropObject", _test1, _test2);
    }

    [TestMethod]
    public void TestList()
    {
      var list = new List<object> { 1, 2, 3, "bla" };

      DoTest((i, v) => i.List = v, (i, v) => i.List = v, "List", null, list);
    }

#if !PCL
    [TestMethod]
    public void TestCollection()
    {
      var collection = new ObservableCollection<object> { 1, 2, 3 };

      DoTest((i, v) => i.Collection = v, (i, v) => i.Collection = v, "Collection", null, collection);
    }
#endif
  }
}
