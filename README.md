[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![made-with-docsify](https://img.shields.io/badge/Made%20with-Docsify-green.svg)](https://docsify.js.org/)
[![made-with-dotnet-core](https://img.shields.io/badge/Made%20with-DotNet%20Core%202.2-blue.svg)](https://shields.io)

# Introduction

An example .Net project that does a rank check for websites against a specific search term.

For cross-platform compatibility, this application was developed using the `DotNet CLI`, using `Visual Studio Code`. However, `Visual Studio` can be used to run the applications, and instructions has been provided in the documentation.

# Features

* Cross-Platform Project
* Web API Documentation
    * Using Swashbuckle
* Highly documented code
    * Intellisense is able to pick up these code documentations.
    * Used Doxygen to generate documentation for the non-Web API projects.
* Rank checking available for multiple search engines
    * Google
    * Yahoo
    * Bing
* History of Rank Check Requests
* Background Jobs
    * Rank searching is as a background job, as the rank check process can take some amount of time.
* Automated Unit Tests

# Documentation

The full documentation can be viewed on the [GitHub pages](https://deltoss.github.io/mitchrankchecker/).