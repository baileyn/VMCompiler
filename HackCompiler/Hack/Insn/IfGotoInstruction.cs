using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class IfGotoInstruction : Instruction
    {
        private const string GotoFormat = @"@{0}
D;JNE
";

        private string m_ClassName;
        private string m_LabelName;

        public IfGotoInstruction(TokenSequence sequence, string className, string labelName) :
            base(sequence)
        {
            m_ClassName = className;
            m_LabelName = labelName;
        }

        public override string GenerateAssembly()
        {
            var functionName = m_CurrentFunctionName;

            return GeneratePopAssembly() + 
                String.Format(GotoFormat, CreateLabel(functionName, m_LabelName));
        }
    }
}
