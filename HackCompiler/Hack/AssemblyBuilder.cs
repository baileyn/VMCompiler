using HackCompiler.Hack.Insn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{
    public class AssemblyBuilder
    {
        private List<Instruction> m_Instructions = new List<Instruction>();
        private string m_ClassName;

        public AssemblyBuilder(string className)
        {
            m_ClassName = className;
        }

        /// <summary>
        /// The <code>Instruction</code>s to be included in the <code>Assembly</code>
        /// </summary>
        public List<Instruction> Instructions
        {
            get
            {
                return m_Instructions;
            }
        }

        /// <summary>
        /// Append the specified <code>Instruction</code> into the generated <code>Assembly</code> object.
        /// </summary>
        /// <param name="instruction">the <code>Instruction</code> to append</param>
        /// <returns></returns>
        public AssemblyBuilder AppendInstruction(Instruction instruction)
        {
            m_Instructions.Add(instruction);

            return this;
        }

        /// <summary>
        /// Commit all of the proposed instructions into a final <code>Assembly</code>
        /// </summary>
        /// <returns>the <code>Assembly</code> containing all of the instructions</returns>
        public Assembly ToAssembly()
        {
            return new Assembly(m_ClassName, m_Instructions);
        }
    }
}
