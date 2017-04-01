using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class GotoInstruction : Instruction
    {
        private const string GotoFormat = @"@{0}
0;JMP
";

        private string m_ClassName;
        private string m_LabelName;

        public GotoInstruction(TokenSequence sequence, string className, string labelName) :
            base(sequence)
        {
            m_ClassName = className;
            m_LabelName = labelName;
        }

        public override string GenerateAssembly()
        {
            var functionName = m_CurrentFunctionName;

            return String.Format(GotoFormat, 
                CreateLabel(functionName, m_LabelName));
        }
    }
}
