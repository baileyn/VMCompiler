using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public class PopInstruction : Instruction
    {
        private string m_ClassName;
        private MemorySegment m_MemorySegment;
        private int m_Index;

        private const string MemoryLocationFormatPrefix = @"@{1}
D=A
@{0}
A=M
D=D+A
@{0}
M=D
";

        private const string MemoryLocationFormatSuffix = @"@{0}
A=M
M=D
@{1}
D=A
@{0}
A=M
D=A-D
@{0}
M=D
";

        private List<MemorySegment> memoryLocationPops = new List<MemorySegment> { MemorySegment.This, MemorySegment.That, MemorySegment.Argument, MemorySegment.Local };

        public PopInstruction(TokenSequence sequence, string className, MemorySegment memorySegment, int index) :
            base(sequence)
        {
            m_ClassName = className;
            m_MemorySegment = memorySegment;
            m_Index = index;
        }

        public override string GenerateAssembly()
        {
            if(memoryLocationPops.Contains(m_MemorySegment))
            {
                var location = MemorySegmentParser.GetMemoryLocation(m_MemorySegment);
                return String.Format(MemoryLocationFormatPrefix, location, m_Index) 
                    + GeneratePopAssembly()
                    + String.Format(MemoryLocationFormatSuffix, location, m_Index);
            }
            else
            {
                return GeneratePopAssembly() + GenerateSuffix();
            }
        }

        private string GenerateSuffix()
        {
            string prefixFormat = @"@{0}
M=D
";

            var location = "";

            switch (m_MemorySegment)
            {
                case MemorySegment.Temp:
                    // Temporary storage starts at RAM location 5
                    location = (m_Index + 5).ToString();
                    break;
                case MemorySegment.Static:
                    location = String.Format("{0}.{1}", m_ClassName, m_Index);
                    break;
                case MemorySegment.Pointer:
                    switch (m_Index)
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
                default:
                    throw new ArgumentException("Invalid memory segment: " + m_MemorySegment);
            }

            return String.Format(prefixFormat, location);
        }
    }
}
