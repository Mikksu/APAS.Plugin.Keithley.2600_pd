namespace APAS.Plugin.KEYTHLEY.SMU2600.Models
{
    internal class MonitorReporter
    {
        public MonitorReporter(
            SourceModeEnum mode,
            double sourceV,
            double sourceA,
            double measureV,
            double measureA)
        {
            Mode = mode;
            SourceV = sourceV;
            SourceA = sourceA;
            MeasureV = measureV;
            MeasureA = measureA;
        }

        public MonitorReporter()
        {
            
        }

        internal SourceModeEnum Mode { get; set; }

        internal double SourceV { get; set; }

        internal double SourceA{ get; set; }

        internal double MeasureV { get; set; }

        internal double MeasureA { get; set; }
    }
}
