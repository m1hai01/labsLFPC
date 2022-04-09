package com.company.lab1;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class DFA {

    private List<String> _iStates;
    private String _startState;
    private String _currentState;
    private List<String> _finalState;
    private String[] _separateInputs;
    private Map<Map<String, String>, String> _transitionMap;


    public DFA(List<String> iStates, String startState, String currentState, List<String> finalStates, String[] separateInputs, Map<Map<String, String>, String> transitionMap) {
        _iStates = iStates;
        _startState = startState;
        _currentState = currentState;
        _finalState = finalStates;
        _separateInputs = separateInputs;
        _transitionMap = transitionMap;
    }

    public void PrintAutomaton() {
        System.out.print("States: ");
        for (var  state : _iStates) {
            System.out.print(state);
        }

        System.out.println();

        System.out.println("Starting state:" + _startState);
        System.out.println("Current state:" + _currentState);
        System.out.print("Final states are:");

        for (var finals : _finalState) {
            System.out.println(finals);
        }

        System.out.println();

        System.out.print("Alphabet: ");

        for (var inpState: _separateInputs)
        {
            System.out.print(inpState);
        }

        System.out.println();


        for (Map.Entry<Map<String, String>, String> entry : _transitionMap.entrySet())
        {
            Map<String,String> childMap = entry.getKey();
            for (Map.Entry<String,String> entry2 : childMap.entrySet()) {
                String start = entry2.getKey();
                String inpState = entry2.getValue();
                String finish = String.valueOf(entry.getValue());
                System.out.print(start + inpState + "->" + finish + " ");
            }

        }


    }
    public String Verification(String inpValue)
    {
        //bbd
        _currentState = _startState;

        for (var symbol : inpValue.toCharArray())
        {
            Map temp = new HashMap<String, String>();
            temp.put(_currentState, String.valueOf(symbol));

            if (!_transitionMap.containsKey(temp))
                return "No";
            //cu get scoatem pe baza la cheie ,valoarea din map
            _currentState = _transitionMap.get(temp);
        }

        if (_finalState.contains(_currentState))
            return "Yes";

        return "No";
    }

}

