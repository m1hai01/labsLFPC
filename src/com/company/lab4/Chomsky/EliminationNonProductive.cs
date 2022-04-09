using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chomsky
{
    internal class EliminationNonProductive
    {
        private HashSet<string> nonTerminals = new();

        internal void AddNonTerminals(Dictionary<string, List<string>> transitions)
        {
            foreach (var (key, list) in transitions)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Length == 1 && char.IsLower(char.Parse(list[i])))  nonTerminals.Add(key);  
                }
            }
        }

        internal void CheckTransitions(Dictionary<string, List<string>> transitions)
        {
            //verificam daca   stateurile nonterminale, care nu duc in terminale unice,duc in staturi nonterminale care duc in terminale unice
            int iteration = 1;
            while (iteration != 0)
            {
                foreach (var (key, list) in transitions)
                {
                    if (nonTerminals.Contains(key)) continue; //pasim peste stateurile pozitive

                    //verificam daca e pozitiv
                    for (int i = 0; i < list.Count; i++)
                    {
                        var toBeCheckedNonTerminals = new List<string>();
                        for (int j = 0; j < list[i].Length; j++)
                        {
                            if(char.IsUpper(list[i][j])) 
                                toBeCheckedNonTerminals.Add(list[i][j].ToString());
                        }

                        //verificam din nou pe toate non terminale
                        if (toBeCheckedNonTerminals.All(nonTerminal => nonTerminals.Contains(nonTerminal)))
                        {
                            nonTerminals.Add(key);
                            iteration++;
                            break;
                        }
                    }
                }

                iteration--;
            }
        }

        internal void RemoveOccurrencesKey(Dictionary<string, List<string>> transitions)
        {
            //remove all occurrences of that key
            foreach (var (key, list) in transitions)
            {
                if (!nonTerminals.Contains(key))
                {
                    //facem remove la intrus
                    transitions.Remove(key);

                    
                    foreach (var (tempKey, tempList) in transitions)
                    {
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            //scoatem unde se mai contine intrus
                            transitions[tempKey].RemoveAll(state => state.Contains(key));
                        }
                    }
                }
            }
        }
    }
}
