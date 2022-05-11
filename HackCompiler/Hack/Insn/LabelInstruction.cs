using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class LabelInstruction : Instruction
    {
        private const string LabelFormat = "({0})\n";

        private string m_ClassName;
        private string m_LabelName;

        public LabelInstruction(TokenSequence sequence, string className, string labelName) :
            base(sequence)
        {
            m_ClassName = className;
            m_LabelName = labelName;
        }

        public override string GenerateAssembly()
        {
            var functionName = m_CurrentFunctionName;

            var labelName = CreateLabel(functionName, m_LabelName);
            return String.Format(LabelFormat, labelName);
        }
    }
}
