package com.easefilter.easefilter.filterapi.enums;

/**
 * Access control for a filter rule (REGISTRY mode).
 */
public enum RegistryControlFlag implements NumericEnumULong {
    OPEN_KEY(0x00000001),
    CREATE_KEY(0x00000002),
    QUERY_KEY(0x00000004),
    RENAME_KEY(0x00000008),
    DELETE_KEY(0x00000010),
    SET_VALUE_KEY_INFORMATION(0x00000020),
    SET_INFORMATION_KEY(0x00000040),
    ENUMERATE_KEY(0x00000080),
    QUERY_VALUE_KEY(0x00000100),
    ENUMERATE_VALUE_KEY(0x00000200),
    QUERY_MULTIPLE_VALUE_KEY(0x00000400),
    DELETE_VALUE_KEY(0x00000800),
    QUERY_KEY_SECURITY(0x00001000),
    SET_KEY_SECURITY(0x00002000),
    RESTORE_KEY(0x00004000),
    REPLACE_KEY(0x00008000),
    SAVE_KEY(0x00010000),
    FLUSH_KEY(0x00020000),
    LOAD_KEY(0x00040000),
    UNLOAD_KEY(0x00080000),
    KEY_CLOSE(0x00100000),
    QUERY_KEYNAME(0x00200000),
    MAX_ACCESS_FLAG(0xFFFFFFFF),

    ;

    private final int numeric;

    RegistryControlFlag(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
