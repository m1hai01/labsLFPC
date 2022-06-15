using System;
using System.Text;

namespace SimplePrecedence
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var lines = System.IO.File.ReadAllLines(
                @"C:\Users\mustu\Desktop\lfpclabs\src\com\company\lab5\SimplePrecedence\source.txt");
            var terminals = lines[0].Split(',').ToList();
            var nonTerminals = lines[1].Split(',').ToList();
            var transitions = Initialization(lines[2..]);
            var s_precedence = new SimplePrecedenceParser(transitions, terminals, nonTerminals);

            s_precedence.Start();
            s_precedence.DisplayTransitions();
            s_precedence.DisplayFirstAndLast();
            s_precedence.DisplayMatrix();
            
            
            while (true)
            {
                Console.WriteLine("Type in a string  to check it: ");
                Console.WriteLine("Press 'S' to stop the program. ");
                var write = Console.ReadLine();
                
                if (write == null) 
                    continue;
                if (write == "S")
                    break;

                s_precedence.StringVerification(write);
            }


            static Dictionary<string, List<string>> Initialization(string[] path)
            {
                //adds grammar from file to dictionary of arrays
                var transitions = new Dictionary<string, List<string>>();
                foreach (var line in path)
                {
                    if (!transitions.ContainsKey(line[0].ToString()))
                    {
                        transitions.Add(line[0].ToString(), new List<string>());
                        TransitionAdding(line);
                    }
                    else
                    {
                        TransitionAdding(line);
                    }
                }

                void TransitionAdding(string line)
                {
                    string substr = line.Substring(line.IndexOf('>') + 1, line.Length - 2);
                    transitions[line[0].ToString()].Add(substr);
                }

                return transitions;
            }
        }
    }
}