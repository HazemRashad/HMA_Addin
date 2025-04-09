# HMA_Addin 🧱

 HMA Addin is a Revit plugin developed using the Revit API to streamline the management of structural columns. It enhances modeling workflows by automating adjustments, detecting clashes, and synchronizing parameter updates.

## Features
- Revit add-in (DLL) for native integration
- Automatic clash detection between structural columns and other elements
- Auto-adjustment logic to resolve positioning issues
- Real-time updating of instance parameters during model interaction

## Tech Stack
- C#
- Revit API (.NET Framework)

## Requirements
- Autodesk Revit 2021 or later
- .NET Framework 4.8

## Installation
1. Copy the `.addin` and `.dll` files to Revit's `Addins` folder.
2. Launch Revit and access the plugin from the Add-Ins tab.

## Future Enhancements
- Rule-based clash resolution
- Reporting/logging functionality
- UI for user-defined clash settings
