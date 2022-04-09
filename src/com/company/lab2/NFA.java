package com.company.lab2;

import java.util.*;

public class NFA {

    private ArrayList<String> _states;
    private String _startState;
    private String _currentState;
    private ArrayList<String> _finalState;
    private ArrayList<String> _separateInputs;
    private Map<Pair, String> _transitionMap;


    public NFA(ArrayList<String> states, String startState, String currentState, ArrayList<String> finalStates, ArrayList<String> separateInputs, Map<Pair, String> transitionMap) {
        _states = states;
        _startState = startState;
        _currentState = currentState;
        _finalState = finalStates;
        _separateInputs = separateInputs;
        _transitionMap = transitionMap;
    }

    public void PrintAutomaton() {
        System.out.print("States: ");
        for (var  state : _states) System.out.print(state + " ");

        System.out.println();

        System.out.println("Starting state:" + _startState);
        System.out.println("Current state:" + _currentState);
        System.out.print("Final states are:");

        for (var finals : _finalState) System.out.print(finals + " ");

        System.out.println();

        System.out.print("Alphabet: ");

        for (var inpState: _separateInputs) System.out.print(inpState);
        System.out.println();

        System.out.println();
        String[][] table = new String[_states.size() + 1][ _separateInputs.size() + 1];
        for (int i = 0; i < _states.size(); i++) table[i + 1][0] = _states.get(i);

        for (int i = 0; i < _separateInputs.size(); i++) table[0][i + 1] = _separateInputs.get(i);

        for (int i = 1; i < table.length; i++)
        {
            for (int j = 1; j < table[0].length; j++)
            {
                var pair = new Pair(table[i][0], table[0][j]);
                if(_transitionMap.containsKey(pair)) table[i][j] = _transitionMap.get(pair);
                else table[i][j] = "-";
            }
        }

        table[0][0] = "";

        for (int i = 0; i < table.length; i++)
        {
            for (int j = 0; j < table[1].length; j++)
            {
                if(table[i][j].length() == 4) System.out.print(table[i][j] + "\t\t");
                else if(table[i][j].length() == 6) System.out.print(table[i][j] + "\t\t");
                else System.out.print(table[i][j] + "\t\t\t");
            }
            System.out.println();
        }
    }
    public String Verification(String inpValue)
    {
        //bbd
        _currentState = _startState;

        for (var symbol : inpValue.toCharArray())
        {
            var temp = new Pair(_currentState, String.valueOf(symbol));
            if (!_transitionMap.containsKey(temp)) //nu este asa litera in input state
                return "No";

            _currentState = _transitionMap.get(temp);
        }

        if (_finalState.contains(_currentState))
            return "Yes";

        return "No";
    }

    public void ConvertNFA()
    {
        //in queue vom adauga toate state noi care nu au fost analizate
        Queue<String> queue = new PriorityQueue<>();
        Map<Pair,String> newTransitions = new HashMap<>();
        //citeste toate stateurile care deja o fost
        HashSet<String> presentStates = new HashSet<>(); //  adugam q0 ca state existent
        //adaugam q0 in queue si se adauga in presentStates deoarece este analizat
        ConversionInitiate( presentStates,  queue);

        while (queue.size() > 0)
        {
            if (_states.contains(queue.peek()))//aici lucram cu staturile simple decompuse
            {
                for (var symbol : _separateInputs)
                {
                    AddNewState(queue, newTransitions, presentStates, symbol);
                }

                queue.remove();
            }
            else
            {
                // aranjam in forma normala,separama q1q2 in q1,q2
                List<String> decomposedStates =  BreakDownQueueState(queue.peek());

                for (var symbol : _separateInputs) //verifcam fieare state ex: q1 si q2 pentru a dupa facem pentru b
                {
                    //ca sa nu avem valori identice cand verificam state ompuse
                    HashSet<String> foundStates = AddNewComposedState(newTransitions, symbol, decomposedStates);

                    AddFinalState(foundStates, queue, symbol, newTransitions, presentStates);

                }

                queue.remove();
            }
        }

        AutomatonUpdate(newTransitions);

    }
    public void ConversionInitiate(HashSet<String> presentStates,  Queue<String> queue)
    {
        System.out.println("Initializing conversion. Adding q0 to the new transition table");
        presentStates.add("q0");

        //push la state initial in queue
        queue.add(_startState);
    }

