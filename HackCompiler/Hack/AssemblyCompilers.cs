﻿using HackCompiler.Hack.Insn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{
    public static class AssemblyCompilers
    {
        public delegate Instruction AssemblyCompiler(string className, TokenSequence sequence);

        public static Dictionary<string, AssemblyCompiler> Compilers = new Dictionary<string, AssemblyCompiler>
        {
            { Instruction.Push, PushCompiler },
            { Instruction.Pop, PopCompiler },
            { Instruction.Add, AddCompiler },
            { Instruction.Sub, SubCompiler },
            { Instruction.Neg, NegCompiler },
            { Instruction.Eq, EqCompiler },
            { Instruction.Gt, GtCompiler },
            { Instruction.Lt, LtCompiler },
            { Instruction.And, AndCompiler },
            { Instruction.Or, OrCompiler },
            { Instruction.Not, NotCompiler },
            { Instruction.Label, LabelCompiler },
            { Instruction.Goto, GotoCompiler },
            { Instruction.IfGoto, IfGotoCompiler },
        };

        private static void VerifySequence(TokenSequence sequence, params TokenType[] types)
        {
            if(sequence.Tokens.Count - 1 != types.Length)
            {
                throw new CompilationException(sequence.Tokens[0],
                    "Invalid number of arguments to " + sequence.Tokens[0].Data + " instruction.\nExpected " +
                    types.Length.ToString() + ", but got " + (sequence.Tokens.Count - 1));
            }

            // The first token in the TokenSequence is actually the instruction itself,
            // so it's actually skipped when checking the Tokens used as arguements.
            for(var i = 1; i < sequence.Tokens.Count; i++)
            {
                var token = sequence.Tokens[i];

                if(token.Type != types[i - 1])
                {
                    throw new CompilationException(token,
                        "Expected a " + types[i] + ", but found a " + token.Type);
                }
            }
        }

        #region Compilers
        public static Instruction PushCompiler(string className, TokenSequence sequence)
        {
            var tokens = sequence.Tokens;
            VerifySequence(sequence, TokenType.MemorySegment, TokenType.Number);

            var memorySegment = MemorySegmentParser.Parse(tokens[1].Data);
            int index = int.Parse(tokens[2].Data);

            return new PushInstruction(className, memorySegment, index);
        }

        public static Instruction PopCompiler(string className, TokenSequence sequence)
        {
            var tokens = sequence.Tokens;
            VerifySequence(sequence, TokenType.MemorySegment, TokenType.Number);


            var memorySegment = MemorySegmentParser.Parse(tokens[1].Data);
            int index = int.Parse(tokens[2].Data);

            return new PopInstruction(className, memorySegment, index);
        }

        public static Instruction AddCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new AddInstruction();
        }

        public static Instruction SubCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new SubInstruction();
        }

        public static Instruction NegCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new NegInstruction();
        }

        public static Instruction EqCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new EqInstruction();
        }

        public static Instruction GtCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new GtInstruction();
        }

        public static Instruction LtCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new LtInstruction();
        }

        public static Instruction AndCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new AndInstruction();
        }

        public static Instruction OrCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new OrInstruction();
        }

        public static Instruction NotCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence);
            return new NotInstruction();
        }

        public static Instruction LabelCompiler(string className, TokenSequence sequence)
        {

            VerifySequence(sequence, TokenType.Text);

            var labelName = sequence.Tokens[1].Data;

            return new LabelInstruction(className, labelName);
        }

        public static Instruction GotoCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence, TokenType.Text);

            var labelName = sequence.Tokens[1].Data;
            return new GotoInstruction(className, labelName);
        }

        public static Instruction IfGotoCompiler(string className, TokenSequence sequence)
        {
            VerifySequence(sequence, TokenType.Text);

            var labelName = sequence.Tokens[1].Data;
            return new IfGotoInstruction(className, labelName);
        }
        #endregion
    }
}
