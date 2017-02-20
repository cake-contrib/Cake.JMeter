using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.JMeter
{
    /// <summary>
    /// Settings for the aliases
    /// </summary>
    public class JMeterSettings : ToolSettings
    {
        /// <summary>
        /// If false adds the -n cmdline parameter.
        /// </summary>
        public bool ShowGui { get; set; }

        /// <summary>
        /// The input jmx file for the -t cmdline parameter
        /// </summary>
        public FilePath TestFile { get; set; }

        /// <summary>
        /// The -l cmdline parameter file path
        /// </summary>
        public FilePath LogFile { get; set; }

        /// <summary>
        /// If true sets the -e parameter
        /// </summary>
        public bool GenerateReports { get; set; }

        /// <summary>
        /// The -o cmdline parameter directory
        /// </summary>
        public DirectoryPath ReportOutput { get; set; }
    }
}