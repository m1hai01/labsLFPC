package com.company.ex;

import java.sql.Array;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner s = new Scanner(System.in);
        String uInput = s.nextLine();
        Integer n = Integer.valueOf(uInput);


        for (int i = 0; i < n; i++) {

            for (int j = 0; j <= n-i; j++) {
                System.out.print(" ");
            }
            for (int j = 0; j <= i; j++){
                System.out.print("#");
            }
            System.out.println();

        }
    }
}
