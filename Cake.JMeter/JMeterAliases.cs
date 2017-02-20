using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.JMeter
{
    /// <summary>
    /// Contains functionality for using the JMeter tool
    /// </summary>
    /// <code>
    /// <![CDATA[
    /// #addin "nuget:?package=Cake.JMeter"
    /// #tool "nuget:?package=JMeter&include=./**/*.bat"
    /// ]]>
    /// </code>
    [CakeAliasCategory("JMeter")]
    public static class JMeterAliases
    {
        /// <summary>
        /// Runs JMeter with the -t parameter with the test file
        /// </summary>
        /// <example>
        /// <para>Use the #addin preprocessor directive and the #tool</para>
        /// <code>
        /// <![CDATA[
        /// #tool "nuget:?package=JMeter&include=./**/*.bat"
        /// #addin "nuget:?package=Cake.JMeter"
        /// ]]>
        /// </code>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("SomeTask").Does(() =>  JMeter("mytests.jmx"));
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="testFile">The JMX Test file.</param>
        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, FilePath testFile)
        {
            JMeter(context, testFile, null);
        }

        /// <summary>
        /// Runs JMeter with the -t parameter with the test file
        /// </summary>
        /// <example>
        /// <para>Use the #addin preprocessor directive and the #tool</para>
        /// <code>
        /// <![CDATA[
        /// #tool "nuget:?package=JMeter&include=./**/*.bat"
        /// #addin "nuget:?package=Cake.JMeter"
        /// ]]>
        /// </code>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("SomeTask").Does(() =>  JMeter("mytests.jmx", "outFile.jml"));
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="testFile">The JMX Test file</param>
        /// <param name="logFile">The output log file</param>
        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, FilePath testFile, FilePath logFile)
        {
            JMeter(context, new JMeterSettings
            {
                TestFile = testFile,
                LogFile = logFile
            });
        }

        /// <summary>
        /// Runs JMeter with the -t parameter with the test file
        /// </summary>
        /// <example>
        /// <para>Use the #addin preprocessor directive and the #tool</para>
        /// <code>
        /// <![CDATA[
        /// #tool "nuget:?package=JMeter&include=./**/*.bat"
        /// #addin "nuget:?package=Cake.JMeter"
        /// ]]>
        /// </code>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("SomeTask").Does(() =>  JMeter(new JMeterSettings
        /// {
        ///     TestFile = "mytests.jmx",
        ///     LogFile = "outFile.jml"
        /// });
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="settings">The JMeterSettings</param>
        [CakeMethodAlias]
        public static void JMeter(this ICakeContext context, JMeterSettings settings)
        {
            var runner = new JMeterTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.RunJMeter(settings);
        }
    }
}