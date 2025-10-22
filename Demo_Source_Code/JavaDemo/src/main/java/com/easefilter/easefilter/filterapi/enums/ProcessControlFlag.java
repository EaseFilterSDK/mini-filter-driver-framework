package com.easefilter.easefilter.filterapi.enums;

/**
 * Filter configuration flags.
 */
public enum ProcessControlFlag implements NumericEnumULong {
    NONE(0),

    /**
     * Prevent new process creation.
     */
    DENY_NEW_PROCESS_CREATION(0x00000001),
    /**
     * Send a callback request before the process is terminated. You may block process termination.
     */
    PROCESS_PRE_TERMINATION_REQUEST(0x00000002),
    /**
     * Get a notification when a new process is created.
     */
    PROCESS_CREATION_NOTIFICATION(0x00000100),
    /**
     * Get a notification when a process is terminated.
     */
    PROCESS_TERMINATION_NOTIFICATION(0x00000200),
    /**
     * Get a notification for process handle operations, i.e. a process handle is created or duplicated.
     */
    PROCESS_HANDLE_OP_NOTIFICATION(0x00000400),
    /**
     * Get a notification when a thread is created.
     */
    THREAD_CREATION_NOTIFICATION(0x00000800),
    /**
     * Get a notification when a thread is terminated.
     */
    THREAD_TERMINATION_NOTIFICATION(0x00001000),
    /**
     * Get a notification for thread handle operations, i.e. a thread handle is created or duplicated.
     */
    THREAD_HANDLE_OP_NOTIFICATION(0x00002000),

    ;

    private final int numeric;

    ProcessControlFlag(int numeric) {
        this.numeric = numeric;
    }

    public int getNumeric() {
        return numeric;
    }
}
