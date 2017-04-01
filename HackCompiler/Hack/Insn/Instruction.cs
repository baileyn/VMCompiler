using System;
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
        public const string Call = "call";

        protected const int PointerLocationThis = 0;
        protected const int PointerLocationThat = 1;

        private static Dictionary<string, int> LabelUses = new Dictionary<string, int>();

        protected static string m_CurrentFunctionName;

        public TokenSequence TokenSequence
        {
            get;
            set;
        }

        public string Raw
        {
            get
            {
                if(TokenSequence == null)
                {
                    return "<UNRECOVERABLE>";
                }
                else
                {
                    StringBuilder builder = new StringBuilder();

                    builder.Append(TokenSequence.Tokens[0].Data);
                    foreach(var token in TokenSequence.Tokens.Skip(1))
                    {
                        builder.AppendFormat(" {0}", token.Data);
                    }

                    return builder.ToString();
                }
            }
        }

        protected Instruction(TokenSequence sequence)
        {
            TokenSequence = sequence;
        }

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

        protected static string CreateLabel(string functionName, string labelName)
        {
            return String.Format("{0}${1}", functionName, labelName);
        }

        protected static string CreateLabel(string labelName)
        {
            int uses;

            if(!LabelUses.TryGetValue(labelName, out uses))
            {
                uses = 0;
            }

            LabelUses[labelName] = uses + 1;

            return String.Format("__{0}_{1}__", labelName, uses);
        }

        /// <summary>
        /// Generate the assembly for the desired instruction
        /// </summary>
        /// <returns>the generated assembly</returns>
        public abstract string GenerateAssembly();
    }
}
