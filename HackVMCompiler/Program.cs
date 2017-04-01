using HackCompiler.Hack;
using System;

namespace HackCLI
{
    static class EntryPoint
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var program = new Program(@"C:\Users\Nicholas Bailey\Desktop\Coding\nand2tetris\projects\08\ProgramFlow\BasicLoop");
            var compiler = new Compiler(program);
            compiler.Compile();

            foreach(var assembly in compiler.Assemblies)
            {
                assembly.Write(@"C:\Users\Nicholas Bailey\Desktop\Coding\nand2tetris\projects\08\ProgramFlow\BasicLoop");
            }
            
            Console.ReadLine();
        }
    }
}
