package com.easefilter.easefilter.filterapi.structs;

import com.sun.jna.Structure;

/**
 * Helper type for {@link MESSAGE_REPLY_DATA}.
 */
@Structure.FieldOrder({"DataBufferLength", "DataBuffer"})
public class Data extends Structure {
    public static int BLOCK_SIZE = 65536;

    public int DataBufferLength;
    public char[] DataBuffer = new char[BLOCK_SIZE];
}
