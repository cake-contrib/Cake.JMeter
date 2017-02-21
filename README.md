# Cake.JMeter
An extension to Cake to do this

```cs
#addin "nuget:?package=Cake.JMeter"
#tool "nuget:?package=JMeter&include=./**/*.bat"

JMeter("myTestFile.jmx");
```

[![Build status](https://img.shields.io/appveyor/ci/pitermarx/cake-jmeter.svg)](https://ci.appveyor.com/project/pitermarx/cake-jmeter)
[![NuGet](https://img.shields.io/nuget/v/Cake.JMeter.svg)](https://www.nuget.org/packages/Cake.JMeter/)
