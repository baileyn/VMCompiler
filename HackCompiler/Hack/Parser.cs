using HackCompiler.Hack.Insn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{
    public class Parser
    {
        private Lexer m_Lexer;

        public Parser(Lexer lexer)
        {
            m_Lexer = lexer;
            m_Lexer.Parse();
        }

        /// <summary>
        /// Parse all of the <code>Token</code>s that are tokenized by the <code>Lexer</code> into sequences of <code>Token</code>s,
        /// called <code>TokenSequence</code>s. Each <code>TokenSequence</code> is considered a complete set of <code>Token</code>s
        /// to complete an <code>Instruction</code>
        /// </summary>
        /// <returns>the parsed list of <code>TokenSequence</code>s</returns>
        private List<TokenSequence> ParseTokenSequences()
        {
            var tokenSequences = new List<TokenSequence>();
            var currentSequence = new TokenSequence();

            foreach (var token in m_Lexer.Tokens)
            {
                if(token.Type == TokenType.NewLine)
                {
                    if(currentSequence.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        tokenSequences.Add(currentSequence);
                        currentSequence = new TokenSequence();
                        continue;
                    }
                }

                currentSequence.AddToken(token);
            }

            if(currentSequence.Tokens.Count > 0)
            {
                tokenSequences.Add(currentSequence);
            }

            return tokenSequences;
        }

        /// <summary>
        /// <see cref="ParseTokenSequences"/>
        /// </summary>
        public List<TokenSequence> Parse()
        {
            // TODO: I think technically the <code>Praser</code> is supposed to return an <code>Abstract Syntax Tree</code>, but 
            //       for now, these <code>TokenSequence<code>s will work swimmingly.
            return ParseTokenSequences();
        }
    }
}
