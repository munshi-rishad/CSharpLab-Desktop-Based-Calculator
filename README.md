# Desktop-Based Calculator

A desktop calculator made with **C# Windows Forms** for the C# lab.
It has a dark modern design, shows the full calculation above the answer and can also be used with the keyboard.

## Features

- Addition, subtraction, multiplication and division.
- Shows the calculation (for example `5 + 3 =`) above the answer.
- Chain calculations: `2 + 3 × 4 =` is worked out step by step.
- Decimal numbers, percent (`%`), plus/minus (`±`) and backspace (`←`).
- Clear error message when dividing by zero.
- Keyboard support: numbers, `+ - * /`, `.`, `%`, `Enter` (=), `Backspace` and `Esc` (clear).

## Technologies Used

- C# with Windows Forms
- .NET Framework 4.8
- Visual Studio

## Project Structure

```
CSharpLab-Desktop-Based-Calculator/
├── DesktopCalculator.sln              # open this in Visual Studio
├── DesktopCalculator/
│   ├── CalculatorForm.cs              # calculator logic (buttons, keyboard, math)
│   ├── CalculatorForm.Designer.cs     # design: colors, buttons and their positions
│   ├── CalculatorForm.resx
│   ├── Program.cs                     # starts the app
│   ├── App.config
│   ├── DesktopCalculator.csproj
│   └── Properties/
└── Screenshots/
```

## Requirements

- **Windows**
- **Visual Studio 2019 or 2022** (the free Community edition works) with the **.NET desktop development** workload – download from [visualstudio.microsoft.com](https://visualstudio.microsoft.com/)

## Clone the Project

```bash
git clone https://github.com/munshi-rishad/CSharpLab-Desktop-Based-Calculator.git
```

## Setup and Run

1. Install Visual Studio and select **.NET desktop development** during setup.
2. Open the cloned folder and double-click `DesktopCalculator.sln` (it opens in Visual Studio).
3. Press **F5** (or click **Start**) to build and run.
4. The calculator window opens in the middle of the screen.

> If Visual Studio asks for the **.NET Framework 4.8 Developer Pack**, click the install link it shows, then open the project again.

## Screenshots

### 1. Calculator

Dark theme with the calculation on top, a big answer, blue operators and a blue `=` button.

<p align="center">
  <img src="Screenshots/Calculator.png" alt="Calculator" width="320">
</p>
