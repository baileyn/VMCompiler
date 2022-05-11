﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    // I know the static functions in this class look incredibly ugly, 
    // but it stops the whitespace. I apologize deeply, but it's the easiest
    // way to achieve my goal.
    public abstract class Instruction
    {
        public const string Push = "push";
        public const string Pop = "pop";
        public const string Add = "add";
        public const string Sub = "sub";
        public const string Neg = "neg";
        public const string Eq = "eq";
        public const string Gt = "gt";
        public const string Lt = "lt";
        public const string And = "and";
        public const string Or = "or";
        public const string Not = "not";
        public const string Label = "label";
        public const string IfGoto = "if-goto";
        public const string Goto = "goto";
        public const string Function = "function";
        public const string Return = "return";
        
        protected const int PointerLocationThis = 0;
        protected const int PointerLocationThat = 1;

        private static Dictionary<string, int> labelUses = new Dictionary<string, int>();

        /// <summary>
        /// Generates the default assembly to push the value in register <code>D</code> onto the stack.
        /// </summary>
        /// <returns>the generated assembly</returns>
        protected static string GeneratePushAssembly()
        {
            return @"@SP
A=M
M=D
@SP
M=M+1
";
        }

        protected static string GeneratePopAssembly()
        {
            return @"@SP
M=M-1
A=M
D=M
";
        }

        protected static string CreateLabel(string labelName)
        {
            int uses;

            if(!labelUses.TryGetValue(labelName, out uses))
            {
                uses = 0;
            }

            labelUses[labelName] = uses + 1;

            return String.Format("__{0}_{1}__", labelName, uses);
        }

        /// <summary>
        /// Generate the assembly for the desired instruction
        /// </summary>
        /// <returns>the generated assembly</returns>
        public abstract string GenerateAssembly();
    }
}