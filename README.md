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

## Give a Star! :star:

If you like or are using this project please give it a star. Thanks!

[![Build status](https://img.shields.io/appveyor/ci/pitermarx/cake-jmeter.svg)](https://ci.appveyor.com/project/pitermarx/cake-jmeter)
[![NuGet](https://img.shields.io/nuget/v/Cake.JMeter.svg)](https://www.nuget.org/packages/Cake.JMeter/)
[![Coverage Status](https://coveralls.io/repos/github/pitermarx/Cake.JMeter/badge.svg)](https://coveralls.io/github/pitermarx/Cake.JMeter)

## Discussion

For questions and to discuss ideas & feature requests, use the [GitHub discussions on the Cake GitHub repository](https://github.com/cake-build/cake/discussions), under the [Extension Q&A](https://github.com/cake-build/cake/discussions/categories/extension-q-a) category.

[![Join in the discussion on the Cake repository](https://img.shields.io/badge/GitHub-Discussions-green?logo=github)](https://github.com/cake-build/cake/discussions)

## Release History

Click on the [Releases](https://github.com/cake-contrib/Cake.JMeter/releases) tab on GitHub.

---

_Copyright &copy; 2017-2021 Cake Contributors - Provided under the [MIT License](LICENSE)._
