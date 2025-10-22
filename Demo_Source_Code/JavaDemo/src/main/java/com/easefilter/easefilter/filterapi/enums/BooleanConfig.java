package com.easefilter.easefilter.filterapi.enums;

/**
 * Filter configuration flags.
 */
public enum BooleanConfig implements NumericEnumULong {
    NONE(0),

    /**
     * (EaseTag) If true, after opening the reparse point file, won't restore data back for read/write.
     */
    ENABLE_NO_RECALL_FLAG(0x00000001),

    /**
     * When true, disables unloading the filter driver.
     */
    DISABLE_FILTER_UNLOAD_FLAG(0x00000002),

    /**
     * When true, sets the offline attribute for virtual files.
     */
    ENABLE_SET_OFFLINE_FLAG(0x00000004),

    /**
     * When true, uses a default IV to encrypt files in ENCRYPT mode.
     */
    ENABLE_DEFAULT_IV_TAG(0x00000008),

    /**
     * When true, sends message data to a persistent file. Otherwise, send the event to the service immediately.
     */
    ENABLE_ADD_MESSAGE_TO_FILE(0x00000010),

    /**
     * When true, embed encrypted file metadata in the reparse point tag.
     *
     * @since EaseFilter version 5.0
     */
    ENCRYPT_FILE_WITH_REPARSE_POINT_TAG(0x00000020),

    /**
     * @deprecated Use {@link BooleanConfig#REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE} instead.
     */
    @Deprecated
    REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE(0x00000040),

    /**
     * When true, enables control filter rules at boot time.
     */
    ENABLE_PROTECTION_IN_BOOT_TIME(0x00000080),

    /**
     * When true, encryption rules get the encryption key and IV, plus optional tag data from your user-mode callback code instead of storing it in the driver.
     */
    REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE(0x00000100),

    /**
     * When true, sends read/write data to user-mode callback code.
     */
    ENABLE_SEND_DATA_BUFFER(0x00000200),

    /**
     * When true, will re-open files when rehydrating the stub.
     */
    ENABLE_REOPEN_FILE_ON_REHYDRATION(0x00000400),

    /// When true, queues MONITOR mode events in a buffer, then processes them asynchronously.
    ///
    /// This avoids blocking I/O requests, but may result in events being dropped when the buffer is full.
    ENABLE_MONITOR_EVENT_BUFFER(0x00000800),

    /**
     * When true, register events even when they are blocked.
     */
    ENABLE_SEND_DENIED_EVENT(0x00001000),

    /// When true, and write access is disabled, writes return success and write zero data to the file.
    ///
    /// The data written is sent to the user-mode callback code.
    ENABLE_WRITE_WITH_ZERO_DATA_AND_SEND_DATA(0x00002000),

    /// When true, treats portable massive storage as USB.
    ///
    /// This is for the volume control flag for `BLOCK_USB_READ`, `BLOCK_USB_WRITE`.
    DISABLE_REMOVABLE_MEDIA_AS_USB(0x00004000),

    /// When true, blocks moving an encrypted file to a different folder.
    ///
    /// By default, moving a file out of an encrypted folder results in the file being unable to be decrypted.
    DISABLE_RENAME_ENCRYPTED_FILE(0x00008000),

    /**
     * When true, disables file synchronization for file reading in CloudTier.
     */
    DISABLE_FILE_SYNCHRONIZATION(0x00010000),

    /**
     * When true, data protection continues after the service process is stopped.
     */
    ENABLE_PROTECTION_IF_SERVICE_STOPPED(0x00020000),

    /**
     * When true and write encrypt info to cache is enabled, the system thread will write cached data to disk right away.
     */
    ENABLE_SIGNAL_WRITE_ENCRYPT_INFO_EVENT(0x00020000),

    /**
     * Enable this feature to enable {@link AccessFlag#ALLOW_ALL_SAVE_AS} and {@link AccessFlag#ALLOW_COPY_PROTECTED_FILES_OUT}.
     * <br>
     * By default, this flag is disabled, since if the two flags mentioned above are also disabled,
     * any process that reads protected files will be unable to create new files.
     */
    ENABLE_BLOCK_SAVE_AS_FLAG(0x00040000),

    ;

    private final int numeric;

    BooleanConfig(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
