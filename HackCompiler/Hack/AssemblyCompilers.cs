using HackCompiler.Hack.Insn;
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
        };

        #region Compilers
        public static Instruction PushCompiler(string className, TokenSequence sequence)
        {
            var tokens = sequence.Tokens;
            
            if (tokens.Count != 3)
            {
                throw new CompilationException(tokens[0], "Invalid number of arguments to " + Instruction.Push + " instruction. Expected 2, Got " + (tokens.Count - 1));
            }

            if (tokens[1].Type != TokenType.MemorySegment)
            {
                throw new CompilationException(tokens[1], "Expected a MemorySegment, but found a " + tokens[1].Type);
            }

            if (tokens[2].Type != TokenType.Number)
            {
                throw new CompilationException(tokens[2], "Expected an index number, but found a " + tokens[2].Type);
            }

            var memorySegment = MemorySegmentParser.Parse(tokens[1].Data);
            int index = int.Parse(tokens[2].Data);

            return new PushInstruction(className, memorySegment, index);
        }

        public static Instruction PopCompiler(string className, TokenSequence sequence)
        {
            var tokens = sequence.Tokens;

            if (tokens.Count != 3)
            {
                throw new CompilationException(tokens[0], "Invalid number of arguments to " + Instruction.Pop + " instruction. Expected 2, Got " + (tokens.Count - 1));
            }

            if (tokens[1].Type != TokenType.MemorySegment)
            {
                throw new CompilationException(tokens[1], "Expected a MemorySegment, but found a " + tokens[1].Type);
            }

            if (tokens[2].Type != TokenType.Number)
            {
                throw new CompilationException(tokens[2], "Expected an index number, but found a " + tokens[2].Type);
            }

            var memorySegment = MemorySegmentParser.Parse(tokens[1].Data);
            int index = int.Parse(tokens[2].Data);

            return new PopInstruction(className, memorySegment, index);
        }

        public static Instruction AddCompiler(string className, TokenSequence sequence)
        {
            return new AddInstruction();
        }

        public static Instruction SubCompiler(string className, TokenSequence sequence)
        {
            return new SubInstruction();
        }

        public static Instruction NegCompiler(string className, TokenSequence sequence)
        {
            return new NegInstruction();
        }

        public static Instruction EqCompiler(string className, TokenSequence sequence)
        {
            return new EqInstruction();
        }

        public static Instruction GtCompiler(string className, TokenSequence sequence)
        {
            return new GtInstruction();
        }

        public static Instruction LtCompiler(string className, TokenSequence sequence)
        {
            return new LtInstruction();
        }

        public static Instruction AndCompiler(string className, TokenSequence sequence)
        {
            return new AndInstruction();
        }

        public static Instruction OrCompiler(string className, TokenSequence sequence)
        {
            return new OrInstruction();
        }

        public static Instruction NotCompiler(string className, TokenSequence sequence)
        {
            return new NotInstruction();
        }
        #endregion
    }
}
