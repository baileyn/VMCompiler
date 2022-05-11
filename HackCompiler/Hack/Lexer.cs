using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{
    public enum State
    {
        Base,
        LineComment,
        Text,
        Number,
    }

    public enum TokenType
    {
        Instruction,
        MemorySegment,
        Number,
        LeftParen,
        RightParen,
        Text,
        NewLine
    }

    public class Token
    {
        public TokenType Type
        {
            get;
            private set;
        }

        public int LineNumber
        {
            get;
            private set;
        }

        public int CharPosition
        {
            get;
            private set;
        }

        public string Data
        {
            get;
            private set;
        }

        public Token(TokenType type, int lineNumber, int charPosition, string data)
        {
            Type = type;
            LineNumber = lineNumber;
            CharPosition = charPosition;
            Data = data;
        }
    }



    public class Lexer
    {
        private List<Token> m_Tokens = new List<Token>();

        /// <summary>
        /// the entire contents of the Virtual Machine File that is to be lexically tokenized
        /// </summary>
        private string m_Line;

        private int m_LineIndex = 0;

        /// <summary>
        /// the line position in the m_Lines array
        /// </summary>
        private int m_LineNumber = 1;
        
        /// <summary>
        /// the character position in the line pointed to by the <code>LineIndex</code>
        /// </summary>
        private int m_CharPosition = 1;

        /// <summary>
        /// the <code>StringBuilder</code> used to build the <code>Token</code>'s data
        /// </summary>
        private StringBuilder m_Data = new StringBuilder();

        /// <summary>
        /// the current <code>State</code> of the <code>Lexer</code>
        /// </summary>
        private State m_CurrentState = State.Base;

        public IEnumerable<Token> Tokens
        {
            get
            {
                return m_Tokens.AsReadOnly();
            }
        }

        private List<char> validCharacters = new List<char> { '_', '-' };

        private List<string> instructions = new List<string> {
            "push", "pop", "add", "sub", "neg", "eq", "gt", "lt", "and",
            "or", "not", "label", "if-goto", "goto", "function", "return"
        };

        private List<string> memorySegment = new List<string> {
            "static", "this", "local", "argument", "that", "constant",
            "pointer", "temp"
        };

        public Lexer(string line)
        {
            m_Line = line;
        }

        public void Parse()
        {
            while(m_LineIndex < m_Line.Length)
            {
                char current = PeekChar();
                char next = PeekChar(1);

                switch(m_CurrentState)
                {
                    case State.Base:
                        BaseLexer(current, next);
                        break;
                    case State.Text:
                        TextLexer(current, next);
                        break;
                    case State.Number:
                        NumberLexer(current, next);
                        break;
                    case State.LineComment:
                        LineCommentLexer(current, next);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid state: " + m_CurrentState);
                }
            }
        }

        public void Reject()
        {
            // Grab the current character and proceed to the next index,
            // but don't worry about storing it.
            NextChar();
        }

        public void Accept()
        {
            // Grab the current character and proceed to the next index.
            char c = NextChar();

            m_Data.Append(c);
        }

        public void EmitToken(TokenType type)
        {
            m_Tokens.Add(new Token(type, m_LineNumber, m_CharPosition, m_Data.ToString()));
            m_Data.Clear();
        }

        private char PeekChar(int lookAhead = 0)
        {
            if(m_LineIndex + lookAhead >= m_Line.Length)
            {
                return Char.MinValue;
            }

            return m_Line[m_LineIndex + lookAhead];
        }

        /// <summary>
        /// Increases the index of the string being processed. If the current character
        /// is a newline character, the <code>Line Number</code> is increased by one and
        /// the <code>Character Position</code> is set to zero. Otherwise, the <code>
        /// Character Position</code> is just increased by one.
        /// </summary>
        /// <returns>the character that was just processed</returns>
        private char NextChar()
        {
            if(m_LineIndex >= m_Line.Length)
            {
                return Char.MinValue;
            }

            char currentChar = m_Line[m_LineIndex];
            m_LineIndex++;

            if(currentChar == '\n')
            {
                m_LineNumber++;
                m_CharPosition = 0;
            }

            m_CharPosition++;

            return currentChar;
        }

        /// <summary>
        /// The base state is in charge of discarding all whitespace, and 
        /// setting the appropriate state needed.
        /// </summary>
        private void BaseLexer(char current, char next)
        {
            if(Char.IsWhiteSpace(current))
            {
                if (current == '\n')
                {
                    Accept();
                    EmitToken(TokenType.NewLine);
                }
                else
                {
                    Reject();
                }
            }
            else if(Char.IsLetter(current) || validCharacters.Contains(current))
            {
                m_CurrentState = State.Text;
            }
            else if(current == '/' && next == '/')
            {
                m_CurrentState = State.LineComment;
            }
            else if(current == '(')
            {
                Accept();
                EmitToken(TokenType.LeftParen);
                m_CurrentState = State.Text;
            }
            else if(current == ')')
            {
                Accept();
                EmitToken(TokenType.RightParen);
            }
            else if(Char.IsDigit(current))
            {
                m_CurrentState = State.Number;
            }
        }

        private void TextLexer(char current, char next)
        {
            if(Char.IsLetterOrDigit(current) || validCharacters.Contains(current))
            {
                Accept();
            }
            else
            {
                if(instructions.Contains(m_Data.ToString()))
                {
                    EmitToken(TokenType.Instruction);
                    m_CurrentState = State.Base;
                }
                else if(memorySegment.Contains(m_Data.ToString()))
                {
                    EmitToken(TokenType.MemorySegment);
                    m_CurrentState = State.Base;
                }
                else
                {
                    EmitToken(TokenType.Text);
                    m_CurrentState = State.Base;
                }
            }

            // If we're at the end of the stream.
            if(next == Char.MinValue)
            {
                if (instructions.Contains(m_Data.ToString()))
                {
                    EmitToken(TokenType.Instruction);
                    m_CurrentState = State.Base;
                }
                else if (memorySegment.Contains(m_Data.ToString()))
                {
                    EmitToken(TokenType.MemorySegment);
                    m_CurrentState = State.Base;
                }
                else
                {
                    EmitToken(TokenType.Text);
                    m_CurrentState = State.Base;
                }
            }
        }

        private void LineCommentLexer(char current, char next)
        {
            Reject();

            if (next == '\n')
            {
                m_CurrentState = State.Base;
            }
        }

        private void NumberLexer(char current, char next)
        {
            if(Char.IsDigit(current))
            {
                Accept();
            }

            if(!Char.IsDigit(next))
            {
                EmitToken(TokenType.Number);
                m_CurrentState = State.Base;
            }
        }
    }
}
