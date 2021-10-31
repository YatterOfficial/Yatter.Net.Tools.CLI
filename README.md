## Alpha Alert!

It's a winding road to Camelot so please read the [YatterOfficial](https://github.com/yatterofficial) Overview and Raison d'être.

y@R and You-At-A-Resource are trademarks of Count Anthony Harrison, operating from the United Kingdom

Copyright © 2021

# Yatter.Net.Tools.CLI

<a href="https://www.nuget.org/packages/Yatter.Net.Tools.CLI/" target="_blank" rel="noreferrer noopener"><img alt="Nuget" src="https://img.shields.io/nuget/v/Yatter.Net.Tools.CLI?color=blue&style=for-the-badge"></a>

![GitHub](https://img.shields.io/github/license/yatterofficial/Yatter.Net.Tools.CLI?style=for-the-badge)

[![Yatter.Net.Tools.CLI on fuget.org](https://www.fuget.org/packages/Yatter.Net.Tools.CLI/badge.svg)](https://www.fuget.org/packages/Yatter.Net.Tools.CLI)

## Yatter Content-Management CLI (dotnet tool)

### Installation

- ```dotnet tool install --global Yatter.Net.Tools.CLI --version 0.0.2```

### Overview

This solution is a dotnet tool that is released as a Nuget package (```Yatter.Net.Tools.CLI```) that can be installed using the dotnet CLI, which executes from the cli as ```yatter <service> (arguments)```.

Presently, the cli's capability is:

- ```yatter microsite``` (arguments), which has the purpose of archiving a local Yatter Microsite so that the archive can be transported and then unarchived elsewhere.
  - ```-p``` (```--pack```), optional, however one of ```-p``` (```--pack```) or ```-u``` (```--unpack```) must be specified; instructs the CLI to pack a Yatter Microsite from the cureent directory
  - ```-u``` (```--unpack```), NOT YET IMPLEMENTED - optional, however one of ```-p``` (```--pack```) or ```-u``` (```--unpack```) must be specified; instructs the CLI to pack a Yatter Microsite from the cureent directory
  - ```-a``` (```--yatra```), optional, however one of ```-a``` (```--yatra```) or ```-z``` (```--yatrz```) must be specified; instructs the CLI to archive ```.yatr``` files in the current directory, into a lightweight archive with the file extention ```.yatra```. Files are converted to Base64 and stored with their relative path's in a ```Document``` of Type ```Yatter.UI.ListBuilder.Serialization.Archives.Document```, which are collectively stored in a ```Magazine``` of Type ```Yatter.UI.ListBuilder.Serialization.Archives.Magazine```, then serialised into ```Yatter Object Notation (YON)``` - a subset of ```JSON``` - and then saved in a file with the file extension ```.yatra```
  - ```-z``` (```--yatrz```), NOT YET IMPLEMENTED - optional, however one of ```-a``` (```--yatra```) or ```-z``` (```--yatrz```) must be specified; instructs the Yatter CLI to recursively archive Yatter documents in a ZIP file with the file extension ```.yatrz```, which contains a Manifest of the archived contents.
  - ```-f``` (```--filename```), mandatory, the output filename.
  - ```-v``` (```--verbose```), optional, instructs the CLI to make verbose CLI comments as it executes.




