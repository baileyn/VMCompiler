using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class EqInstruction : Instruction
    {
        private const string EqFormat = @"@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@{0}
D;JNE
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

        public EqInstruction(TokenSequence sequence) :
            base(sequence)
        {

        }

        public override string GenerateAssembly()
        {
            var falseLabel = CreateLabel("eq_false");
            var endLabel = CreateLabel("eq_end");
            return String.Format(EqFormat, falseLabel, endLabel);
        }
    }
}
