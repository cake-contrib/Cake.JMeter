# Cake.JMeter
An extension to Cake to do this

```cs
#addin "nuget:?package=Cake.JMeter"
#tool "nuget:?package=JMeter&include=./**/*.bat"

var jmxProject = "myTestFile.jmx";
// Install all needed plugins for the project
JMeterInstallForJmx(jmxProject);
// Run the project
JMeter(jmxProject);
```

[![Build status](https://img.shields.io/appveyor/ci/pitermarx/cake-jmeter.svg)](https://ci.appveyor.com/project/pitermarx/cake-jmeter)
[![NuGet](https://img.shields.io/nuget/v/Cake.JMeter.svg)](https://www.nuget.org/packages/Cake.JMeter/)
[![Coverage Status](https://coveralls.io/repos/github/pitermarx/Cake.JMeter/badge.svg)](https://coveralls.io/github/pitermarx/Cake.JMeter)