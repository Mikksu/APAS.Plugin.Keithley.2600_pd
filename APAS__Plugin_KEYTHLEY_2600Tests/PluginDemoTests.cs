using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Tests
{
    [TestClass()]
    public class PluginDemoTests
    {
        [TestMethod()]
        public void InitTest()
        {
            var plugin = new Keithley2600(null, "Keithley 2600");

            var win = new Window
            {
                Content = plugin.UserView,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            win.ShowDialog();
        }
    }
}