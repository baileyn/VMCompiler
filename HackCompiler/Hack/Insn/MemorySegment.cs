using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack.Insn
{
    public static class MemorySegmentParser
    {
        private static Dictionary<string, MemorySegment> memorySegments = new Dictionary<string, MemorySegment>
        {
            { "static",  MemorySegment.Static },
            { "this", MemorySegment.This },
            { "local", MemorySegment.Local },
            { "argument", MemorySegment.Argument },
            { "that", MemorySegment.That },
            { "constant", MemorySegment.Constant },
            { "pointer", MemorySegment.Pointer },
            { "temp", MemorySegment.Temp },
        };

        private static Dictionary<MemorySegment, string> memoryLocations = new Dictionary<MemorySegment, string>
        {
            { MemorySegment.This, "THIS" },
            { MemorySegment.That, "THAT" },
            { MemorySegment.Argument, "ARG" },
            { MemorySegment.Local, "LCL" },
        };

        public static MemorySegment Parse(string memorySegment)
        {
            return memorySegments[memorySegment];
        }

        public static string GetMemoryLocation(MemorySegment memorySegment)
        {
            return memoryLocations[memorySegment];
        }
    }

    public enum MemorySegment
    {
        Static,
        This,
        Local,
        Argument,
        That,
        Constant,
        Pointer,
        Temp 
    }
}
