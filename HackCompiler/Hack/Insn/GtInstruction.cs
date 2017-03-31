using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class GtInstruction : Instruction
    {
        private const string GtFormat = @"@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@{0}
D;JLE
@SP
A=M
M=-1
@{1}
0;JMP
({0})
@SP
A=M
M=0
({1})
@SP
M=M+1
";

        public override string GenerateAssembly()
        {
            var falseLabel = CreateLabel("gt_false");
            var endLabel = CreateLabel("gt_end");

            return String.Format(GtFormat, falseLabel, endLabel);
        }
    }
}
