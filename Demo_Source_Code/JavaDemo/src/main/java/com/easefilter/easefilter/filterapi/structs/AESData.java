package com.easefilter.easefilter.filterapi.structs;

import com.sun.jna.Structure;

/**
 * Helper type for {@link MESSAGE_REPLY_DATA}.
 */
@Structure.FieldOrder({"SizeOfData", "Data"})
public class AESData extends Structure {
    public int SizeOfData;
    public AESDataInner Data;
}
