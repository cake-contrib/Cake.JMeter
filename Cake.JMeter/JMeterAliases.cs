using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.JMeter
{
    [CakeAliasCategory("JMeter")]
    public static class JMeterAliases
    {
        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, FilePath sourceJmx)
        {
            JMeter(context, sourceJmx, null, false);
        }

        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, FilePath sourceJmx, bool showGui)
        {
            JMeter(context, sourceJmx, null, showGui);
        }

        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, FilePath sourceJmx, FilePath targetJtl)
        {
            JMeter(context, sourceJmx, targetJtl, false);
        }

        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, FilePath sourceJmx, FilePath targetJtl, bool showGui)
        {
            var runner = new JMeterTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.RunJMeter(new JMeterSettings
            {
                SourceJMX = sourceJmx,
                TargetJTL = targetJtl,
                ShowGUI = showGui
            });
        }
    }
}