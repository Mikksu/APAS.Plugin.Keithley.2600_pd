namespace APAS.Plugin.KEYTHLEY.SMU2600.Models
{
    internal class MonitorReporter
    {
        public MonitorReporter(
            SourceModeEnum mode,
            bool isON,
            double sourceV,
            double sourceI,
            double measureV,
            double measureA)
        {
            Mode = mode;
            IsON = isON;
            SourceV = sourceV;
            SourceI = sourceI;
            MeasureV = measureV;
            MeasureA = measureA;
        }

        public MonitorReporter()
        {
            
        }

        internal SourceModeEnum Mode { get; set; }

        internal bool IsON { get; set; }

        internal double SourceV { get; set; }

        internal double SourceI{ get; set; }

        internal double MeasureV { get; set; }

        internal double MeasureA { get; set; }
    }
}
