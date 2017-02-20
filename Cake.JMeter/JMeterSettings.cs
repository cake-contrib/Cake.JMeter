using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.JMeter
{
    public class JMeterSettings : ToolSettings
    {
        public bool ShowGUI { get; set; }
        public FilePath SourceJMX { get; set; }
        public FilePath TargetJTL { get; set; }
    }
}