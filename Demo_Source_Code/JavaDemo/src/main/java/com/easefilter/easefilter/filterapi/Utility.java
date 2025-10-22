package com.easefilter.easefilter.filterapi;

public class Utility {
    static public char[] toWCharString(String s) {
        char[] buf = new char[s.length() + 1];
        s.getChars(0, s.length(), buf, 0);
        return buf;
    }
}
