using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.JMeter
{
    internal class JMeterTool : Tool<JMeterSettings>
    {
        protected internal JMeterTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
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
            if (!settings.ShowGUI)
            {
                sb.Append("-n");
            }

            sb.Append("-t " + settings.SourceJMX);

            if (settings.TargetJTL != null)
            {
                sb.Append("-l " + settings.TargetJTL);
            }

            Run(settings, sb);
        }
    }
}