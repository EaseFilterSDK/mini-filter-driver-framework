package com.easefilter.easefilter.filterapi.enums;

public enum FilterType implements NumericEnumULong {
    NONE(0),
    /**
     * File I/O control.
     */
    CONTROL(0x01),

    /**
     * Transparent file encryption.
     */
    ENCRYPTION(0x02),

    /**
     * File I/O monitoring
     */
    MONITOR(0x04),

    /**
     * Registry I/O monitoring & control.
     */
    REGISTRY(0x08),

    /**
     * Process/thread monitoring & control.
     */
    PROCESS(0x10),

    /**
     * Hierarchical storage management.
     */
    HSM(0x40),

    /**
     * Cloud storage.
     */
    CLOUD(0x80),
    ;
    private final int numeric;

    FilterType(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
