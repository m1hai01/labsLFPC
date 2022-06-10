using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePrecedence
{
    internal class FirstLastTable
    {
        public Dictionary<char, List<char>> FirstSymb = new();
        public Dictionary<char, List<char>> LastSymb = new();

        private Dictionary<string, List<string>> _transitions;

        public void Start(Dictionary<string, List<string>> transitions)
        {
            _transitions = transitions;
            //Adding First
            foreach (var (key, list) in _transitions)
            {
                foreach (var term in list)
                {
                    if (FirstSymb.ContainsKey(key[0]))
                    {
                        //duplicate
                        if (!FirstSymb[key[0]].Contains(term[0]))
                        {
                            FirstSymb[key[0]].Add(term[0]);
                        }
                    }
                    else FirstSymb.Add(key[0], new List<char> { term[0] });


                }
            }
            //Adding Last()
            foreach (var (key, list) in _transitions)
            {
                foreach (var term in list)
                {
                    if (LastSymb.ContainsKey(key[0]))
                    {
                        
                        if (!LastSymb[key[0]].Contains(term[^1]))
                        {
                            LastSymb[key[0]].Add(term[^1]);
                        }
                    }
                    else LastSymb.Add(key[0], new List<char> { term[^1] });

                }
            }

            

            //add remainings of first
            int repetition = 1;

            while (repetition > 0)
            {
                foreach (var (key, list) in FirstSymb)
                {
                    foreach (var character in list.ToList())
                    {

                        if (!key.Equals(character) && char.IsUpper(character))
                        {
                            foreach (var tempChar in FirstSymb[character])
                            {
                                if (!FirstSymb[key].Contains(tempChar))
                                {
                                    FirstSymb[key].Add(tempChar);
                                    if (char.IsUpper(tempChar))
                                    {
                                        repetition++;
                                    }
                                }
                            }
                        }
                    }
                }

                repetition--;
            }

            //add remainings of last
            int repetition1 = 1;

            while (repetition1 > 0)
            {
                foreach (var (key, list) in LastSymb)
                {
                    foreach (var character in list.ToList())
                    {

                        if (!key.Equals(character) && char.IsUpper(character))
                        {
                            foreach (var tempChar in LastSymb[character])
                            {
                                if (!LastSymb[key].Contains(tempChar))
                                {
                                    LastSymb[key].Add(tempChar);
                                    if (char.IsUpper(tempChar))
                                    {
                                        repetition1++;
                                    }
                                }
                            }
                        }
                    }
                }

                repetition1--;
            }
        }

        

        

        public void Display(Dictionary<char, List<char>> dictionary)
        {
            foreach (var (key, list) in dictionary) {
                Console.Write($"{key} -> ");
                for (int i = 0; i < list.Count; i++)
                {
                    if (i < list.Count - 1)  
                    {
                        Console.Write($"{list[i]} | ");
                    }
                    else
                    {
                        Console.Write(list[i]);
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("##############");
        }
    }
}

