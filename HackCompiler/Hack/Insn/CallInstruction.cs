using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class CallInstruction : Instruction
    {
        private string CallFormat = @"{0}
{1}
{2}
{3}
{4}
D=M
@{5}
D=D-A
@5
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@{6}
0;JMP
({7})
";

        private string MemoryPushFormat = @"@{0}
D={1}
@SP
A=M
M=D
@SP
M=M+1
";

        private string m_ClassName;
        private string m_FunctionName;
        private int m_NumArgs;

        public CallInstruction(TokenSequence sequence, string className, string functionName, int numArgs) :
            base(sequence)
        {
            m_ClassName = className;
            m_FunctionName = functionName;
            m_NumArgs = numArgs;
        }

        public override string GenerateAssembly()
        {
            // Generate a return label for the function. The specification
            // doesn't actually state how this should be formatted, so
            // this should work.
            var returnLabel = CreateReturnLabel();
            var pushReturnAddress = GenerateMemoryPush(returnLabel, false);
            var pushLcl = GenerateMemoryPush("LCL");
            var pushArg = GenerateMemoryPush("ARG");
            var pushThis = GenerateMemoryPush("THIS");
            var pushThat = GenerateMemoryPush("THAT");

            return String.Format(CallFormat, 
                pushReturnAddress, pushLcl, pushArg, pushThis, pushThat,
                m_NumArgs, m_FunctionName, returnLabel);
        }

        private string CreateReturnLabel()
        {
            return CreateLabel("Return_" + m_FunctionName);
        }

        private string GenerateMemoryPush(string location, bool pointer=true)
        {
            var readLocation = "M";

            if(!pointer)
            {
                readLocation = "A";
            }

            return String.Format(MemoryPushFormat, location, readLocation);
        }

        public static Instruction GenerateCallInstruction(string function)
        {
            TokenSequence mockSequence = new TokenSequence();
            mockSequence.AddToken(new Token(TokenType.Instruction, 0, 0, "call"));
            mockSequence.AddToken(new Token(TokenType.Text, 0, 0, function));
            mockSequence.AddToken(new Token(TokenType.Number, 0, 0, "0"));

            return new CallInstruction(mockSequence, "Bootstrap", function, 0);
        }
    }
}
