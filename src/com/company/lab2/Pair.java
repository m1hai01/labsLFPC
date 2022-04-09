package com.company.lab2;

import java.util.Objects;

public class Pair {
    private String key;
    private String value;

    public Pair(String key, String value) {
        this.key = key;
        this.value = value;
    }

    public String getKey() {
        return key;
    }

    public void setKey(String key) {
        this.key = key;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    public boolean equals(Object other) {
        if (other instanceof Pair) {
            Pair otherPair = (Pair) other;
            return
                    ((  this.key == otherPair.key ||
                            ( this.key != null && otherPair.key != null &&
                                    this.key.equals(otherPair.key))) &&
                            (  this.value == otherPair.value ||
                                    ( this.value != null && otherPair.value != null &&
                                            this.value.equals(otherPair.value))) );
        }

        return false;
    }

    public int hashCode() {
        int hashFirst = key != null ? key.hashCode() : 0;
        int hashSecond = value != null ? value.hashCode() : 0;

        return (hashFirst + hashSecond) * hashSecond + hashFirst;
    }
}
