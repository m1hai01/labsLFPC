using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chomsky
{
    internal class RenamingElimination
    {
        private Dictionary<string, List<string>> unitList;
        internal bool HasUnit(Dictionary<string, List<string>> transitions)
        {
            //verifica dfaca state are un caracter Mare
            foreach (var (_, list) in transitions)
            {
                if (list.Any(state => state.Length == 1 && state.Any(char.IsUpper))) 
                    return true;
            }

            return false;
        }
        internal void AddUnits(Dictionary<string, List<string>> transitions) 
        {
            unitList = new Dictionary<string, List<string>>(); //in case S -> B|C
            foreach (var (key, list) in transitions)
            {
                int size = list.Count;
                for (int i = 0; i < size; i++)
                {
                    if (list[i].Length == 1 && char.IsUpper(char.Parse(list[i])))
                    {
                        if (!unitList.ContainsKey(key)) unitList.Add(key, new List<string>() { list[i] });
                        
                        else unitList[key].Add(list[i]);
                        
                    }
                }
            }
        }

        internal void AddStates(Dictionary<string, List<string>> transitions)
        {
            foreach (var (unit, list) in unitList)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    transitions[unit].Remove(list[i]);

                    
                    foreach (var state in transitions[list[i]]) 
                    {
                        //duplicate
                        if (!transitions[unit].Contains(state)) transitions[unit].Add(state); 
                        
                    }
                }
            }
        }
    }
}
