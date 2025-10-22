package com.easefilter.easefilter.filterapi.structs;

import com.sun.jna.Pointer;
import com.sun.jna.Structure;

/**
 * Helper type for {@link MESSAGE_REPLY_DATA}.
 */
@Structure.FieldOrder({"FileNameLength", "FileName"})
public class FileInfo extends Structure {
    public int FileNameLength;
    /**
     * Of type {@code PWCHAR}, i.e. Java {@code char[]}.
     */
    public Pointer FileName;
}
