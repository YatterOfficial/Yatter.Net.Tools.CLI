## Alpha Alert!

It's a winding road to Camelot so please read the [YatterOfficial](https://github.com/yatterofficial) Overview and Raison d'être.

y@R and You-At-A-Resource are trademarks of Count Anthony Harrison, operating from Cumberland in the United Kingdom

Copyright © 2021

# Yatter.Net.Tools.CLI

<a href="https://www.nuget.org/packages/Yatter.Net.Tools.CLI/" target="_blank" rel="noreferrer noopener"><img alt="Nuget" src="https://img.shields.io/nuget/v/Yatter.Net.Tools.CLI?color=blue&style=for-the-badge"></a>

![GitHub](https://img.shields.io/github/license/yatterofficial/Yatter.Net.Tools.CLI?style=for-the-badge)

[![Yatter.Net.Tools.CLI on fuget.org](https://www.fuget.org/packages/Yatter.Net.Tools.CLI/badge.svg)](https://www.fuget.org/packages/Yatter.Net.Tools.CLI)

## Yatter Content-Management CLI (dotnet tool)

### Installation

- ```dotnet tool install --global Yatter.Net.Tools.CLI --version 0.0.5```

### Overview

This solution is a dotnet tool that is released as a Nuget package (```Yatter.Net.Tools.CLI```) that can be installed using the dotnet CLI, which executes from the cli as ```yatter <service> (arguments)```.

Presently, the cli's capability is:

```
microsite
  The microsite command archives a local Yatter Microsite so that the archive can be transported, or unarchives an archived Yatter Microsite that has been transported. Standard usage: yatter microsite [options]

Usage:
  Yatter.Net.Tools.CLI [options] microsite

Options:
  -p, --pack                 The microsite command's packing switch, optional, however one of -p (--pack) or -u (--unpack) must be specified; instructs the CLI to pack a Yatter Microsite from the current directory
  -u, --unpack               The microsite command's unpacking switch (NOT IMPLEMENTED), optional, however one of -p (--pack) or -u (--unpack) must be specified; instructs the CLI to unpackpack a Yatter Microsite from the current directory
  -a, --yatra                The microsite command's lightweight archiving switch, optional, however one of -a (--yatra) or -z (--yatrz) must be specified; instructs the CLI to archive .yatr files in the current directory, into a lightweight archive 
                             with the file extention .yatra. Files are converted to Base64 and stored with their relative path's in a Document of Type Yatter.UI.ListBuilder.Serialization.Archives.Document, which are collectively stored in a Magazine 
                             of Type Yatter.UI.ListBuilder.Serialization.Archives.Magazine, then serialised into Yatter Object Notation (YON) - a subset of JSON - and then saved in a file with the file extension .yatra
  -z, --yatrz                The microsite command's zip-based archiving switch (NOT IMPLEMENTED), optional, however one of -a (--yatra) or -z (--yatrz) must be specified; instructs the Yatter CLI to recursively archive Yatter documents in a ZIP file 
                             with the file extension .yatrz, which contains a Manifest of the archived contents.
  -f, --filename <filename>  The microsite command's filename switch, mandatory, the input or output filename, depending upon whether packing (-p (--pack)) or unpacking (-u (--unpack).
  -r, --root <root>          The microsite command's RootPath switch, mandatory,  instructs the CLI to assign a path to the archive PathRoot property, indicating where it will be unpacked to. As this can be assigned later in any workflow, the minimum 
                             assignment should be -r null or --root null, which will artificially assign 'PathRoot' : 'null' to the archive's YON Yatter.UI.ListBuilder.Serialization.Archives.Magazine.PathRoot property.
  -v, --verbose              Instructs the CLI to make verbose CLI comments as it executes.
  -?, -h, --help             Show help and usage information
```

## Local

When cloning and building locally from the cloned root directory, the following CLI commands can be used to insall, and uninstall:

- ```dotnet tool install --global --add-source ./nupkg Yatter.Net.Tools.CLI```
- ```dotnet tool uninstall -g Yatter.Net.Tools.CLI```




