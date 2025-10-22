package com.easefilter.easefilter.filterapi.structs;

import com.sun.jna.Structure;

/**
 * Helper type for {@link MESSAGE_REPLY_DATA}.
 */
@Structure.FieldOrder({"AccessFlag", "IVLength", "IV", "EncryptionKeyLength", "EncryptionKey", "TagDataLength", "TagData"})
public class AESDataInner extends Structure {
    public int AccessFlag;
    public int IVLength;
    public char[] IV = new char[16];
    public int EncryptionKeyLength;
    public char[] EncryptionKey = new char[32];
    public int TagDataLength;
    public char[] TagData = new char[1];
}
