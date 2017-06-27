# TicTacToe

## Installation 
- install F# 4.0 - [Download Link](http://fsharp.org/use/windows/) Use Option 2 Instructions.

## Visual Studio Code Setup
- Install Visual Studio Code
- Add Ionide-fsharp, Ionide-FAKE, Ionide-Paket extensions to Visual Studio Code

## Windows - Setup
- clone the repository
- checkout develop branch
- download NUnit.Console-3.6.0.zip - [Download Link](https://github.com/nunit/nunit-console/releases/tag/3.6)
- extract Nunit console runner into /{ProjectRoot}/tools/nunit
- Open project folder Visual Studio Code

## Mac - Setup
- verify mono is installed - if not installed download and install from here [Download Link](http://www.mono-project.com/download/#download-mac)
- clone the repository
- checkout develop branch
- download NUnit.Console-3.6.0.zip - [Download Link](https://github.com/nunit/nunit-console/releases/tag/3.6)
- extract Nunit console runner into /{ProjectRoot}/tools/nunit
- Open project folder Visual Studio Code


## In Visual Studio Code
- ctrl-shift-P to enter command console

## Building
- command > run FAKE Build > choose Build

## Running Tests - Visual Studio Code
- command > run FAKE Build > choose NunitTest

## Deploying
- commmand run FAKE Build > choose Deploy (Note, running Nunit Tests deploys project as well.)

## Troubleshooting builds and testing for alternate Windows versions
- If msbuild target is missing, verify paths in project files point correctly to F# compiler. Version numbers might need to be adjusted. 
    - Compiler location likely stored at: C:\Program Files (x86)\Microsoft SDKs\F#\4.0\Framework\v4.0
    - Version numbers sometimes different for different Windows Versions or F# installations.
- if missing .net 452 reference assemblies install windows 8 .net sdk targeting pack - [Download Link](https://www.microsoft.com/en-us/download/confirmation.aspx?id=42637)
- if missing msbuild, install from here - [Download Link](https://www.microsoft.com/en-us/download/details.aspx?id=48159)