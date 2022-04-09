package com.company.lab3;
import java.util.HashMap;
import java.util.Map;

public class Token {
    public TokenType type;
    public String value;

    public Token(TokenType type, String value)
    {
        this.type = type;
        this.value = value;
    }

    public static Map<String, TokenType> words = new HashMap<>()
    {
        {
            put("true", TokenType.ADEVARAT);
            put("false", TokenType.FALS);
            put("if", TokenType.IF);
            put("else", TokenType.ELSE);
            put("intoarce", TokenType.RETURN);
            put("for", TokenType.FOR);
            put("intreg", TokenType.NR_INTREG);
            put("main", TokenType.PRINCIPAL);
            put("real", TokenType.NR_REAL);
        }
    };

    public static TokenType CheckKeyword(String keyword)
    {
        if(words.containsKey(keyword)) return words.get(keyword);
        return TokenType.NICKNAME;
    }
}
