package com.easefilter.easefilter.filterapi.enums;

public enum FileEventType implements NumericEnumULong {
    NONE(0),
    CREATED(0x00000020),
    WRITTEN(0x00000040),
    RENAMED(0x00000080),
    DELETED(0x00000100),
    SECURITY_CHANGED(0x00000200),
    INFO_CHANGED(0x00000400),
    READ(0x00000800),
    /// File copied event.
    ///
    /// @since Windows 11.
    COPIED(0x00001000),
    ;
    private final int numeric;

    FileEventType(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
