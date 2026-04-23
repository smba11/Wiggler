# WIGGLER by SMBA

WIGGLER by SMBA is a lightweight Windows desktop utility that keeps a session active by moving the mouse only after the user has gone idle.

## What it does

- Arms when you turn it on.
- Waits for real user inactivity before taking control.
- Moves the mouse in visible bounded patterns.
- Stops instantly when you move the mouse yourself.
- Ignores its own injected mouse movement so it does not fight the user.
- Supports tray mode, persistent settings, and a replayable onboarding tutorial.

## Patterns

- Circle
- Square
- Triangle
- Figure 8
- Parallelogram
- Random

## Run from source

```powershell
dotnet run
```

## Build

```powershell
dotnet build
```

## Publish a shareable release

```powershell
dotnet publish -c Release
```

The published executable will be created under `bin\Release\net9.0-windows\win-x64\publish`.

## Notes

- Default idle delay is 12 seconds.
- Closing the window can either minimize to tray or exit, based on Settings.
- Launch mode can start in a normal window or directly in the tray.
