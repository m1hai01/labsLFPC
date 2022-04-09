using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Chomsky
{
    internal class ChomskyNormalForm
    {
        //private char constant = '\u03B1';
        private char constant = '\u03C0';
        
        internal void NormalizeTerminals(Dictionary<string, List<string>> transitions)
        {
            
            var rules = new Dictionary<string, string>();
            foreach (var (key, list) in transitions)//parcurg dictionarul
            {
                for (int i = 0; i < list.Count; i++)//parcurg valorile(list) la key
                {
                    if (list[i].Length > 1) //daca e mai mare ca 1
                    {
                        for (int j = 0; j < list[i].Length; j++)//parcurg fiecare simbol din valoare (lista)
                        {
                            if (char.IsLower(list[i][j])) //change all terminals, all
                            {
                                //check if we have in rules that substitution 
                                if (rules.ContainsKey(list[i][j].ToString()))
                                {
                                    //take substring before terminal, the changed terminal and after terminal substring
                                    int temp = j;
                                    string subState = list[i][..temp] +
                                                      rules[list[i][temp].ToString()] +
                                                      list[i][++temp..];
                                    transitions[key][i] = subState;
                                }
                                else
                                {
                                    //a -> X1 (greek letter in my case)
                                    int temp = j;
                                    rules[list[i][temp].ToString()] = $"{constant++}";
                                    string subState = list[i][..temp] +
                                                      rules[list[i][temp].ToString()] +
                                                      list[i][++temp..];
                                    transitions[key][i] = subState;
                                }
                            }
                        }
                    }
                }
            }

            //aplicam regulile la dictionarul principal
            foreach (var (key, value) in rules)
            {
                transitions[value] = new List<string> { key };
            }
        }
        internal void NormalizeManySymbols(Dictionary<string, List<string>> transitions)
        {
            //schimbam tranzitiile care au mai mult de 2 simboluri
            var rules = new Dictionary<string, string>();
            while (HasLong(transitions))
            {
                foreach (var (key, list) in transitions.ToList())
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Length > 2) 
                        {
                            //in caz de avem impar ignoram
                            int size = list[i].Length - (list[i].Length % 2);
                            var subStates = new List<string>();
                            for (int j = 0; j < size; j += 2)
                            {
                                
                                var subState = list[i].Substring(j, 2);
                                subStates.Add(subState);
                            }

                            foreach (var state in subStates) 
                            {
                                if (!rules.ContainsKey(state))
                                {
                                    //adaugam regula
                                    rules.Add(state, constant.ToString());
                                    string changedState = list[i].Replace(state, rules[state]);
                                    transitions[key][i] = changedState;
                                    constant++;
                                }
                                else
                                {
                                    //facem replace la 2 terminal cu 1
                                    string changedState = list[i].Replace(state, rules[state]);
                                    transitions[key][i] = changedState;
                                }
                            }
                        }
                    }
                }
            }

            //adaugam regulile ramase
            foreach (var (key, value) in rules)
            {
                transitions.Add(value, new List<string>() { key });
            }
        }
        internal bool HasLong(Dictionary<string, List<string>> transitions)
        {
            //verificam daca gramatica are staturi mai mare ca 2
            foreach (var (_, list) in transitions)
            {
                if (list.Any(state => state.Length > 2)) 
                    return true;
            }

            return false;
        }
    }
}
