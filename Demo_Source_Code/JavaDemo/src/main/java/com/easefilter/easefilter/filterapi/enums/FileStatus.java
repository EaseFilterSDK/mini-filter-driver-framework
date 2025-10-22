package com.easefilter.easefilter.filterapi.enums;

/**
 * See <a href="https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55"}>Microsoft's documentation.</a>
 */
public enum FileStatus implements NumericEnumULong {
    SUCCESS(0),
    ACCESS_DENIED(0xC0000022),
    REPARSE(0x00000104),
    NO_MORE_FILES(0x80000006),
    WARNING(0x80000000),
    ERROR(0xC0000000),

    ;
    private final int numeric;

    FileStatus(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
