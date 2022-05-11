using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class FunctionInstruction : Instruction
    {
        private string AssemblyPushZero = @"@SP
A=M
M=0
@SP
M=M+1
";
        // Right now, m_ClassName isn't used, but I have
        // it incase in the future I need to use it for 
        // naming clashes.
        private string m_ClassName;
        private string m_FunctionName;
        private int m_NumArgs;

        public FunctionInstruction(string className, string functionName, int numArgs)
        {
            m_ClassName = className;
            m_FunctionName = functionName;
            m_NumArgs = numArgs;
        }

        public override string GenerateAssembly()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("({0})\n", m_FunctionName));

            // Reserve space for the arguments.
            for(int i = 0; i < m_NumArgs; i++)
            {
                builder.Append(AssemblyPushZero);
            }

            return builder.ToString();
        }
    }
}
