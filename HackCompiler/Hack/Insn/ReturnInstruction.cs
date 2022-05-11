using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class ReturnInstruction : Instruction
    {
        private const string FrameOffsetFormat = @"@{1}
D=D-A
A=D
D=M
@{0}
M=D
";

        private const string ReturnFormat = @"@LCL
D=M
@frame
M=D
{0}
{1}
@ARG
A=M
M=D
@ARG
D=M+1
@SP
M=D
{2}
{3}
{4}
{5}
@retAddr
A=M
0;JMP
";

        private static string GenerateFrameOffset(string location, int offset, bool loadFrame = true)
        {
            StringBuilder builder = new StringBuilder();

            if(loadFrame)
            {
                builder.Append(@"@frame
D=M
");
            }

            builder.AppendFormat(FrameOffsetFormat, location, offset);

            return builder.ToString();
        }

        public override string GenerateAssembly()
        {
            return String.Format(ReturnFormat,
                GenerateFrameOffset("retAddr", 5, false),
                GeneratePopAssembly(),
                GenerateFrameOffset("THAT", 1),
                GenerateFrameOffset("THIS", 2),
                GenerateFrameOffset("ARG", 3),
                GenerateFrameOffset("LCL", 4)
                );
        }
    }
}
