package com.easefilter.easefilter.filterapi.structs;

import com.sun.jna.Pointer;
import com.sun.jna.Structure;

/**
 * Helper type for {@link MESSAGE_REPLY_DATA}.
 */
@Structure.FieldOrder({"UserNameLength", "UserName"})
public class UserInfo extends Structure {
    public int UserNameLength;
    /**
     * Of type {@code PWCHAR}, i.e. Java {@code char[]}.
     */
    public Pointer UserName;
}
