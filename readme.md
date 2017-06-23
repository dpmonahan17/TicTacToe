# TicTacToe

## Installation 
- install F# 4.0 - [Download Link](http://fsharp.org/use/windows/) Use Option 2 Instructions.
- if missing msbuild, install from here - [Download Link](https://www.microsoft.com/en-us/download/details.aspx?id=48159)

## Windows - Visual Studio Code
- clone the repository
- checkout develop branch
- download Nunit console runner - [Download Link](https://github.com/nunit/nunit-console/releases/tag/3.6)
- extract Nunit console runner into /{ProjectRoot}/tool/nunit
- Open In Visual Studio Code

## Visual Studio Code Setup
- Add Ionide-fsharp, Ionide-FAKE, Ionide-Paket extensions to Visual Studio Code

## Building
- command > run FAKE Build > choose Build

## Running Tests - Visual Studio Code
- ctrl-shift-P to enter command console > run FAKE Build > choose NunitTest

## Deploying
- commmand run FAKE Build > choose Deploy (Note, running Nunit Tests deploys project as well.)

