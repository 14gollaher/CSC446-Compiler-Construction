using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMinusMinusCompiler
{
    public class CodeGenerator
    {
        // Private properties
        private SymbolTable SymbolTable { get; set; }
        private List<string> SourceFileContents { get; set; }
        private static string OutputFormat { get; } = "{0,-15} {1}";
        public int LastLocalOffset { get; set; } = -2;
        public int LastParameterOffset { get; set; } = 4;
        public FunctionNode CurrentFunction { get; set; }


        // Constructor that accepts a symbol table and intermediate code file path
        public CodeGenerator(string intermediateCodeFilePath, SymbolTable symbolTable)
        {
            SourceFileContents = File.ReadAllLines(intermediateCodeFilePath).
                Select(line => line.Trim()).
                Where(line => line != string.Empty).
                ToList();
            SymbolTable = symbolTable;
        }

        // Starts the Intel 8086 assembly language code generation
        public void Start()
        {
            ProcessProgramBegin();

            List<string> currentProcedureCode = new List<string>();
            foreach (string line in SourceFileContents)
            {
                if (!line.StartsWith("ENDP"))
                {
                    currentProcedureCode.Add(line);
                }
                else
                {
                    ProcessFunction(currentProcedureCode);
                    currentProcedureCode = new List<string>();
                }
            }
            ProcessCallMainFunction();
        }

        private void ProcessProgramBegin()
        {
            OutputAssembly(".MODEL SMALL", 1);
            OutputAssembly(".STACK 100H", 1);
            OutputAssembly(string.Empty, 1);
            OutputAssembly(".DATA", 1);

            ProcessBaseDepthVariables();
            ProcessStringLiterals();
            OutputAssembly(string.Empty, 1);

            OutputAssembly(".CODE", 1);
            OutputAssembly("INCLUDE io.asm", 1);
        }

        private void ProcessBaseDepthVariables()
        {
            List<Node> nodes = SymbolTable.GetSymbolTableDepth(GlobalConfiguration.BaseDepth);
            foreach (Node node in nodes)
            {
                if (node is VariableNode)
                {
                    if (((VariableNode)node).Type == Token.IntToken) OutputAssembly($"_{node.Lexeme} DW ?", 1);
                    else if (((VariableNode)node).Type == Token.CharToken) OutputAssembly($"_{node.Lexeme} DB ?", 1);
                    // TODO: Add float here
                }
            }
        }

        private void ProcessStringLiterals()
        {
            for (int i = 0; i < SymbolTable.StringLiteralCount; i++)
            {
                StringLiteralNode node = (StringLiteralNode) SymbolTable.LookupNode($"_S{i}");
                OutputAssembly($"__S{i} DB {node.Literal}, \"$\"", 1);
            }
        }

        private void ProcessCallMainFunction()
        {
            OutputAssembly(new string[] { "__STARTPROC", "PROC" });
            OutputAssembly("MOV AX, @data", 2);
            OutputAssembly("MOV DS, AX", 2);
            OutputAssembly("CALL _main", 2);
            OutputAssembly("MOV AX, 4C00H", 2);
            OutputAssembly("INT 21H", 2);
            OutputAssembly(new string[] { "__STARTPROC", "ENDP" });
            OutputAssembly("END __STARTPROC", 1);
        }

        private void ProcessFunction(List<string> functionCode)
        {
            CurrentFunction = (FunctionNode) SymbolTable.LookupNode(functionCode[0].Split(' ')[1]);
            functionCode.RemoveAt(0);

            ProcessFunctionBegin();
            foreach (string line in functionCode)
            {
                if (line.StartsWith("_PUSH")) ProcessPush(line.Split(' '));
                else if (line.StartsWith("_CALL")) ProcessCall(line.Split(' '));
                else if (line.StartsWith("_WRITE")) ProcessWrite(line.Split(' '));
                else if (line.StartsWith("_READ")) ProcessRead(line.Split(' '));
                else ProcessRegisterOperation(line.Split(' '));
            }
            ProcessFunctionEnd();
        }

        private void ProcessPush(string[] line)
        {
            OutputAssembly($"PUSH {ConvertNotation(line[1])}", 2);
        }

        private void ProcessCall(string[] line)
        {
            OutputAssembly($"CALL {ConvertNotation(line[1])}", 2);
        }

        private void ProcessWrite(string[] line)
        {
            if (line[0] == "_WRITESTRINGLITERAL") ProcessWriteStringLiteral(line[1]);
            else if (line[0] == "_WRITECHAR") ProcessWriteChar(line[1]);
            else if (line[0] == "_WRITEINT") ProcessWriteInt(line[1]);
            else if (line[0] == "_WRITENEWLINE") ProcessWriteNewLine();
        }

        private void ProcessWriteStringLiteral(string output)
        {
            OutputAssembly($"MOV DX, OFFSET _{output}", 2);
            OutputAssembly("CALL writestr", 2);
        }

        private void ProcessWriteChar(string output)
        {
            OutputAssembly($"MOV DL, {ConvertNotation(output)}", 2);
            OutputAssembly("CALL writech", 2);
        }

        private void ProcessWriteInt(string output)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(output)}", 2);
            OutputAssembly("CALL writeint", 2);
        }

        private void ProcessWriteNewLine()
        {
            OutputAssembly("CALL writeln", 2);
        }

        private void ProcessRead(string[] line)
        {
            if (line[0] == "_READCHAR") ProcessReadChar(line[1]);
            else if (line[0] == "_READINT") ProcessReadInt(line[1]);
        }

        private void ProcessReadInt(string output)
        {
            OutputAssembly("CALL readint", 2);
            OutputAssembly($"MOV {ConvertNotation(output)}, BX", 2);
        }

        private void ProcessReadChar(string output)
        {
            OutputAssembly("CALL readch", 2);
            OutputAssembly($"MOV {ConvertNotation(output)}, AL", 2);
        }

        private void ProcessRegisterOperation(string[] line)
        {
            if (line.Count() == 3) ProcessAssignmentOperation(line);
            if (line.Count() == 5 && line[3] == "+") ProcessAdditionOperation(line);
        }

        private void ProcessAssignmentOperation(string[] line)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(line[2])}", 2);
            OutputAssembly($"MOV {ConvertNotation(line[0])}, AX", 2);
        }
        private void ProcessAdditionOperation(string[] line)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(line[2])}", 2);
            OutputAssembly($"MOV BX, {ConvertNotation(line[4])}", 2);
            OutputAssembly("ADD AX, BX", 2);
            OutputAssembly($"MOV {ConvertNotation(line[0])}, AX", 2);
        }

        private string ConvertNotation(string output)
        {
            if (output.StartsWith("_BP"))
            {
                int offset = Convert.ToInt32(output.Remove(0, 3));
                if (offset < 0) LastLocalOffset = offset;
                else LastParameterOffset = offset;  

                return $"[BP{offset.ToString("+0;-#")}]";
            }
            else if (Char.IsLetter(output[0]))
            {
                return $"_{output}";
            }
            else if (output == "_AX")
            {
                return "AX";
            }

            return output;
        }

        private void ProcessFunctionBegin()
        {
            OutputAssembly(new string[] { $"_{CurrentFunction.Lexeme}", "PROC" });
            OutputAssembly("PUSH BP", 2);
            OutputAssembly("MOV BP, SP", 2);
            OutputAssembly($"SUB SP, {CurrentFunction.LocalsSize}", 2);
        }

        private void ProcessFunctionEnd()
        {
            OutputAssembly($"ADD SP, {CurrentFunction.LocalsSize}", 2);
            OutputAssembly("POP BP", 2);
            OutputAssembly($"RET {CurrentFunction.ParametersSize}", 2);
            OutputAssembly(new string[] { $"_{CurrentFunction.Lexeme}", "ENDP" });
            OutputAssembly(string.Empty, 1);
            OutputAssembly(string.Empty, 1);
        }

        public void OutputAssembly(string output, int column)
        {
            string[] outputData = new string[] { string.Empty, string.Empty };
            outputData[column - 1] = output;
            OutputAssembly(outputData);
        }

        public void OutputAssembly(string[] outputs)
        {
            CommonTools.WriteOutput(string.Format(OutputFormat, outputs));
        }
    }
}