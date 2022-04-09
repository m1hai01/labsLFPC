package com.company.lab2;


//import javafx.util.Pair;
import java.util.*;
import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Scanner;

class lab2 {
    public static void main(String[] args) throws FileNotFoundException {
        File file = new File("D:\\UTM\\Anul2\\Semestru2\\LFPC\\lfpclabs\\src\\com\\company\\lab2\\test1.txt");
        Scanner scanner = new Scanner(file);

        ArrayList<String> a1 = new ArrayList<String>();
        //completez array listul cu stringurile din txt
        while (scanner.hasNextLine()) {
            String string = scanner.nextLine();
            a1.add(string);
        }


        String startState = "q0";
        String currentState = "q0";

        ArrayList<String> finalStates = new ArrayList<String>();

        //am scos virgulele din primul sir din txt -states
        String states = a1.get(0);
        String[] separationStates = states.split(",");
        var sepStates = new ArrayList<String>();
        Collections.addAll(sepStates, separationStates);


        //in imputs punem inputurile din txt fara virgula
        String inputs = a1.get(1);
        String[] separateInputs = inputs.split(",");
        var sepInputs = new ArrayList<String>();
        Collections.addAll(sepInputs, separateInputs);

        String fstates = a1.get(2);
        String[] separateFStates = fstates.split(",");
        var sepFStates = new ArrayList<String>();
        Collections.addAll(sepFStates, separateFStates);

        Map<Pair,String> transitionMap = new HashMap<>();

        for (int i = 3; i < a1.size() ; i++) {
            var fromState = a1.get(i).substring(0, a1.get(i).indexOf(':'));
            var symbol = a1.get(i).substring(a1.get(i).indexOf(':') + 1, a1.get(i).indexOf(':') + 2);
            var toState = a1.get(i).substring(a1.get(i).indexOf('>') + 1, a1.get(i).indexOf('>') + 3);
            var pair = new Pair(fromState, symbol);

            if (!transitionMap.containsKey(pair))
            {
                transitionMap.put(new Pair(fromState, symbol), toState);
                continue;
            }
            var val = transitionMap.get(pair);
            val += toState;
            transitionMap.replace(pair, val);

        }

        //System.out.println("iState:" + iStates);
        //System.out.println("finalState" + finalStates);
        //System.out.println("stateMap" + stateMap);
        System.out.println("transitionMap" + transitionMap);

        NFA dfa = new NFA(sepStates, startState, currentState, sepFStates, sepInputs, transitionMap);

        System.out.println("Press X - to see the states  \n Press Z - to check a string \n Press N - exit the program ");
        while (true)
        {

            Scanner s = new Scanner(System.in);
            String uInput = s.nextLine();
            if(uInput.equals("N"))break;
            switch (uInput)
            {
                case "X":
                    dfa.PrintAutomaton();
                    break;
                case "Z":
                    {
                        System.out.println("Press W - brake");
                        while (true)
                        {
                            System.out.println("Type in a string  to check it: ");
                            Scanner u = new Scanner(System.in);
                            String inpValue = u.nextLine();
                            //var inpValue = System.console().readLine();
                            if (inpValue.equals("W")) break;
                            System.out.println(dfa.Verification(inpValue));

                        }
                        break;
                    }
                case "Q":
                {
                    dfa.ConvertNFA();
                    break;
                }
            }

        }



    }
}