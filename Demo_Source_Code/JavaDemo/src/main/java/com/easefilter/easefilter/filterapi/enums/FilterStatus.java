package com.easefilter.easefilter.filterapi.enums;

public enum FilterStatus implements NumericEnumULong {
    /**
     * Reply was changed and needs to be processed.
     */
    FILTER_MESSAGE_IS_DIRTY(0x00000001),
    /**
     * Pre-operation was completed by your code.
     */
    FILTER_COMPLETE_PRE_OPERATION(0x00000002),
    /**
     * Reply data contains update.
     */
    FILTER_DATA_BUFFER_IS_UPDATED(0x00000004),
    /**
     * Read block data-buffer returned to filter.
     */
    FILTER_BLOCK_DATA_WAS_RETURNED(0x00000008),
    /**
     * The whole cache file was downloaded.
     */
    FILTER_CACHE_FILE_WAS_RETURNED(0x00000010),
    /**
     * The whole cache file was downloaded, and the stub file needs to be rehydrated.
     */
    FILTER_REHYDRATE_FILE_VIA_CACHE_FILE(0x00000020),

    ;
    private final int numeric;

    FilterStatus(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
