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

        /// <summary>
        /// Installs the given plugin to the JMeter package.
        /// </summary>
        /// <example>
        /// <para>Use the #addin preprocessor directive and the #tool</para>
        /// <code>
        /// <![CDATA[
        /// #tool "nuget:?package=JMeter&version=5.0.0&include=./**/*.bat"
        /// #addin "nuget:?package=Cake.JMeter"
        /// ]]>
        /// </code>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("SomeTask").Does(() => {
        ///     JMeterInstallPlugin("jpgc-casutg", jMeterPath); // Custom Thread Groups
        ///     JMeterInstallPlugin("jpgc-graphs-additional", jMeterPath); // 5 Additional Graphs
        ///     JMeterInstallPlugin("jpgc-graphs-basic", jMeterPath); // 3 Basic Graphs
        ///     JMeterInstallPlugin("jpgc-graphs-composite", jMeterPath); // Composite Timeline Graph
        ///     JMeterInstallPlugin("jpgc-graphs-dist", jMeterPath); // Distribution/Percentile Graphs
        ///     JMeterInstallPlugin("jpgc-mergeresults", jMeterPath); // Merge Results
        ///     JMeterInstallPlugin("jpgc-dummy", jMeterPath); // Dummy Sampler
        ///     JMeterInstallPlugin("jpgc-tst", jMeterPath); // Throughput Shaping Timer
        ///     JMeterInstallPlugin("jpgc-functions", jMeterPath); // Custom JMeter Functions
        ///     JMeterInstallPlugin("jpgc-cmd", jMeterPath); // Command-Line Graph Plotting Tool
        /// });
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pluginName">The name of the plugin to install. Should be the plugin name (ex: jpgc-json)
        /// and can include the version (jpgc-json=2.2). Multiple can be installed at once by separating them with a comma.
        /// </param>
        [CakeMethodAlias]
        public static void JMeterInstallPlugin(this ICakeContext context, string pluginName)
        {
            JMeterRunPluginManagerCommand(context, $"install {pluginName}");
        }

        /// <summary>
        /// Allows to run any command with the JMeter Plugin Manager Command Line.
        /// See https://jmeter-plugins.org/wiki/PluginsManagerAutomated/ for details.
        /// </summary>
        /// <example>
        /// <para>Use the #addin preprocessor directive and the #tool</para>
        /// <code>
        /// <![CDATA[
        /// #tool "nuget:?package=JMeter&version=5.0.0&include=./**/*.bat"
        /// #addin "nuget:?package=Cake.JMeter"
        /// ]]>
        /// </code>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("SomeTask").Does(() => {
        ///     JMeterRunPluginManagerCommand("status");
        /// });
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="commandWithArguments">The full command including all arguments.</param>
        [CakeMethodAlias]
        public static void JMeterRunPluginManagerCommand(this ICakeContext context, string commandWithArguments)
        {
            var pluginManagerCmdPath = context.Tools.Resolve("PluginsManagerCMD.bat");
            var process = context.ProcessRunner.Start(pluginManagerCmdPath, new ProcessSettings { Arguments = commandWithArguments });
            process.WaitForExit();
        }
    }
}
