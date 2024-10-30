# Lexical Compiler

This repository contains a lexical compiler developed using C#. The compiler reads source code, analyzes it lexically, and identifies tokens.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Code Structure](#code-structure)
- [Authors](#authors)
- [License](#license)

## Overview
The lexical compiler reads source code from a file, processes it to identify various tokens such as keywords, operators, and identifiers, and displays the results in a user interface.

## Features
- **File Reading**: Opens and reads source code files.
- **Lexical Analysis**: Identifies and categorizes tokens in the source code.
- **Error Handling**: Detects and reports unrecognized characters.
- **User Interface**: Displays tokens and errors in a data grid.

## Prerequisites
- **.NET Framework**: Ensure you have .NET Framework installed.
- **Visual Studio**: For compiling and running the project.

## Installation
1. **Clone the repository**:
   ```sh
   git clone https://github.com/yourusername/lexical-compiler.git
   cd lexical-compiler
   ```

2. **Open the project in Visual Studio**:
   - Open `LexicoWF.sln` in Visual Studio.

3. **Build the project**:
   - Build the solution to restore dependencies and compile the project.

## Usage
1. **Run the application**:
   - Start the application from Visual Studio by pressing `F5` or selecting `Debug > Start Debugging`.

2. **Open a source code file**:
   - Use the `File > Open` menu to open a source code file for analysis.

3. **Analyze the source code**:
   - Click the `Analyze` button to perform lexical analysis on the opened file.

## Code Structure
- **`Form1.cs`**: Contains the main form and logic for file handling and lexical analysis.

## Authors
- Paulo Vinicius Martimiano de Oliveira
- Mateus Talzzia Diogo

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.
