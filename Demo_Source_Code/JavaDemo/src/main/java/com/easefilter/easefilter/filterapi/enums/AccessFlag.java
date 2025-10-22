package com.easefilter.easefilter.filterapi.enums;

import java.util.EnumSet;

/**
 * Access control for a file (CONTROL mode only).
 * *IMPORTANT:* Do not set this to 0 for least access. Instead, set {@link AccessFlag#LEAST_ACCESS_FLAG}.
 */
public enum AccessFlag implements NumericEnumULong {
    /**
     * Skip all I/O on this file (i.e. disable access protection).
     */
    EXCLUDE_FILTER_RULE(0x00000000),

    /**
     * Block file open.
     */
    EXCLUDE_FILE_ACCESS(0x00000001),

    /**
     * Allow reparsing the file to another filename through a reparse mask.
     */
    ENABLE_REPARSE_FILE_OPEN(0x00000002),

    /**
     * Enable the file hiding feature in a directory (needs a hide file mask).
     */
    ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING(0x00000004),

    /**
     * Enable transparent file encryption (requires setting an encryption key).
     */
    ENABLE_FILE_ENCRYPTION_RULE(0x00000008),

    /**
     * Allow file opens to access the file's security information.
     */
    ALLOW_OPEN_WITH_ACCESS_SYSTEM_SECURITY(0x00000010),

    /**
     * Allow file open with read access.
     */
    ALLOW_OPEN_WITH_READ_ACCESS(0x00000020),

    /**
     * Allow file open with write access.
     */
    ALLOW_OPEN_WITH_WRITE_ACCESS(0x00000040),

    /**
     * Allow file open with create/overwrite access.
     */
    ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS(0x00000080),

    /**
     * Allow file open with delete access.
     */
    ALLOW_OPEN_WITH_DELETE_ACCESS(0x00000100),

    /**
     * Allow reading data from file.
     */
    ALLOW_READ_ACCESS(0x00000200),

    /**
     * Allow writing data from file.
     */
    ALLOW_WRITE_ACCESS(0x00000400),

    /**
     * Allow querying the file's information.
     */
    ALLOW_QUERY_INFORMATION_ACCESS(0x00000800),

    /**
     * Allow changing the file's information, e.g. file attributes, file size, filename, and deletion.
     */
    ALLOW_SET_INFORMATION(0x00001000),

    ALLOW_FILE_RENAME(0x00002000),

    ALLOW_FILE_DELETE(0x00004000),

    ALLOW_FILE_SIZE_CHANGE(0x00008000),

    ALLOW_QUERY_SECURITY_ACCESS(0x00010000),

    ALLOW_SET_SECURITY_ACCESS(0x00020000),

    /**
     * Allow listing the directory's contents.
     */
    ALLOW_DIRECTORY_LIST_ACCESS(0x00040000),

    ALLOW_FILE_ACCESS_FROM_NETWORK(0x00080000),

    /**
     * Enable encryption of newly created files (requires an ENCRYPT filter rule).
     */
    ALLOW_ENCRYPT_NEW_FILE(0x00100000),

    /**
     * Allow applications to read encrypted files; if not set, return encrypted (unreadable) data.
     */
    ALLOW_READ_ENCRYPTED_FILES(0x00200000),

    /**
     * Allow applications to create new files after opening a protected file. Disable to prevent copying the file.
     */
    ALLOW_ALL_SAVE_AS(0x00400000),

    /**
     * If {@link AccessFlag#ALLOW_ALL_SAVE_AS} is enabled, prevent copying protected files out of the protected folder.
     */
    ALLOW_COPY_PROTECTED_FILES_OUT(0x00800000),

    ALLOW_FILE_MEMORY_MAPPED(0x01000000),

    DISABLE_ENCRYPT_DATA_ON_READ(0x02000000),

    ALLOW_COPY_PROTECTED_FILES_TO_USB(0x04000000),

    /**
     * Disable all access to this file. Do NOT use 0 as an AccessFlag; use this flag.
     */
    LEAST_ACCESS_FLAG(0xF0000000),

    /**
     * Allow all access to the file.
     */
    ALLOW_MAX_RIGHT_ACCESS(0xFFFFFFF0),
    ;

    private final int numeric;

    AccessFlag(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }

    /**
     * Gets the value {@link AccessFlag#ALLOW_MAX_RIGHT_ACCESS} in the form of an {@link EnumSet} instead of an integer.
     * @return The aforementioned value.
     */
    static public EnumSet<AccessFlag> getMaxAccessSet() {
        EnumSet<AccessFlag> accessFlag = BitFlag.fromNumericULong(AccessFlag.class, AccessFlag.ALLOW_MAX_RIGHT_ACCESS.getNumeric());
        accessFlag.remove(AccessFlag.ALLOW_MAX_RIGHT_ACCESS);
        return accessFlag;
    }
}
