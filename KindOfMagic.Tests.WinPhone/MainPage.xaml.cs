using Microsoft.Phone.Controls;
using Microsoft.Phone.Testing;

namespace KindOfMagic.Tests.WinPhone
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();

      Content = UnitTestSystem.CreateTestPage();
    }
  }
}