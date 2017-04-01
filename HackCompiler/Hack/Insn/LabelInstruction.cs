using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class LabelInstruction : Instruction
    {
        private const string LabelFormat = "({0}${1})\n";

        private string m_ClassName;
        private string m_LabelName;

        public LabelInstruction(string className, string labelName)
        {
            m_ClassName = className;
            m_LabelName = labelName;
        }

        public override string GenerateAssembly()
        {
            return String.Format(LabelFormat, m_ClassName, m_LabelName);
        }
    }
}
