# Illumina Helper

A Notepad++ plugin (written in C#) for working with the log and configuration files
produced by Illumina sequencing instruments. It reads the open document in Notepad++
and adds two dockable panels: one that summarises a run's `RunParameters.xml`, and one
that finds error lines in a log file.

## Features

### Parse runparameters
Parses an Illumina `RunParameters.xml` file that is open in the active Notepad++ tab
and displays a clean, human-readable summary in a dockable panel:

- **Header** — platform, run ID, experiment name, and run start date.
- **Consumables** — serial number, lot number, part number, and expiry date for each
  consumable (flow cell, reagent kit, buffer, cluster, SBS, PR2, cartridge, etc.).
  Expiry dates are colour-coded: **green** if valid at the run start date, **red**
  (marked `expired`) if the consumable had already expired.
- **Additional info** — control software name/version and custom primer usage.
- A **Copy** button copies the parsed summary to the clipboard.

The panel refreshes automatically when you switch between open tabs, so you can move
from one run file to another without re-running the command.

Supported platforms (including their `Dx` variants):

- MiSeq
- iSeq
- MiniSeq
- NextSeq 500/550
- NextSeq 1000/2000
- NovaSeq

Each platform stores its parameters under different XML paths; the plugin maps the
correct XPath for each instrument automatically after detecting the platform from the
`Application` / `ApplicationName` node.

### Find errors
Scans the active document for lines matching a configurable set of regular expressions
and lists every match in a dockable panel. Clicking a result jumps the editor straight
to that line. The search runs on a background worker so large log files stay responsive,
and the results refresh automatically when you switch tabs.

Default search patterns: `FATAL`, `\sERR\s`, `\sERROR`, `\sError`, `\serror`.

### Settings
A settings dialog lets you choose:

- Which header, consumable, and additional-info fields appear in the runparameters summary.
- Which error expressions are active, with the ability to add or remove custom
  regular expressions.

Settings are persisted to an `.ini` file in the Notepad++ plugin config directory and
reloaded on startup.

## Usage

Once installed, the plugin's commands are available under
**Plugins → Illumina Helper**:

| Command | Description |
| --- | --- |
| Parse runparameters | Open the run summary panel for the active `RunParameters.xml`. |
| Find errors | Open the error-search panel for the active document. |
| Settings | Configure displayed fields and error expressions. |
| About | Version and author information. |

A toolbar icon is also registered for quick access.

## Building

This is a .NET Framework class library built as a Notepad++ plugin.

- **Target framework:** .NET Framework 4.7.1
- **Platforms:** x86 / x64 (Notepad++ must match the built architecture)
- **Dependencies:** the plugin uses the standard
  [NppPlugin.NET](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net)
  infrastructure (included under `PluginInfrastructure/`).

Open `illumina_helper.sln` in Visual Studio and build the `Illumina_helper` project.
The build produces `Illumina_helper.dll`. A post-build step copies the DLL to the local
Notepad++ plugins folder:

```
C:\Program Files (x86)\Notepad++\plugins\Illumina_helper\
```

## Installation

Copy `Illumina_helper.dll` into its own folder inside the Notepad++ `plugins`
directory, so the layout is:

```
<Notepad++>\plugins\Illumina_helper\Illumina_helper.dll
```

Then restart Notepad++. Make sure the plugin architecture (x86/x64) matches your
Notepad++ installation.

## Author

Jaren Junren Sia — 2021
