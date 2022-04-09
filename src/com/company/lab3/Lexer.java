package com.company.lab3;

import java.util.ArrayList;
import java.util.List;

public class Lexer {
    int currInpPos;
    int currInpRead;
    String input;
    Character currChar;

    public Lexer(String input) {
        this.input = input;
    }

    public ArrayList<Token> Tokenizer()
    {
        ReadChar(); //initializam caracterii
        var tokens = new ArrayList<Token>();

        Token token;
        do
        {
            token = NewToken();
            tokens.add(token);
        } while (token.type != TokenType.EOF);

        return tokens;
    }

    protected Token NewToken()
    {
        Token token;
        SkipWhitespaces();

        switch (currChar)
        {
            case ',':
                token = new Token(TokenType.VIRGULITA, ",");
                break;

            case ';':
                token = new Token(TokenType.PUNCTVIRGULA, ";");
                break;

            case '[':
                token = new Token(TokenType.PRNT_PTR_ST, "[");
                break;

            case ']':
                token = new Token(TokenType.PRNT_PTR_DR, "]");
                break;

            case '{':
                token = new Token(TokenType.ACOLADA_STANGA, "{");
                break;

            case '}':
                token = new Token(TokenType.ACOLADA_DREAPTA, "}");
                break;

            case '*':
                token = new Token(TokenType.INMULTIRE, "*");
                break;

            case '/':
                token = new Token(TokenType.IMPARTIRE, "/");
                break;

            case '!':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.SEMN_NUEGAL, "!=");
                }
                else token = new Token(TokenType.NU, "!");
                break;

            case '>':
                token = new Token(TokenType.SEMN_MARE, ">");
                break;

            case '<':
                token = new Token(TokenType.SEMN_MIC, "<");
                break;

            case '+':
                if (PeekChar() == '+') //verificam dac urmatorul char e tot +
                {
                    ReadChar(); //ne miscam la al doilea +
                    token = new Token(TokenType.PLUSPLUS, "++");
                }
                else token = new Token(TokenType.PLUS, "+");
                break;

            case '-':
                if (PeekChar() == '-')
                {
                    ReadChar();
                    token = new Token(TokenType.MINUSMINUS, "--");
                }
                else token = new Token(TokenType.MINUS, "-");
                break;

            case '=':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.EGALEGAL, "==");
                }
                else token = new Token(TokenType.SEMN_EGAL, "=");
                break;

            case '\0':
                token = new Token(TokenType.EOF, "");
                break;

            default:
                if (IsLetter()) return ReadWord();
                else if (IsDigit()) return ReadNumber();
                else token = new Token(TokenType.NU_SE_POATE, "");
                break;
        }

        ReadChar();
        return token;
    }

    protected void SkipWhitespaces()
    {
        while (currChar == ' ' || currChar == '\t' || currChar == '\n' || currChar ==  '\r')
        {
            ReadChar();
        }
    }

    protected void ReadChar()
    {
        //vedem daca am ajuns la sfarsit de string
        if (currInpRead >= input.length()) currChar = '\0'; //NUL
        else currChar = input.charAt(currInpRead);
        currInpPos = currInpRead;
        currInpRead++;
    }

    protected String ReadIdentifier()
    {
        //pozitia initiala unde se incepe cuvantul
        var pos = currInpPos;
        while (IsLetter() || IsDigit())
        {
            ReadChar();
        }

        //returnam cuvantul (positia initiala, positia curenta)
        return input.substring(pos, currInpPos);
    }

    protected Token ReadNumber()
    {
        //ca si la identifier, handleuim pentru numere
        int pos = currInpPos;
        while (IsDigit())
        {
            ReadChar();
        }

        if (currChar == '.')
        {
            ReadChar(); //facem skip la punct
            while (IsDigit())
            {
                ReadChar();
            }

            return new Token(TokenType.NR_REAL, input.substring(pos, currInpPos));
        }

        return new Token(TokenType.NR_INTREG, input.substring(pos, currInpPos));
    }

    protected Token ReadWord()
    {
        var literal = ReadIdentifier();
        var tokenType = Token.CheckKeyword(literal);
        return new Token(tokenType, literal); //facem skip la readChar
    }

    protected char PeekChar()
    {
        //acelasi readChar(), nu schimbam indexii
        if (currInpRead > input.length()) return '\0'; //NUL
        return input.charAt(currInpRead);
    }

    protected boolean IsLetter()
    {
        if(currChar  >= 'a' && currChar <= 'z' || currChar >= 'A' && currChar <= 'Z' || currChar =='_')
        {
            return true;
        }

        return false;
    }

    protected boolean IsDigit()
    {
        if(currChar  >= '0' && currChar <= '9')
        {
            return true;
        }
        return false;
    }
}

