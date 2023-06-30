using System.Windows;
using System.Windows.Controls;

namespace APAS.Plugin.KEYTHLEY._2600_PD.Views
{
    public partial class PluginDemoView : UserControl
    {
        public PluginDemoView()
        {
            InitializeComponent();

            // once the datacontext is set, register the corresponding event to blink the indicator.
            DataContextChanged += PluginDemoView_DataContextChanged;
        }

        private void PluginDemoView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Keithley2600_PD plugin)
            {
                plugin.OnCommShot += (s, arg) =>
                {
                   blinkIndicator.Blink();
                };
            }
        }
    }
}
