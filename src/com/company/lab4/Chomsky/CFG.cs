using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chomsky
{
    internal class CFG
    {
        private Dictionary<string, List<string>> Transitions;
        private EpsilonElimination EpsilonElimination = new();
        private RenamingElimination RenamingElimination = new();
        private EliminationNonProductive EliminationNonProductive = new();
        private EliminationInaccessible EliminationInaccessible = new();
        private ChomskyNormalForm ChomskyNormalForm = new();

        public CFG(Dictionary<string, List<string>> transitions)
        {
            Transitions = transitions;
        }

        public void CFGtoCNF()
        {
            Console.WriteLine("*CFG: ");
            PrintTransitions();
            EliminationEpsTransitions();

            Console.WriteLine("*Elimination of ε productions ");
            PrintTransitions();
            EliminationRenamingTransitions();

            Console.WriteLine("*Elimination of renaming: ");
            PrintTransitions();
            EliminationNonProductiveTransactions();

            Console.WriteLine("*Elimination of nonproductive symbols. ");
            PrintTransitions();
            EliminationInaccessibleTransactions();

            Console.WriteLine("*Elimination of inaccesibile symbols ");
            PrintTransitions();
            CNF();

            Console.WriteLine("*Chomsky normal form: ");
            PrintTransitions();
        }

        private void PrintTransitions()
        {
            foreach (var (key, list) in Transitions)
            {
                Console.Write($"{key} -> ");
                for (int i = 0; i < list.Count; i++)
                {
                    if(i < list.Count - 1) Console.Write($"{list[i]} | ");
                    else Console.Write(list[i]);
                }

                Console.WriteLine();
            }

            Console.WriteLine('\n'+ "===========================================================");
        }

        private void EliminationEpsTransitions()
        {
            
            while (EpsilonElimination.HasEpsilon(Transitions))
            {
                EpsilonElimination.GenerateEpsilonStates(Transitions);//gasim toate stateurile care au epsilon 
                EpsilonElimination.AddNewEpsilonStatesToTransitions(Transitions);
            }
        }

        private void EliminationRenamingTransitions()
        {
            while (RenamingElimination.HasUnit(Transitions))//atata timp cat state are un caracter Mare
            {
                RenamingElimination.AddUnits(Transitions);
                RenamingElimination.AddStates(Transitions);
            }
        }

        private void EliminationNonProductiveTransactions()
        {
            EliminationNonProductive.AddNonTerminals(Transitions);//adaugam stateurile care  duc in terminale unice
            EliminationNonProductive.CheckTransitions(Transitions);
            EliminationNonProductive.RemoveOccurrencesKey(Transitions);
        }

        private void EliminationInaccessibleTransactions()
        {
            EliminationInaccessible.FindNonTerminals(Transitions);
            EliminationInaccessible.EliminationNonTerminals(Transitions);
        }

        private void CNF()
        {
           ChomskyNormalForm.NormalizeTerminals(Transitions);
           Console.WriteLine("*Normalizing terminals: ");
           PrintTransitions();
           ChomskyNormalForm.NormalizeManySymbols(Transitions);
        }

    }
}
