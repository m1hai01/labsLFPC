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
            var word = FirstStageParse(writeLine);
            Console.WriteLine(word);
            MainParse(word);
        }


        //parse
        public void MainParse(string writeLine)

        {
            while (true)
            {

                //abcefedg
                var word = writeLine.ToString(); //parsed word with symbols

                //cautam de la dreapta la stanga paranteze inchise
                
                int parenthesisLeft = word.Length - 1;
                int parenthesisRight = word.Length - 1;
                
                for (int j = 0; j < word.Length - 1; j++)
                {
                    if (word[j] == '<')
                    {
                        parenthesisLeft = j;
                        break;
                    }

                }
                

                for (int j = parenthesisLeft; j < word.Length - 1; j++)
                {
                    if (word[j] == '<')
                    {
                        for (int i = j; i < word.Length - 1; i++)
                        {
                            if (word[i] == '<')
                            {
                                parenthesisLeft = i;
                                break;
                            }
                        }
                    }
                    if (word[j] == '>')
                    {
                        parenthesisRight = j;
                        break;
                    }
                }

                /*while (word[parenthesisLeft] != '<')
              {
                  parenthesisLeft--;
              }*/


                /*parenthesisRight = parenthesisLeft;

                while (word[parenthesisRight] != '>')
                {
                    parenthesisRight++;
                }*/
                var length = parenthesisRight - parenthesisLeft - 1;
                var wordInsideParenthesis = word.Substring(parenthesisLeft + 1, length);

                wordInsideParenthesis = wordInsideParenthesis.Replace("=", string.Empty);

                var prevChar = writeLine[parenthesisLeft - 1];
                var nextChar = writeLine[parenthesisRight + 1];

                var transitionReplaced = ReplaceInsideParenthesis(wordInsideParenthesis);

                if (transitionReplaced.Count == 0) //nu-s transitii cu care putem inlocui cuvantul
                {
                    Console.WriteLine("Rejected");
                    return;
                }

                //one transition
                if (transitionReplaced.Count == 1)
                {
                    //inlocuim cuvantul cu transitia (transitiaSchimbata, caracter prev, nextChar)
                    wordInsideParenthesis = ReplaceSigns(transitionReplaced[0], prevChar, nextChar);
                }
                else
                {
                    //daca is mai multe transitii, rezolvam ambiguitatea
                    wordInsideParenthesis = Ambiguity(transitionReplaced, writeLine, parenthesisLeft, parenthesisRight);
                }

                writeLine = writeLine.Remove(parenthesisLeft, length + 2);
                writeLine = writeLine.Insert(parenthesisLeft, wordInsideParenthesis);

                Console.WriteLine(writeLine);

                //base case
                if (writeLine == "$<S>$")
                {
                    Console.WriteLine("Accepted");
                    return;
                }
            }
        }

        
        private string Ambiguity(List<string> replacement, string writeLine, int left, int right)
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

            modifyOfState = symbols[0] + modifyOfState + symbols[1];
            return modifyOfState;
        }


        private string FirstStageParse(string writeLine)
        {
            //state initial
            var path = "$< >$";
            int index = 2;
            try
            {
                if (writeLine.Length == 1)
                {
                    path = path.Remove(index, 1).Insert(2, writeLine);
                    return path;
                }

                for (int i = 0; i < writeLine.Length - 1; i++)
                {
                    int first = _indexes[writeLine[i]];
                    int second = _indexes[writeLine[i + 1]];

                    //sign is intersection of the two adjacent characters
                    var sign = _matrix[first, second].ToString();
                    var word = writeLine[i] + sign + writeLine[i + 1];
                    path = path.Remove(index, 1).Insert(index, word);
                    index += 2;
                }
            }
            catch
            {
                Console.WriteLine("Word invalid. Try again");
                Environment.Exit(-1);
            }

            return path;
        }
        //
        private string ReplaceSigns(string replace, char previous, char next)
        {
            string leftSign = "", rightSign = "";

            if (previous == '$')
            {
                leftSign = "<";
            }
            else
            {
                //left sign
                int first = _indexes[previous];
                int second = _indexes[replace[0]];
                leftSign =  _matrix[first, second].ToString();
            }

            if (next == '$')
            {
                rightSign = ">";
            }
            else
            {
                //right sign
                int first = _indexes[replace[0]];
                int second = _indexes[next];
                rightSign = _matrix[first, second].ToString();
            }

            return leftSign + replace + rightSign;
        }

        private List<string> ReplaceInsideParenthesis(string wordInsideParenthesis)
        {
            var changingCharacter = new List<string>();
            foreach (var (key, dictionary) in _transitions){
                for (var i = 0; i < dictionary.Count; i++){
                    var transition = dictionary[i];
                    if (transition.Equals(wordInsideParenthesis)) 
                        changingCharacter.Add(key);
                }
            }

            if (changingCharacter.Count != 0)
                return changingCharacter;
            return new();//not found
        }
    }
}
