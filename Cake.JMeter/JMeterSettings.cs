using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.JMeter
{
    public class JMeterSettings : ToolSettings
    {
        public bool ShowGUI { get; set; }
        public FilePath TestFile { get; set; }
        public FilePath LogFile { get; set; }
        public bool GenerateReports { get; set; }
        public FilePath ReportOutput { get; set; }
    }
}