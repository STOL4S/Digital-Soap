![LogoLarge](https://github.com/STOL4S/Digital-Soap/assets/138336394/3f48d5e1-4836-4264-80a7-c07389cc417a)
<h2 align="center">
  Lightweight .NET C# Open-Source Drive Cleaner and General Maintenance Tool for Windows
</h2>

<div align="center">
<img src="https://img.shields.io/badge/Visual%20Studio-2022-8A2BE2?logo=visualstudio"/>
<img src="https://img.shields.io/badge/.NET%208.0-C%23-239120"/>
<img src="https://img.shields.io/badge/License-GPLv3-blue.svg"/>
<img src="https://img.shields.io/github/v/tag/STOL4S/Digital-Soap?label=Release&color=4DC81F"/>
<img src="https://img.shields.io/github/downloads/STOL4S/Digital-Soap/total?label=Downloads"/>
<img src="https://img.shields.io/github/commit-activity/m/STOL4S/Digital-Soap?label=Commits"/>
<img src="https://img.shields.io/github/issues-raw/STOL4S/Digital-Soap?label=Open%20Issues"/>
<img src="https://img.shields.io/github/languages/code-size/STOL4S/Digital-Soap?label=Code%20Size"/>
<img src="https://img.shields.io/badge/GitHub-STOLAS-8A2BE2?logo=github"/>
</div>  

<br/>
Digital Soap is a .NET C# open-source tool designed for Windows systems. The goal of this program is to provide a free tool that can be used to easily free up space, manage programs and services, and benchmark or view system details.  

The program cleans the hard drive by wiping out old cache and temporary files and searching for unused or duplicate files found on the system and removing them to free up space. Installed applications on the computer may be recognized by Digital Soap and the cache for the program can be wiped, if chosen by the user. Custom folders can also be set to save locations on the computer that can be selected to wipe when the cleaner runs. The user has the option to select which system locations and programs are cleaned and can view scan information prior to deleting the data from the computer.

## Getting Started
*Prerequesite: .NET 8.0 Desktop Runtime is required to run this program.*<br/><br/>
First, download the latest [release](https://github.com/STOL4S/Digital-Soap/releases) of Digital Soap. Currently the program is only available in portable mode. Extract "DigitalSoap.exe" to the Desktop. All filesystem funcationality is handled through the AppData folder and will not create any files in the directory it is placed in. Run the application, there will be a prompt administrator privileges. The program then starts on the "Clean" tab, which will be discussed next.

## Digital Clean
Digital Clean is a library in the Digital Soap application that handles scanning the filesystem for cached, duplicated, temporary, and unused files. It handles the recognition of installed applications and storing custom folders to clean out when running the tool. This is the first tab when loading the application. In the left panel, there is a tree view that has options for which folders to clean. The right panel will display results for both scan and clean. Clicking scan checks how many and how large the files are in the selected folders and then present that information in the right panel. Clicking clean will delete all the files in the selected folders and then present that information in the right panel.<br/><br/>
*Note: Files can be cleaned without doing a scan first.*

## Functionality
By release version 1.0.0 this program should:
- Remove cache and temporary files from system.
- Find old, unused, or duplicate files taking up space.
- Modify/remove programs from the system.
- Modify which services are loaded on startup.
- Display for system disk defragmentation
- Retrieve detailed system specifications and have built-in benchmarks.
- Detect and troubleshoot possible hardware issues.
