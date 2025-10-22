package com.easefilter.easefilter.filterapi.enums;

/**
 * I/O types that can be intercepted via MONITOR/CONTROL filter.
 * MONITOR can only register events after they occur (post I/O),
 * while CONTROL can register all events (pre and post).
 *
 * @see <a href="https://en.m.wikipedia.org/wiki/I/O_request_packet">Wikipedia page for IRPs</a>
 */
public enum IOCallbackClass implements NumericEnumULongLong {
    NONE(0),
    /**
     * IRP_MJ_CREATE: open file handle
     */
    PRE_CREATE(0x00000001),
    /**
     * IRP_MJ_CREATE: open file handle
     */
    POST_CREATE(0x00000002),

    PRE_NEW_FILE_CREATED(0x0000000100000000L),
    POST_NEW_FILE_CREATED(0x0000000200000000L),

    PRE_FASTIO_READ(0x00000004),
    POST_FASTIO_READ(0x00000008),

    /**
     * IRP_MJ_READ: read data from cache
     */
    PRE_CACHE_READ(0x00000010),
    /**
     * IRP_MJ_READ: read data from cache
     */
    POST_CACHE_READ(0x00000020),

    /**
     * IRP_MJ_READ: read data, bypassing cache manager
     */
    PRE_NOCACHE_READ(0x00000040),
    /**
     * IRP_MJ_READ: read data, bypassing cache manager
     */
    POST_NOCACHE_READ(0x00000080),

    /**
     * IRP_MJ_READ: paging read, cache on-disk data
     */
    PRE_PAGING_IO_READ(0x00000100),
    /**
     * IRP_MJ_READ: paging read, cache on-disk data
     */
    POST_PAGING_IO_READ(0x00000200),

    PRE_FASTIO_WRITE(0x00000400),
    POST_FASTIO_WRITE(0x00000800),

    /**
     * IRP_MJ_WRITE: cache write
     */
    PRE_CACHE_WRITE(0x00001000),
    /**
     * IRP_MJ_WRITE: cache write
     */
    POST_CACHE_WRITE(0x00002000),

    /**
     * IRP_MJ_WRITE: write directly to disk, bypass cache manager
     */
    PRE_NOCACHE_WRITE(0x00004000),
    /**
     * IRP_MJ_WRITE: write directly to disk, bypass cache manager
     */
    POST_NOCACHE_WRITE(0x00008000),

    /**
     * IRP_MJ_WRITE: paging write that moves data from cache to disk
     */
    PRE_PAGING_IO_WRITE(0x00010000),
    /**
     * IRP_MJ_WRITE: paging write that moves data from cache to disk
     */
    POST_PAGING_IO_WRITE(0x00020000),

    /**
     * IRP_QUERY_INFORMATION: this flag registers all queries, and other flags can watch specific queries, e.g. only file size.
     */
    PRE_QUERY_INFORMATION(0x00040000),
    /**
     * IRP_QUERY_INFORMATION: this flag registers all queries, and other flags can watch specific queries, e.g. only file size.
     */
    POST_QUERY_INFORMATION(0x00080000),
    /**
     * IRP_QUERY_INFORMATION
     */
    PRE_QUERY_FILE_SIZE(0x0000000400000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    POST_QUERY_FILE_SIZE(0x0000000800000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    PRE_QUERY_FILE_BASIC_INFO(0x0000001000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    POST_QUERY_FILE_BASIC_INFO(0x0000002000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    PRE_QUERY_FILE_STANDARD_INFO(0x0000004000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    POST_QUERY_FILE_STANDARD_INFO(0x0000008000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    PRE_QUERY_FILE_NETWORK_INFO(0x0000010000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    POST_QUERY_FILE_NETWORK_INFO(0x0000020000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    PRE_QUERY_FILE_ID(0x0000040000000000L),
    /**
     * IRP_QUERY_INFORMATION
     */
    POST_QUERY_FILE_ID(0x0000080000000000L),

    /**
     * IRP_SET_INFORMATION
     */
    PRE_SET_INFORMATION(0x00100000),
    /**
     * IRP_SET_INFORMATION
     */
    POST_SET_INFORMATION(0x00200000),
    /**
     * IRP_SET_INFORMATION
     */
    PRE_SET_FILE_SIZE(0x0000400000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    POST_SET_FILE_SIZE(0x0000800000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    PRE_SET_FILE_BASIC_INFO(0x0001000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    POST_SET_FILE_BASIC_INFO(0x0002000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    PRE_SET_FILE_STANDARD_INFO(0x0004000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    POST_SET_FILE_STANDARD_INFO(0x0008000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    PRE_SET_FILE_NETWORK_INFO(0x0010000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    POST_SET_FILE_NETWORK_INFO(0x0020000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    PRE_RENAME_FILE(0x0040000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    POST_RENAME_FILE(0x0080000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    PRE_DELETE_FILE(0x0100000000000000L),
    /**
     * IRP_SET_INFORMATION
     */
    POST_DELETE_FILE(0x0200000000000000L),

    /**
     * IRP_MJ_DIRECTORY_CONTROL
     */
    PRE_DIRECTORY(0x00400000),
    /**
     * IRP_MJ_DIRECTORY_CONTROL
     */
    POST_DIRECTORY(0x00800000),

    /**
     * IRP_MJ_QUERY_SECURITY
     */
    PRE_QUERY_SECURITY(0x01000000),
    /**
     * IRP_MJ_QUERY_SECURITY
     */
    POST_QUERY_SECURITY(0x02000000),

    /**
     * IRP_MJ_SET_SECURITY
     */
    PRE_SET_SECURITY(0x04000000),
    /**
     * IRP_MJ_QUERY_SECURITY
     */
    POST_SET_SECURITY(0x08000000),

    /**
     * IRP_MJ_CLEANUP
     */
    PRE_CLEANUP(0x10000000),
    /**
     * IRP_MJ_CLEANUP
     */
    POST_CLEANUP(0x20000000),

    /**
     * IRP_MJ_CLOSE
     */
    PRE_CLOSE(0x40000000),
    /**
     * IRP_MJ_CLOSE
     */
    POST_CLOSE(0x80000000L),
    ;
    private final long numeric;

    IOCallbackClass(long numeric) {
        this.numeric = numeric;
    }

    public long getNumeric() {
        return numeric;
    }
}
