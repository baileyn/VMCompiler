using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class GotoInstruction : Instruction
    {
        private const string GotoFormat = @"@{0}${1}
0;JMP
";

        private string m_ClassName;
        private string m_LabelName;

        public GotoInstruction(string className, string labelName)
        {
            m_ClassName = className;
            m_LabelName = labelName;
        }

        public override string GenerateAssembly()
        {
            return String.Format(GotoFormat, m_ClassName, m_LabelName);
        }
    }
}
