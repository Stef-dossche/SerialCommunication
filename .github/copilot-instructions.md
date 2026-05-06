# Copilot instructions for SerialCommunication

Purpose
- Provide repository-specific guidance for Copilot sessions: build/run facts, architecture, and project conventions.

Build, run, test, lint
- Open solution in Visual Studio 2017/2019/2022 and build normally.
- CLI build (MSBuild):
  - msbuild .\SerialCommunication.slnx /p:Configuration=Debug
  - or msbuild .\SerialCommunication\SerialCommunication.csproj /p:Configuration=Debug
- Resulting binary: SerialCommunication\bin\Debug\SerialCommunication.exe
- Tests: This repository contains no test projects. (No test runner configured.)
- Linting: No C# lint/StyleCop/EditorConfig rules included in repo.

High-level architecture
- Desktop app: Windows Forms (.NET Framework 4.7.2) located in SerialCommunication/.
  - Entry: Program.cs; UI and serial logic: Form1.cs.
  - Uses System.IO.Ports to enumerate ports and communicate with the device.
  - UI stores resources under SerialCommunication\Resources and settings under Properties/.
- Embedded device: Arduino sketch and helpers in arduino/SerialCommunication/.
  - Implements a text-based command protocol (SerialCommand library) at 115200 baud.
  - Commands: set, toggle, get, ping, help, debug. Examples: "set d3 on", "get a0". Responses are plain text (e.g., "set done", "a0: 123").
  - Pin ranges and meanings are enforced in firmware (e.g., digital pins 2..4 for outputs, pwm 9..11, analog 0..5 for inputs).
- Integration: The WinForms app sends human-readable ASCII commands matching the sketch's expected tokens and parses textual responses.

Key conventions
- Arduino defines the canonical command names and response strings; the desktop UI must match them exactly.
- Baudrate default is 115200; changing it requires sync on both PC and Arduino sides.
- Resource and settings files are managed via the Visual Studio project (Properties/Resources.resx; Settings.settings).
- The project targets .NET Framework via MSBuild/Visual Studio; dotnet CLI (dotnet build) is not used for this project type.

Other AI assistant configs
- No CLAUDE.md, AGENTS.md, .cursorrules, .windsurfrules, CONVENTIONS.md, or similar assistant config files detected at repo root.

Notes for Copilot sessions
- Prefer making minimal, surgical edits to Form1.cs when adjusting serial protocol handling.
- When modifying Arduino command names or formats, update both arduino/SerialCommunication/*.ino and any UI code that emits those tokens.