    public void AutomatonUpdate(Map<Pair, String> input)
    {
        //update NFA
        _states.clear();

        for(Pair key : input.keySet()){
            if (!_states.contains(key.getKey()))
                _states.add(key.getKey());
            if(!_states.contains(input.get(key)))
                _states.add(input.get(key));
        }

        _transitionMap = input;

        int len = _finalState.size();
        for (int i = 0; i < len; i++)
        {
            for (var state : _states)
            {
                if (state.contains(_finalState.get(i)) && state != _finalState.get(i)) //adds the final states, not considering the one that is already there
                    _finalState.add(state);
            }
        }
    }

    public void AddNewState(Queue<String> queue,  Map<Pair,String> newTransitions,  HashSet<String> presentStates, String symbol)
    {
        var pair = new Pair(queue.peek(), symbol);//creez o pereche si verifi daca aceasta este in primul transition table
        if (_transitionMap.containsKey(pair))
        {
            //daca satisface conditia in noul tabel pun stateul destinatar(q1)  q0 : a -> q1
            System.out.println("New transition! " + queue.peek() + " : " + symbol + " -> " + _transitionMap.get(pair) + '\n');
            newTransitions.put(pair, _transitionMap.get(pair));//migram transition table la new table
            if (!presentStates.contains(_transitionMap.get(pair))) // verificam daca presentStates este deja in transition table
            {
                // din motiv ca e new state , il adaugam la queue and to the presebtSates
                System.out.print(_transitionMap.get(pair) + " will be tested\n");
                queue.add(_transitionMap.get(pair));
                presentStates.add(_transitionMap.get(pair));//il adaug in presentSt ca sa nu fie repetari
            }
        }
    }
    //le reunim si le punem in noul tabel
    public void AddFinalState(HashSet<String>foundStates, Queue<String>queue, String symbol, Map<Pair, String> newTransitions, HashSet<String> presentStates ){
        if (foundStates.size() > 0)
        {
            var sortedStatesFromTable = new ArrayList<>(foundStates);
            Collections.sort(sortedStatesFromTable);
            String finalState = sortedStatesFromTable.get(0); //creaem final state

            for (int i = 1; i < sortedStatesFromTable.size(); i++)
                finalState += sortedStatesFromTable.get(i);

            Pair pair = new Pair(queue.peek(), symbol);
            System.out.println("New transition! " + queue.peek() + " : " + symbol + " -> " + finalState + '\n');
            newTransitions.put(pair, finalState); //adaugam acesta in final state
            if (!presentStates.contains(finalState)) //in case it is not present in the transition table, add it to the queue and add to the presentStates to not consider it again//in caz ca acesta nu e prezent in transitiontable,il adaugam in queue si in presentState ca sa nu fie verifiat din nou
            {
                System.out.println(finalState + " will be tested\n");
                queue.add(finalState);
                presentStates.add(finalState);
            }
        }
    }

    public List<String> BreakDownQueueState(String s1)
    {
        List<String> temp = new ArrayList<>();
        for (int i = 0; i < s1.length(); i += 2) {
            temp.add(s1.substring(i, i + 2));
        }
        return temp;
    }

    //gasim stateurile care trebuie sa fie reunite
    public HashSet<String> AddNewComposedState(Map<Pair, String> newTransitions, String symbol, List<String> decomposed)
    {
        var newStates = new HashSet<String>();
        for (var state : decomposed)
        {
            var pair = new Pair(state, symbol);
            var temp = new ArrayList<String>(); //decompose compose states
            if (newTransitions.containsKey(pair)) //in transition table facuta cautam state
            {
                for (int i = 0; i < newTransitions.get(pair).length(); i += 2) //in caz ca perechea e compusa
                    temp.add(newTransitions.get(pair).substring(i, i + 2));

                for (var s : temp) // ficare state din decomposed list se duce in SEt
                    newStates.add(s);
            }
            else if (_transitionMap.containsKey(pair)) //
            {
                for (int i = 0; i < _transitionMap.get(pair).length(); i += 2) //in caz ca perechea e ompusa
                    temp.add(_transitionMap.get(pair).substring(i, i + 2));

                for (var s : temp) //ficare state din decomposed list se duce in SEt
                    newStates.add(s);
            }
        }

        return newStates;
    }

}

