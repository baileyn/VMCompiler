﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class NegInstruction : Instruction
    {
        public NegInstruction(TokenSequence sequence) :
            base(sequence)
        {

        }

        public override string GenerateAssembly()
        {
            return @"@SP
M=M-1
A=M
M=-M
@SP
M=M+1
";
        }
    }
}
