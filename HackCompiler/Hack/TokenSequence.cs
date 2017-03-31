using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{
    public class TokenSequence
    {
        private List<Token> m_Tokens = new List<Token>();
        
        public bool Empty
        {
            get
            {
                return m_Tokens.Count == 0;
            }
        }

        public List<Token> Tokens
        {
            get
            {
                return m_Tokens;
            }
        }

        public void AddToken(Token token)
        {
            m_Tokens.Add(token);
        }
    }
}
