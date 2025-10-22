package com.easefilter.javademo.util;

import com.sun.jna.Memory;
import com.sun.jna.Pointer;
import com.sun.jna.platform.win32.Advapi32;
import com.sun.jna.platform.win32.WinNT;
import com.sun.jna.ptr.IntByReference;
import com.sun.jna.ptr.PointerByReference;

/**
 * Windows user information.
 */
public record AccountData(String username, String domain) {
    public static int MAX_PATH = 260;

    /**
     * Instantiate from Security Identifier.
     *
     * @param sid    Security Identifier in a buffer.
     * @param sidLen Length (bytes) of SID.
     */
    public static AccountData fromSid(byte[] sid, int sidLen) {
        Pointer sidBuf = new Memory(sidLen);
        sidBuf.write(0, sid, 0, sidLen);
        WinNT.PSID psid = new WinNT.PSID(sidBuf);

        IntByReference cchUser = new IntByReference(0);
        IntByReference cchDomain = new IntByReference(0);
        PointerByReference peUse = new PointerByReference();

        // load required buffer sizes into cchUser, cchDomain
        Advapi32.INSTANCE.LookupAccountSid(null, psid, null, cchUser, null, cchDomain, peUse);

        char[] userStr = new char[cchUser.getValue()];
        char[] domainStr = new char[cchDomain.getValue()];

        Advapi32.INSTANCE.LookupAccountSid(null, psid, userStr, cchUser, domainStr, cchDomain, peUse);

        return new AccountData(Utility.strFromArr(userStr, cchUser.getValue() * 2), Utility.strFromArr(domainStr, cchDomain.getValue() * 2));
    }

    public AccountData(String username, String domain) {
        this.username = username;
        this.domain = domain;
    }

    @Override
    public String toString() {
        return String.format("%s/%s", username, domain);
    }
}
