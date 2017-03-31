using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{

    public class PushInstruction : Instruction
    {
        private const string MemoryLocationFormat = @"@{0}
D=A
@{1}
A=M
D=D+A
A=D
D=M
";

        private MemorySegment m_MemorySegment;
        private int m_Index = -1;

        private string m_ClassName;

        public PushInstruction(string className, MemorySegment memorySegment, int index)
        {
            m_ClassName = className;
            m_MemorySegment = memorySegment;
            m_Index = index;
        }

        public override string GenerateAssembly()
        {
            return GeneratePrefix() + Instruction.GeneratePushAssembly();
        }

        private string GeneratePrefix()
        {
            string prefixFormat = @"@{0}
D={1}
";

            var location = "";
            var register = "M";

            switch(m_MemorySegment)
            {
                case MemorySegment.Constant:
                    location = m_Index.ToString();
                    register = "A";
                    break;
                case MemorySegment.Temp:
                    // Temporary storage starts at RAM location 5
                    location = (m_Index + 5).ToString();
                    break;
                case MemorySegment.Static:
                    location = String.Format("{0}.{1}", m_ClassName, m_Index);
                    break;
                case MemorySegment.Pointer:
                    switch(m_Index)
                    {
                        case PointerLocationThis:
                            location = "THIS";
                            break;
                        case PointerLocationThat:
                            location = "THAT";
                            break;
                        default:
                            throw new ArgumentException("Invalid pointer location");
                    }
                    break;
                default: // Try to push to a memory location.
                    var memoryLocation = MemorySegmentParser.GetMemoryLocation(m_MemorySegment);

                    // This is incredibly filthy. I'm sorry. It made it easier. Please don't lynch me.
                    prefixFormat = MemoryLocationFormat;
                    location = m_Index.ToString();
                    register = memoryLocation;
                    break;
            }

            return String.Format(prefixFormat, location, register);
        }
    }
}
