using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMinusMinusCompiler
{
    // Code generator generates Intel 80x86 assembly language code
    // from a given symbol table and intermediate code file
    public class CodeGenerator
    {
        // Private properties
        private SymbolTable SymbolTable { get; set; }
        private List<string> SourceFileContents { get; set; }
        private int LastLocalOffset { get; set; } = -2;
        private int LastParameterOffset { get; set; } = 4;
        private FunctionNode CurrentFunction { get; set; }
        private int LogicalAndOperationCount { get; set; } = 0;
        private int LogicalOrOperationCount { get; set; } = 0;
        private int LogicalNotOperationCount { get; set; } = 0;
        private static string OutputFormat { get; } = "{0,-15} {1}";


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

        // Outputs assembly for start of program, writing data and starting code segments
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

        // Writes all global/base depth variables in data segment 
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

        // Writes all string literals to data segment
        private void ProcessStringLiterals()
        {
            for (int i = 0; i < SymbolTable.StringLiteralCount; i++)
            {
                StringLiteralNode node = (StringLiteralNode) SymbolTable.LookupNode($"_S{i}");
                OutputAssembly($"__S{i} DB {node.Literal}, \"$\"", 1);
            }
        }

        // Calls mandatory main C-- function in start procedure
        private void ProcessCallMainFunction()
        {
            OutputAssembly(new string[] { "__STARTPROC", "PROC" });
            OutputAssembly("MOV AX, @data", 2);
            OutputAssembly("MOV DS, AX", 2);
            OutputAssembly(string.Empty, 1);
            OutputAssembly("CALL _main", 2);
            OutputAssembly(string.Empty, 1);
            OutputAssembly("MOV AX, 4C00H", 2);
            OutputAssembly("INT 21H", 2);
            OutputAssembly(new string[] { "__STARTPROC", "ENDP" });
            OutputAssembly("END __STARTPROC", 1);
        }

        // Processes a function, writing necessary assembly generated from intermediate code
        private void ProcessFunction(List<string> functionCode)
        {
            CurrentFunction = (FunctionNode) SymbolTable.LookupNode(functionCode[0].Split(' ')[1]);

            if (CurrentFunction.LocalsSize % 2 != 0) CurrentFunction.LocalsSize += 1;
            if (CurrentFunction.ParametersSize % 2 != 0) CurrentFunction.ParametersSize += 1;

            functionCode.RemoveAt(0);
            ProcessFunctionBegin();
            foreach (string line in functionCode)
            {
                if (line.StartsWith("_PUSH")) ProcessPush(line.Split(' '));
                else if (line.StartsWith("_CALL")) ProcessCall(line.Split(' '));
                else if (line.StartsWith("_WR")) ProcessWrite(line.Split(' '));
                else if (line.StartsWith("_RD")) ProcessRead(line.Split(' '));
                else ProcessRegisterOperation(line.Split(' '));
                OutputAssembly(string.Empty, 1);
            }
            ProcessFunctionEnd();
        }

        // Writes assembly for push statement
        private void ProcessPush(string[] line)
        {
            OutputAssembly($"PUSH {ConvertNotation(line[1])}", 2);
        }

        // Writes assembly for function call statement
        private void ProcessCall(string[] line)
        {
            OutputAssembly($"CALL {ConvertNotation(line[1])}", 2);
        }

        // Runs appropriate console out statement
        private void ProcessWrite(string[] line)
        {
            if (line[0] == "_WRS") ProcessWriteStringLiteral(line[1]);
            else if (line[0] == "_WRC") ProcessWriteChar(line[1]);
            else if (line[0] == "_WRI") ProcessWriteInt(line[1]);
            else if (line[0] == "_WRL") ProcessWriteNewLine();
        }

        // Writes assembly for string output statement
        private void ProcessWriteStringLiteral(string output)
        {
            OutputAssembly($"MOV DX, OFFSET _{output}", 2);
            OutputAssembly("CALL writestr", 2);
        }

        // Writes assembly for character output statement
        private void ProcessWriteChar(string output)
        {
            OutputAssembly($"MOV DL, {ConvertNotation(output)}", 2);
            OutputAssembly("CALL writech", 2);
        }

        // Writes assembly for integer output statement
        private void ProcessWriteInt(string output)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(output)}", 2);
            OutputAssembly("CALL writeint", 2);
        }

        // Writes assembly for newline output statement
        private void ProcessWriteNewLine()
        {
            OutputAssembly("CALL writeln", 2);
        }

        // Calls appropriate read assembly function
        private void ProcessRead(string[] line)
        {
            if (line[0] == "_RDC") ProcessReadChar(line[1]);
            else if (line[0] == "_RDI") ProcessReadInt(line[1]);
        }

        // Writes assembly for read integer statement
        private void ProcessReadInt(string output)
        {
            OutputAssembly("CALL readint", 2);
            OutputAssembly($"MOV {ConvertNotation(output)}, BX", 2);
        }

        // Writes assembly for read character statement
        private void ProcessReadChar(string output)
        {
            OutputAssembly("CALL readch", 2);
            OutputAssembly($"MOV {ConvertNotation(output)}, AL", 2);
        }

        // Calls appropriate register processing function based on argument count and statement
        private void ProcessRegisterOperation(string[] statement)
        {
            if (statement.Count() == 3 && statement[2][0] == '!') ProcessNotAssignment(statement);
            else if (statement.Count() == 3 && statement[2][0] == '-') ProcessNegationAssignment(statement);
            else if (statement.Count() == 3) ProcessAssignmentOperation(statement);

            if (statement.Count() == 5 && statement[3] == "+") ProcessAdditionOperation(statement);
            else if (statement.Count() == 5 && statement[3] == "-") ProcessSubtractionOperation(statement);
            else if (statement.Count() == 5 && statement[3] == "*") ProcessMultiplicationOperation(statement);
            else if (statement.Count() == 5 && statement[3] == "/") ProcessDivisionOperation(statement);
            else if (statement.Count() == 5 && statement[3] == "%") ProcessModuloOperation(statement);
            else if (statement.Count() == 5 && statement[3] == "&&") ProcessLogicalAndOperation(statement);
            else if (statement.Count() == 5 && statement[3] == "||") ProcessLogicalOrOperation(statement);
        }

        // Writes assembly for a a not (!) operation assignment
        private void ProcessNotAssignment(string[] statement)
        {
            statement[2] = statement[2].Remove(0, 1);
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly("CMP AX, 0", 2);
            OutputAssembly($"JNE LOGICAL_NOT_{LogicalNotOperationCount}", 2);
            OutputAssembly("MOV AX, 1", 2);
            OutputAssembly($"JMP END_LOGICAL_NOT_{LogicalNotOperationCount}", 2);
            OutputAssembly($"LOGICAL_NOT_{LogicalNotOperationCount}:", 2);
            OutputAssembly("MOV AX, 0", 2);
            OutputAssembly($"END_LOGICAL_NOT_{LogicalNotOperationCount}:", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
            LogicalNotOperationCount++;
        }

        // Writes assembly for a negation assignment
        private void ProcessNegationAssignment(string[] statement)
        {
            statement[2] = statement[2].Remove(0, 1);
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly("NEG AX", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
        }

        // Writes assembly for an assignment operation
        private void ProcessAssignmentOperation(string[] statement)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
        }

        // Writes assembly for an addition operation
        private void ProcessAdditionOperation(string[] statement)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly($"MOV BX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("ADD AX, BX", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
        }

        // Writes assembly for a subtraction operation
        private void ProcessSubtractionOperation(string[] statement)
        {
            OutputAssembly($"MOV BX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly($"MOV CX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("SUB BX, CX", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, BX", 2);
        }

        // Writes assembly for a multiplication operation
        private void ProcessMultiplicationOperation(string[] statement)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly($"MOV BX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("MUL BX", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
        }

        // Writes assembly for a division operation
        private void ProcessDivisionOperation(string[] statement)
        {
            OutputAssembly("MOV DX, 0", 2);
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly($"MOV CX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("DIV CX", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
        }

        // Writes assembly for a modulo operation 
        private void ProcessModuloOperation(string[] statement)
        {
            OutputAssembly("MOV DX, 0", 2);
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly($"MOV CX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("DIV CX", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, DX", 2);
        }

        // Writes assembly for a logical or operation
        private void ProcessLogicalOrOperation(string[] statement)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly("CMP AX, 0", 2);
            OutputAssembly($"JNE LOGICAL_OR_{LogicalOrOperationCount}", 2);
            OutputAssembly($"MOV AX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("CMP AX, 0", 2);
            OutputAssembly($"JNE LOGICAL_OR_{LogicalOrOperationCount}", 2);
            OutputAssembly($"JMP END_LOGICAL_OR_{LogicalOrOperationCount}", 2);
            OutputAssembly($"LOGICAL_OR_{LogicalOrOperationCount}:", 2);
            OutputAssembly("MOV AX, 1", 2);
            OutputAssembly($"END_LOGICAL_OR_{LogicalOrOperationCount}:", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
            LogicalOrOperationCount++;
        }

        // Processes assembly for a logical and operation
        private void ProcessLogicalAndOperation(string[] statement)
        {
            OutputAssembly($"MOV AX, {ConvertNotation(statement[2])}", 2);
            OutputAssembly("CMP AX, 0", 2);
            OutputAssembly($"JE LOGICAL_AND_{LogicalAndOperationCount}", 2);
            OutputAssembly($"MOV AX, {ConvertNotation(statement[4])}", 2);
            OutputAssembly("CMP AX, 0", 2);
            OutputAssembly($"JE LOGICAL_AND_{LogicalAndOperationCount}", 2);
            OutputAssembly("MOV AX, 1", 2);
            OutputAssembly($"JMP END_LOGICAL_AND_{LogicalAndOperationCount}", 2);
            OutputAssembly($"LOGICAL_AND_{LogicalAndOperationCount}:", 2);
            OutputAssembly($"END_LOGICAL_AND_{LogicalAndOperationCount}:", 2);
            OutputAssembly($"MOV {ConvertNotation(statement[0])}, AX", 2);
            LogicalAndOperationCount++;
        }

        // Converts given argument to Intel 8086 assembly notation
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

        // Writes function start assembly
        private void ProcessFunctionBegin()
        {
            if (CurrentFunction.LocalsSize % 2 != 0) CurrentFunction.LocalsSize += 1;

            OutputAssembly(new string[] { $"_{CurrentFunction.Lexeme}", "PROC" });
            OutputAssembly("PUSH BP", 2);
            OutputAssembly("MOV BP, SP", 2);
            OutputAssembly($"SUB SP, {CurrentFunction.LocalsSize}", 2);
            OutputAssembly(string.Empty, 1);
        }

        // Writes function ending assembly
        private void ProcessFunctionEnd()
        {
            OutputAssembly($"ADD SP, {CurrentFunction.LocalsSize}", 2);
            OutputAssembly("POP BP", 2);
            OutputAssembly($"RET {CurrentFunction.ParametersSize}", 2);
            OutputAssembly(new string[] { $"_{CurrentFunction.Lexeme}", "ENDP" });
            OutputAssembly(string.Empty, 1);
            OutputAssembly(string.Empty, 1);
        }

        // Writes given assembly to screen at given column
        public void OutputAssembly(string output, int column)
        {
            string[] outputData = new string[] { string.Empty, string.Empty };
            outputData[column - 1] = output;
            OutputAssembly(outputData);
        }

        // Writes given array of outputs to screen at index columns
        public void OutputAssembly(string[] outputs)
        {
            CommonTools.WriteOutput(string.Format(OutputFormat, outputs));
        }
    }
}