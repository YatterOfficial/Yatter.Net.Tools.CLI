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

#### Global

- ```dotnet tool install --global Yatter.Net.Tools.CLI --version 0.0.9```

#### Local

- ```dotnet new tool-manifest # if you are setting up this repo```
- ```dotnet tool install --local Yatter.Net.Tools.CLI --version 0.0.9```

### Overview

This solution is a dotnet tool that is released as a Nuget package (```Yatter.Net.Tools.CLI```) that can be installed using the dotnet CLI, which executes from the cli as ```yatter [command] [options]```.

The nuget package is created into the ```./nupkg``` directory.

Presently, the cli's capability is:

- ```yatter microsite [options]```
- ```yatter cryptography [option]```
  - y@R App Alpha Content Creators, please note that the following CLI commands can be used to obtain the current public key for uploading user content to the app (you can remove or utilize -s (silent) or -v (verbose), according to your need):
    - ```yatter cryptography -g -u publickeys.yatr.me -n userupload -s```
    - ```yatter cryptography -g -u publickeys.yatr.me -n userupload -v```
    - ```yatter cryptography -g -u publickeys.yatr.me -n userupload -o "PublicKey.txt" -s```
    - ```yatter cryptography -g -u publickeys.yatr.me -n userupload -o "PublicKey.txt" -v```
    - Help: ```yatter cryptography -h```

```
microsite
  The microsite command archives a local Yatter Microsite so that the archive can be transported, or unarchives an archived Yatter Microsite that has been transported. Standard usage: yatter microsite [options]

Usage:
  Yatter.Net.Tools.CLI [options] microsite

Options:
  -p, --pack                 The microsite command's packing switch, optional, however one of -p (--pack) or -u (--unpack) must be specified; instructs the CLI to pack a Yatter Microsite from the current directory
  -u, --unpack               The microsite command's unpacking switch (.yatra implemented but .yatrz NOT IMPLEMENTED), optional, however one of -p (--pack) or -u (--unpack) must be specified; instructs the CLI to unpack a Yatter Microsite from the 
                             current directory
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

```
cryptography
  RSA Public and Private key encryption, using only .NET Code from System.Security.Cryptography to create keys, to both encrypt and decrypt using those keys, and to retrieve public keys from DNS TXT records, and public key servers. Standard usage: yatter 
  cryptography [options]

Usage:
  Yatter.Net.Tools.CLI [options] cryptography

Options:
  -c, --createkeypair           The cryptography command's switch to indicate that it will create an RSA Public and Private Key Pair, in Base64 format, outputting the private key to a filepath indicated by the switch -P (--privatekeyfilename), and outputting 
                                the public key to a filepath indicated by the switch -p (--publickeyfilename); optional. Path specified can be to a file in the current directory, relative to the current directory, or absolute.
  -p, --publickeyfilename <p>   The cryptography command's filename switch for the file that contains, or will contain, the public key in Base64 format; optional. Path can be to a file in the current directory, relative to the current directory, or absolute.
  -P, --privatekeyfilename <P>  The cryptography command's filename switch for the file that contains, or will contain, the private key in Base64 format; optional. Path can be to a file in the current directory, relative to the current directory, or absolute.
  -e, --encrypt                 The cryptography command's switch to indicate that it will encrypt text indicated by the switch -t (--textinput), or encrypt text that is the contents of the file indicated by the switch -f (--fileinput), using the Base64 key 
                                indicated by the switch -k (--key), or using the Base64 key in the contents of the file indicated by the switch -a (--anykeyfilename), outputting the result to the console, or to the filename -o (--output); optional. Paths 
                                specified by -a (--anykeyfilename) or -o (--output) can be to files in the current directory, relative to the current directory, or absolute.
  -d, --decrypt                 The cryptography command's switch to indicate that it will decrypt text indicated by the switch -t (--textinput), or decrypt text that is the contents of the file indicated by the switch -f (--fileinput), using either the Base64 
                                public or private key indicated by the switch -k (--key), or using the Base64 key in the contents of the file indicated by the switch -a (--anykeyfilename), outputting the result to the console, or to the filename -o (--output); 
                                optional. Paths specified by -a (--anykeyfilename) or -o (--output) can be to files in the current directory, relative to the current directory, or absolute.
  -t, --textinput <t>           The cryptography command's switch to indicate text that will be encrypted in conjunction with the use of the switch -e (--encrypt); optional
  -f, --fileinput <f>           The cryptography command's switch to indicate text in the contents of a file that will be encrypted in conjunction with the use of the switch -e (--encrypt); optional. Path specified by -f (--fileinput) can be to a file in the 
                                current directory, relative to the current directory, or absolute.
  -g, --getpublickey            The cryptography command's switch to indicate that it will use an HttpClient to get the public key in Base64 from a DNS TXT Record indicated by the switch -n (--dnstxtkey), at a URL indicated by the switch -u (--url), or to get 
                                it from a public key server at a URL indicated by the switch -u (--url), and requiring a key id indicated by the switch -i (--id),  where a querystring appended to that url is in the format ?id=[id] and [id] is Base64Encoded; 
                                optional. When using -u (--url) in conjunction with -i (--id), do not append the querystring, the command appends this internally.
  -u, --url                     The cryptography command's switch to indicate the URL that will be used in conjunction with -g (getpublickey).
  -n, --dnstxtkey <n>           The cryptography command's switch to indicate the DNS TXT key that is used in conjunction with -g (--getpublickey) from a URL indicated by the switch -u (--url); optional. Cannot be used with -i (--id).
  -i, --id <i>                  NOT IMPLEMENTED. The cryptography command's switch to indicate the id that will be used in conjunction with the switch -g (--getpublickey); optional. Cannot be used with -n (--dnstxtkey).
  -y, --tokenheaderkey <y>      NOT IMPLEMENTED. The cryptography command's switch to indicate the header key, if required, of a -u (--url) that will be used in conjunction with the switch -i (--id); optional.
  -z, --tokenheadervalue <z>    NOT IMPLEMENTED. The cryptography command's switch to indicate the header value, if required, of a -u (--url) that will be used in conjunction with the switch -i (--id); optional.
  -o, --output <o>              The cryptography command's switch to indicate that output is to be placed as the contents of the file indicated; optional, if not specified, output will be written to console. Path can be to a file in the current directory, 
                                relative to the current directory, or absolute.
  -v, --verbose                 Instructs the CLI to make verbose CLI comments as it executes.
  -s, --silent                  Instructs the CLI to suppress all unnecessary comments
  -?, -h, --help                Show help and usage information
```

## Local

When cloning and building locally from the cloned root directory, the following CLI commands can be used to install, and uninstall:

- ```dotnet tool install --global --add-source ./nupkg Yatter.Net.Tools.CLI```
- ```dotnet tool uninstall -g Yatter.Net.Tools.CLI```

n.b. Prior to calling the above install line, you must have built the project and generated the Nuget package.

In Visual Studio, just select the ```Yatter.Net.Tools.CLI``` project, right-click, and select ```Create Nuget Package```.

The Nuget package is created in the ```./nupkg``` directory.
