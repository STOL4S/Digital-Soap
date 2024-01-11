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
First, download the latest [release](https://github.com/STOL4S/Digital-Soap/releases) of Digital Soap. Currently the program is only available in portable mode and for Windows operating systems. Extract "DigitalSoap.exe" to the Desktop. All filesystem funcationality is handled through the AppData folder and will not create any files in the directory it is placed in. Run the application, there will be a prompt administrator privileges. The program then starts on the "Clean" tab, which will be discussed next.

## Digital Clean
Digital Clean is a library in the Digital Soap application that handles scanning the filesystem for cached and temporary files. It handles the recognition of installed applications and storing custom folders to clean out when running the tool. This is the first tab when loading the application. In the left panel, there is a tree view that has options for which folders to clean. The right panel will display results for both scan and clean. Clicking scan checks how many and how large the files are in the selected folders and then present that information in the right panel. Clicking clean will delete all the files in the selected folders and then present that information in the right panel.<br/><br/>
After running the cleaner, it is recommended to run the scan again to ensure files have been deleted. If there is any issues please refer to the troubleshooting section of this page.<br/><br/>
*Note: Files can be cleaned without doing a scan first.*

## Digital Storage
Digital Storage is a library written for the Digital Soap application that handles scanning the filesystem for duplicate or unused files. It scans an entire drive or selected directory for any files that are duplicate or considered old. On the left panel, there are settings that control where to scan, what files are considered old, and parameters that define "duplicate" files. By default, the scanner considers any file that has not been touched for six months as old and any file with the same exact name and data as duplicates. Information for the found files and directories will be displayed on the right side panel. The user can then manually choose which files and folders to delete or keep.  

## Digital Programs
Digital Programs is the third library in this program and is responsible for handling modifying and removing programs from the system and which programs and services run on startup. The library scans the system for all installed programs and their uninstaller files in a similar way that Windows "Add/Remove Programs" tool would work. Digital Programs has a chance of finding other programs Windows may not recognize by automatically searching the entire filesystem for uninstallers. All recognized programs and services can be modified, removed, or stopped. Programs and services that can be disabled during startup will be listed and can significantly improve startup time by disabling unnecessary ones.  

## Digital Performance
Digital Performance is the hardware monitoring library for the Digital Soap application. This library is capable of retrieving real time information from the system about the hardware and how it is performing. Initially the user is presented with a system monitor similar to the Windows Task Manager, but has additional monitoring options and can pop out of the window to create a monitor display that can be placed anywhere on the user's screen. The performance module also contains a set of benchmarking tools for CPU, GPU, RAM, and storage devices. The entire system can be benchmarked or just a specific piece of hardware. The benchmark for the device can also be selected and the intensity of the benchmarking. The results generated by this benchmark can be exported in many ways including text, image, or markup languages.

## Digital System
Digital System is the final library associated with this program. This library handles reading information from the system about the operating system and what hardware the system is using. This is useful for reading exact specifications reported by your device. The tool also features an easy way to compile and share system specs and multiple ways to export this information including support for markup languages.

## Functionality
By release version 1.0.0 this program should:
- Remove cache and temporary files from system.
- Find old, unused, or duplicate files taking up space.
- Modify/remove programs from the system.
- Modify which services are loaded on startup.
- Display for system disk defragmentation
- Retrieve detailed system specifications and have built-in benchmarks.
- Detect and troubleshoot possible hardware issues.
