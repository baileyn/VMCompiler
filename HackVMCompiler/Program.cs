using HackCompiler.Hack;
using System;

namespace HackCLI
{
    static class EntryPoint
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length != 2 || args[1] == "-h")
            {
                Console.WriteLine("Usage: HackCLI <project folder that contains multiple vm files including sys.vm>");
            }
            else
            {

                var program = new Program(args[1]);
                var compiler = new Compiler(program);

                try
                {
                    compiler.Compile();
                    compiler.Write();
                }
                catch (CompilationException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
