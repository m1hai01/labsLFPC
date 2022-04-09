package com.company.lab3;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws FileNotFoundException {
        File file = new File("C:\\Users\\mustu\\Desktop\\lfpclabs\\src\\com\\company\\lab3\\file.txt");
        Scanner scanner = new Scanner(file);

        String program = new String();
        while (scanner.hasNextLine())
        {
            program += scanner.nextLine();
        }

        var lexer = new Lexer(program);
        var tokens = lexer.Tokenizer();
        for (var token : tokens)
        {
            System.out.println(token.type + " " + token.value);
        }
    }
}
