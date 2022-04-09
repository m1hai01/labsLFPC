using System.Text;

namespace Chomsky
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var lines = System.IO.File.ReadAllLines(@"D:\UTM\Anul2\Semestru2\LFPC\lfpclabs\src\com\company\lab4\Chomsky\source2.txt");
            var transitions = Initialize(lines);
            var cfg = new CFG(transitions);
            cfg.CFGtoCNF();
        }

        static Dictionary<string, List<string>> Initialize(string[] path)
        { 
            // adaugam gramatica din file in dictionar de array
            var transitions = new Dictionary<string, List<string>>();
            foreach (var line in path)
            {
                if (!transitions.ContainsKey(line[0].ToString()))
                {
                    transitions.Add(line[0].ToString(), new List<string>());
                    AddTransition(line);
                }
                else AddTransition(line);
                
            }

            void AddTransition(string line)
            {
                string substr = line.Substring(line.IndexOf('>') + 1, line.Length - 2);
                transitions[line[0].ToString()].Add(substr);
            }

            return transitions;
        }

    }
}