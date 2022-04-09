package com.company.lab1;


import java.util.*;
import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Scanner;

class Task2 {
    public static void main(String[] args) throws FileNotFoundException {

        File file = new File("src/com/company/lab1/test.txt");
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

        //ca sa stiu care state in care se duce
        Map<String,String> stateMap = new HashMap<String,String>();

        //completam Map cu state care vor fi key si ii dam ca valuri q*
        for (int i = 0; i < separationStates.length; i++) {
            stateMap.put(separationStates[i],"q"+ i);
        }

        //in iStatea vom avea toate staturile ,inclusiv si cele finale scrise sub forma de q*
        ArrayList<String> iStates = new ArrayList<String>();
        for (Map.Entry<String, String> entry : stateMap.entrySet()) {
            //String key = entry.getKey();
            String value = entry.getValue();
            iStates.add(value);
            //System.out.println(key  + value + "\n");
        }
        //in imputs punem inputurile din txt fara virgula
        String inputs = a1.get(1);
        String[] separateInputs = inputs.split(",");


        Map<Map<String,String>,String> transitionMap = new HashMap<>();



        for (int i = 2; i < a1.size() ; i++) {
            //completam stringu cu tranzitii
            String transitions = a1.get(i);
            //scpatem semnul din tranzitii
            String[] separateTransitions = transitions.split(">");
             // separate transition e compus din [0][[1.0][1.1]],atribuim anume caracterul mic
            char MyChar = separateTransitions[1].charAt(0);
            //il transformam in string
            String s = Character.toString(MyChar);
           // System.out.println(separateTransitions[0] +" " + separateTransitions[1]);

            //daca e final
            if (separateTransitions[1].length() == 1){
                // in iStates adaugam staturile finale sub forma de  q
                iStates.add("q"+ iStates.size());
                //si le mai adaugam in finalstate
                finalStates.add(iStates.get(iStates.size() - 1));
                HashMap<String,String> temp = new HashMap<>();
                temp.put(stateMap.get(separateTransitions[0]),separateTransitions[1]);
                transitionMap.put(temp,finalStates.get(finalStates.size() - 1));
            }
            //daca nu e final
            else {
                //deja aii atribuim caracterul mare si il transformam in string
                char MyChar1 = separateTransitions[1].charAt(1);
                String s1 = Character.toString(MyChar1);
                //creem un map temporar unde vom pune curentState si input curent state
                HashMap<String, String> temp = new HashMap<>();
                temp.put(stateMap.get(separateTransitions[0]), s);
                //in transMap punem temp si destinationState
                transitionMap.put(temp, stateMap.get(s1));
            }

        }
        //System.out.println("iState:" + iStates);
        //System.out.println("finalState" + finalStates);
        //System.out.println("stateMap" + stateMap);
        System.out.println("transitionMap" + transitionMap);

         DFA dfa =new DFA(iStates,startState,currentState,finalStates,separateInputs,transitionMap);

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
            }

        }



    }
}