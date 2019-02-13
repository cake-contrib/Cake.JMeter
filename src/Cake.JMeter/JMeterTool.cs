using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.JMeter
{
    internal class JMeterTool : Tool<JMeterSettings>
    {
        private readonly ICakeEnvironment _environment;

        protected internal JMeterTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        protected sealed override string GetToolName()
        {
            return "JMeter";
        }

        protected sealed override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "jmeter.bat", "jmeter" };
        }

        protected internal void RunJMeter(JMeterSettings settings)
        {
            var sb = new ProcessArgumentBuilder();
            if (!settings.ShowGui)
            {
                sb.Append("-n");
            }

            sb.Append("-t " + settings.TestFile.MakeAbsolute(_environment).FullPath.Quote());

            if (settings.LogFile != null)
            {
                sb.Append("-l " + settings.LogFile.MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.GenerateReports)
            {
                sb.Append("-e");
            }

            if (settings.ReportOutput != null)
            {
                sb.Append("-o " + settings.ReportOutput.MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.LocalProperties?.Any() == true)
            {
                foreach (var localProperty in settings.LocalProperties)
                {
                    sb.Append($"-J{localProperty.Key}={localProperty.Value.ToString().Quote()}");
                }
            }

            Run(settings, sb);
        }
    }
}
