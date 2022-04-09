using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chomsky
{
    internal class EpsilonElimination
    {
        private List<string> epsilonStates;
        internal bool HasEpsilon(Dictionary<string, List<string>> transitions)
        {
            //
            foreach (var (_, list) in transitions)
            {
                if (list.Any(state => state.Equals("ε"))) return true;
            }

            return false;
        }

        internal void GenerateEpsilonStates(Dictionary<string, List<string>> transitions)
        {
            epsilonStates = new List<string>();
            //gasim toate stateurile care au epsilon 
            foreach (var (key, list) in transitions)
            {
                if (list.Any(state => state.Equals("ε"))) epsilonStates.Add(key);
            }
        }

        internal void AddNewEpsilonStatesToTransitions(Dictionary<string, List<string>> transitions)
        {

            foreach (var epsilon in epsilonStates)
            {
                // stergem epsilon
                transitions[epsilon].Remove("ε");

                //epsilonState = B;
                //adaugam nou state care are epsilon
                foreach (var (key, list) in transitions)
                {
                    int size = list.Count;
                    for (int i = 0; i < size; i++)
                    {
                        //S -> B,
                        //B -> ε
                        if (list[i].Equals(epsilon))
                        {
                            //il facem ca epsilonState
                            transitions[key][i] = "ε";
                        }
                        else if (list[i].Contains(epsilon))
                        {
                            //S -> bB, B -> ε
                            // verificam daca avem epsilonState compus
                            //cate B avem
                            var compositeEpsilon = list[i].Count(ch => ch == char.Parse(epsilon));
                            if (compositeEpsilon == 1)
                            {
                                //scoatem din stateurile celelate stateu cu eps
                                string add = list[i].Replace(epsilon, "");
                                //omitem duplicarile
                                if(!transitions[key].Contains(add))
                                    transitions[key].Add(add);
                            }
                            else
                            {
                                //S -> BCsB, B -> ε
                                var smallerStates = GenerateStates(list[i], epsilon);
                                foreach (var state in smallerStates)
                                {
                                    transitions[key].Add(state);
                                }
                            }
                        }
                    }
                }
            }
            
        }
        internal List<string> GenerateStates(string input, string epsilonState)
        {
            //BCBBs | BCBs | CBBs | BCs | CBs | Cs
            var list = new List<string>(); // lista temporara care va contine toate starile decompuse
            var queue = new Queue<string>();
            queue.Enqueue(input);
            while (queue.Count != 0)
            {
                string state = queue.Peek();
                for (int i = 0; i < state.Length; i++)
                {
                    if (state[i] == char.Parse(epsilonState)) //daca char curent e B
                    {
                        //  luam substringul pana la B si dupa B
                        string subState = state[..i] + state[++i..];
                        if (subState.Length == 0) 
                        {
                            list.Add("ε");
                        }
                        else if (!list.Contains(subState)) //duplicate
                        {
                            queue.Enqueue(subState);
                            list.Add(subState);
                        }
                        
                    }
                }

                queue.Dequeue();
            }

            return list;
        }
    }
}
