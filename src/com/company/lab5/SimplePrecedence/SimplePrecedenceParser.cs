using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePrecedence
{
    internal class SimplePrecedenceParser
    {
        private List<string> _terminals;
        private List<string> _nonTerminals;
        private Dictionary<char, int> _indexes = new();
        private char[,] _matrix;
        private Dictionary<string, List<string>> _transitions;
        private FirstLastTable firstAndLast = new();
        

        public SimplePrecedenceParser(Dictionary<string, List<string>> transitions, List<string> terminals, List<string> nonTerminals)
        {
            _transitions = transitions;
            _terminals = terminals;
            _nonTerminals = nonTerminals;
        }

        public void DisplayTransitions()
        {
            Console.WriteLine("TRANSITIONS: ");
            foreach (var (key, list) in _transitions)
            {
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

            Console.WriteLine("###########");
        }

        public void DisplayFirstAndLast()
        {
            Console.WriteLine("FIRST: ");
            firstAndLast.Display(firstAndLast.FirstSymb);
            Console.WriteLine("LAST: ");
            firstAndLast.Display(firstAndLast.LastSymb);
        }

        public void DisplayMatrix()
        {
            Console.WriteLine("MATRIX: ");
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(1); j++)
                {
                    if (_matrix[i, j].Equals('\0'))
                    {
                        Console.Write(" ");
                    }
                    Console.Write($"{_matrix[i, j]} ");
                }

                Console.WriteLine();
            }
        }


        public void Start()
        {
            firstAndLast.Start(_transitions);
            MatrixInitialization();
            PrincipleNrOne();
            PrincipleNrTwo();
            PrincipleNrThree();
            PrincipleNrFour();

            
        }

        private void MatrixInitialization()
        {
            var list0 = _terminals.Concat(_nonTerminals).ToList();
            list0.Add("$");
            _matrix = new char[list0.Count + 1, list0.Count + 1];
            for (int i = 0; i < list0.Count; i++)
            {
                _indexes.Add(list0[i][0], i + 1);
                _matrix[i + 1, 0] = list0[i][0];
                _matrix[0, i + 1] = list0[i][0];
            }
        }

        private void AddOperator(char Operator, int rowIndex, int columnIndex)
        {
            _matrix[rowIndex, columnIndex] = Operator;// to the specified rowIndex to columnIndex applies an operator.
        }
        private void PrincipleNrOne()
        {
            foreach (var (_, list) in _transitions)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Length > 1)
                    {
                        for (int j = 1; j < list[i].Length; j++)
                        {
                            char prime = list[i][j - 1];
                            char two = list[i][j];
                            AddOperator('=', _indexes[prime], _indexes[two]);
                        }
                    }
                }
            }
        }

        private void PrincipleNrTwo()
        {
            //lower + upper, lower < First(upper)
            foreach (var (_, list1) in _transitions)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i].Length > 1)
                    {
                        for (int j = 1; j < list1[i].Length; j++)
                        {
                            char terminal = list1[i][j - 1];
                            char nonTerminal = list1[i][j];
                            //primary condition
                            if (char.IsLower(terminal) && char.IsUpper(nonTerminal))
                            {
                                //search nonterminal in FIRST
                                foreach (var character in firstAndLast.FirstSymb[nonTerminal])
                                {
                                    AddOperator('<', _indexes[terminal], _indexes[character]);
                                }
                            }
                        }
                    }
                }
            }
        }

       private void PrincipleNrThree()
        {
        //upper to lower
            foreach (var (_, list2) in _transitions)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    if (list2[i].Length > 1)
                    {
                        for (int j = 1; j < list2[i].Length; j++)
                        {
                            char nonTerminal = list2[i][j - 1];
                            char terminal = list2[i][j];
                            if (char.IsLower(terminal) && char.IsUpper(nonTerminal))
                            {
                                //search nonterminal in LAST
                                foreach (var character in firstAndLast.LastSymb[nonTerminal])
                                {
                                    AddOperator('>', _indexes[character], _indexes[terminal]);
                                }
                            }
                        }
                    }
                }
            }
        }

       private void PrincipleNrFour()
       {
            //signs for $
           var list3 = _terminals.Concat(_nonTerminals).ToList();
           foreach (var symbol in list3)
           {
               AddOperator('<', _indexes['$'], _indexes[symbol[0]]);
               AddOperator('>', _indexes[symbol[0]], _indexes['$']);
           }
       }


        public void StringVerification(string writeLine)
        {
            var word = FirstStageParse("$<" + writeLine);
            if (string.IsNullOrEmpty(word.ToString())) {
                Console.WriteLine("Ignore");
                return;
            }

            Console.WriteLine(word);
            MainParse(word);
        }

        private string Ambiguity(List<string> replacement, StringBuilder writeLine, int left, int right)
        {
            
            var modifyOfState = replacement[0];
            var symbols = new char[2];
            foreach (var substitution in replacement)
            {
                var tempLeft = _matrix[_indexes[writeLine[left - 1]], _indexes[substitution[0]]];
                var tempRight = _matrix[_indexes[substitution[0]], _indexes[writeLine[right + 1]]];
                //priority of right =
                if (tempLeft != '\0' && tempRight == '=')
                {
                    modifyOfState = substitution;
                    symbols[0] = tempLeft;
                    symbols[1] = tempRight;
                }
                // left = and  no right equal 
                else if (tempLeft == '=' && !symbols[1].Equals('=') && tempRight != '\0')
                {
                    modifyOfState = substitution;
                    symbols[0] = tempLeft;
                    symbols[1] = tempRight;
                }
            }

            return modifyOfState;
        }

        //parse
        public void MainParse(StringBuilder writeLine)
        {
            while (true)
            {
                string tempInput = writeLine.ToString();
                if (tempInput.Contains("S"))
                {
                    Console.WriteLine("Recognized");
                    return;
                }


                int left = writeLine.Length - 1;
                int right = writeLine.Length - 1;

                while (writeLine[left] != '<')
                {
                    if (writeLine[left] == '>')
                    {
                        right = left;
                    }
                    left--;
                }
                
                var state = tempInput.Substring(left + 1, right - left - 1);
                var replacement = state.Length == 1 ? SingleGetReplace(state) : ComposedGetReplace(state);

                if (replacement.Count == 0) //empty
                {
                    Console.WriteLine("Rejected");
                    return;
                }

                //one transition
                if (replacement.Count == 1)
                {
                    state = replacement[0];
                }
                else
                {
                    state = Ambiguity(replacement, writeLine, left, right);
                }

                //inside parantheses, modify the state
                writeLine.Remove(left + 1, right - left - 1);
                writeLine.Insert(left + 1, state); 
                right = left + 2;

                // add the required operators in the word fo  pars
                char leftSign = _matrix[_indexes[writeLine[left - 1]], _indexes[writeLine[left + 1]]];
                char rightSign = _matrix[_indexes[writeLine[right - 1]], _indexes[writeLine[right + 1]]];

                writeLine[left] = leftSign;
                writeLine[right] = rightSign;

                Console.WriteLine(writeLine);
                ;
            }
        }

        private StringBuilder FirstStageParse(string writeLine)
        {
            //make initial string 
            for (int i = 3; i < writeLine.Length; i++) {
                //incorect
                if (!(_nonTerminals.Contains(writeLine[i - 1].ToString()) ^ _terminals.Contains(writeLine[i - 1].ToString()))) return new StringBuilder();

                if (!(_nonTerminals.Contains(writeLine[i].ToString()) ^ _terminals.Contains(writeLine[i].ToString()))) return new StringBuilder();
                

                int first = _indexes[writeLine[i - 1]];
                int second = _indexes[writeLine[i]];

                if (_matrix[first, second] == '\0')
                {
                    return new StringBuilder();
                } 
            


                writeLine = writeLine.Insert(i, _matrix[first, second].ToString());
                i++;
            }

            writeLine += ">$";
            var result = new StringBuilder(writeLine);
            return result;
        }

        private List<string> SingleGetReplace(string topOfStack)
        {
            var result = new List<string>();
            foreach (var (key, list) in _transitions)
            {
                
                foreach (var transition in list)
                {
                    if (transition.Equals(topOfStack)) result.Add(key);
                    
                }
            }

            
            return result.Count != 0 ? result : new List<string>(); //dont found transition
        }

        private List<string> ComposedGetReplace(string topOfStack)
        {//for composed
            var miniStates = topOfStack.Split('=').ToList();
            string state = string.Concat(miniStates);
            return SingleGetReplace(state);
        }
    }
}
