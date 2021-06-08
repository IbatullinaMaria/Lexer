using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer.TokenValue;

namespace Lexer
{
    class Program
    {
        static void Main()
        {
            try
            {
                using (StreamReader reader = new StreamReader("../../in.txt"))
                {
                    string line = "";
                    var automata = new Automata();
                    const int startOfLine = 0;
                    int lineNumber = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("LineNumber: " + lineNumber);
                        if (line.Count() != 0) 
                        { 
                            //if (Global.multyCommentFlag != 1)
                            //{
                                automata.ProcessLine(line, startOfLine);
                           // }
                           // else
                           // {
                               // automata.ProcessLimiter(line, startOfLine);
                           // }
                             
                        }
                        //if (Global.multyCommentFlag != 1) { lineNumber++; }
                       // else
                       // {
                       //     lineNumber = lineNumber + Global.multyCommentFlag;
                       //     Global.multyCommentFlag = 0;
                       // }
                        lineNumber++;

                    }
                    reader.Close();
                    Console.ReadLine();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(err.Message);
                Console.ReadLine();
            }
        }

    }
}