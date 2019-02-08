using Cake.Testing.Fixtures;

namespace Cake.JMeter.Tests
{
    public class JMeterToolFixture : ToolFixture<JMeterSettings>
    {
        public JMeterToolFixture() : base("jmeter.bat")
        {
        }

        protected override void RunTool()
        {
            var runner = new JMeterTool(FileSystem, Environment, ProcessRunner, Tools);
            runner.RunJMeter(Settings);
        }
    }
}
