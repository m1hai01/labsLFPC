using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chomsky
{
    internal class EliminationInaccessible
    {
        private HashSet<string> nonTerminals = new() {"S"}; //deobicei S e accesibil

        internal void FindNonTerminals(Dictionary<string, List<string>> transitions)
        {
            
            int iteration = 1;
            while (iteration != 0)
            {
                //incem cu S si iteram pentru a gasi stateurile accesibile
                int size = nonTerminals.Count;
                foreach (var reachableTerminal in nonTerminals.ToList())
                {
                    foreach (var potentialReachable in transitions[reachableTerminal]) 
                    {
                        foreach (var ch in potentialReachable) 
                        {
                            if (char.IsUpper(ch) && !nonTerminals.Contains(ch.ToString()))
                            {
                                nonTerminals.Add(ch.ToString());
                                iteration++;
                            }
                        }
                    }
                }

                iteration--;
            }
        }

        internal void EliminationNonTerminals(Dictionary<string, List<string>> transitions)
        {
            
            foreach (var (key, _) in transitions)
            {
                if (!nonTerminals.Contains(key)) transitions.Remove(key);
                
            }
        }
    }
}
