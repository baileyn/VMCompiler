using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class LtInstruction : Instruction
    {
        private const string LtFormat = @"@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@{0}
D;JGE
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
        public LtInstruction(TokenSequence sequence) :
            base(sequence)
        {

        }

        public override string GenerateAssembly()
        {
            var falseLabel = CreateLabel("lt_false");
            var endLabel = CreateLabel("lt_end");
            return String.Format(LtFormat, falseLabel, endLabel);
        }
    }
}
