using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;
using Xunit;

namespace Cake.JMeter.Tests
{
    public class AddinTests
    {
        [Fact]
        public void TestFileShouldBeDefined()
        {
            // arrange
            var fixture = new JMeterToolFixture
            {
                Settings = new JMeterSettings()
            };
            //act
            var result = Record.Exception(() => fixture.Run());
            //assert
            Assert.IsType(typeof(NullReferenceException), result);
        }

        [Fact]
        public void ShowGui_Does_Not_Add_N_Flag()
        {
            // arrange
            var fixture = new JMeterToolFixture
            {
                Settings = new JMeterSettings
                {
                    TestFile = "SomeFile",
                    ShowGui = true
                }
            };

            //act
            fixture.Run();

            //assert
            Assert.DoesNotContain("-n", fixture.ProcessRunner.Results.Single().Args);
        }

        [Fact]
        public void ShowGui_Adds_N_Flag()
        {
            // arrange
            var fixture = new JMeterToolFixture
            {
                Settings = new JMeterSettings
                {
                    TestFile = "SomeFile"
                }
            };

            //act
            fixture.Run();

            //assert
            Assert.Equal("-n -t SomeFile", fixture.ProcessRunner.Results.Single().Args);
        }

        [Fact]
        public void LogFile_Adds_L_Flag()
        {
            // arrange
            var fixture = new JMeterToolFixture
            {
                Settings = new JMeterSettings
                {
                    TestFile = "SomeFile",
                    LogFile = FilePath.FromString("LogFile")
                }
            };

            //act
            fixture.Run();

            //assert
            Assert.Equal("-n -t SomeFile -l LogFile", fixture.ProcessRunner.Results.Single().Args);
        }

        [Fact]
        public void WithReports()
        {
            // arrange
            var fixture = new JMeterToolFixture
            {
                Settings = new JMeterSettings
                {
                    TestFile = "SomeFile",
                    GenerateReports = true,
                    ReportOutput = "ReportPath"
                }
            };

            //act
            fixture.Run();

            //assert
            Assert.Equal("-n -t SomeFile -e -o ReportPath", fixture.ProcessRunner.Results.Single().Args);
        }

        [Fact]
        public void WithLocalProperties()
        {
            // arrange
            var fixture = new JMeterToolFixture
            {
                Settings = new JMeterSettings
                {
                    TestFile = "SomeFile",
                    LocalProperties = new Dictionary<string, object> {
                        { "prop1", "text" },
                        { "prop2", 10 }
                    }
                }
            };

            //act
            fixture.Run();

            //assert
            Assert.Equal("-n -t SomeFile -Jprop1=\"text\" -Jprop2=\"10\"", fixture.ProcessRunner.Results.Single().Args);
        }
    }
}