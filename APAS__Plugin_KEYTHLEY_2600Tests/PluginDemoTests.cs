using APAS.Plugin.KEYTHLEY._2600_PD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace APAS__Plugin_KEYTHLEY_2600_PDTests
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