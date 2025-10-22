package com.easefilter.easefilter.filterapi.structs;

import com.easefilter.easefilter.rules.BaseFilterRule;
import com.sun.jna.Pointer;
import com.sun.jna.Structure;

/**
 * C struct for driver-to-userspace communication.
 */
@Structure.FieldOrder({"VerificationNumber", "FilterCommand", "MessageId", "FilterRuleId", "RemoteIP", "FileObject", "FsContext", "MessageType", "ProcessId", "ThreadId", "Offset", "Length", "FileSize", "TransactionTime", "CreationTime", "LastAccessTime", "LastWriteTime", "FileAttributes", "DesiredAccess", "Disposition", "ShareAccess", "CreateOptions", "CreateStatus", "InfoClass", "Status", "ReturnLength", "FileNameLength", "FileName", "SidLength", "Sid", "DataBufferLength", "DataBuffer"})
public class MESSAGE_SEND_DATA extends Structure {
    /**
     * Ensure that the struct was correctly transmitted using {@link MESSAGE_SEND_DATA#VerificationNumber}.
     *
     * @throws RuntimeException Verification failed.
     */
    public void verifyIntegrity() {
        if (VerificationNumber != MESSAGE_SEND_VERIFICATION_NUMBER) {
            throw new RuntimeException(String.format("MESSAGE_SEND_DATA integrity check failed; got verification number %s", Integer.toUnsignedString(VerificationNumber)));
        }
    }

    /**
     * The expected value of {@link MESSAGE_SEND_DATA#VerificationNumber}.
     */
    public static int MESSAGE_SEND_VERIFICATION_NUMBER = (int) 0xFF000001L;

    /**
     * Number to ensure this struct was transmitted correctly.
     * It should be equal to {@link MESSAGE_SEND_DATA#MESSAGE_SEND_VERIFICATION_NUMBER}.
     */
    public int VerificationNumber;

    /**
     * Command ID sent by the driver.
     */
    public int FilterCommand;

    /**
     * Sequential message ID.
     */
    public int MessageId;

    /**
     * ID of the rule that triggered this message.
     *
     * @see BaseFilterRule#getRuleId()
     */
    public int FilterRuleId;

    public static final int INET_ADDR_STR_LEN = 22;
    /**
     * IP address of the remote computer that accesses the file by SMB.
     *
     * @since Windows 7 or later.
     */
    public char[] RemoteIP = new char[INET_ADDR_STR_LEN];

    /**
     * Equivalent to file handle. Unique per file stream opened.
     */
    public Pointer FileObject;

    /**
     * Context, unique per file.
     */
    public Pointer FsContext;

    /**
     * File I/O message type registry class.
     */
    public long MessageType;

    /**
     * Process ID associated with the thread that requested the I/O operation.
     */
    public int ProcessId;

    /**
     * Thread ID that requested the I/O operation.
     */
    public int ThreadId;

    /**
     * Read/write offset
     */
    public long Offset;

    /**
     * Read/write length.
     */
    public int Length;

    /**
     * Size of the file affected by the I/O operation.
     */
    public long FileSize;

    /**
     * Transaction time of this request, UTC
     *
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/sysinfo/file-times">File Times</a>
     */
    public long TransactionTime;

    /**
     * Creation time of this request, UTC
     *
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/sysinfo/file-times">File Times</a>
     */
    public long CreationTime;

    /**
     * Last access time of this request, UTC
     *
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/sysinfo/file-times">File Times</a>
     */
    public long LastAccessTime;

    /**
     * Last write time of this request,
     *
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/sysinfo/file-times">File Times</a>
     */
    public long LastWriteTime;

    public int FileAttributes;

    /**
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea">Windows API - CreateFileA</a>
     */
    public int DesiredAccess;

    /**
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea">Windows API - CreateFileA</a>
     */
    public int Disposition;

    /**
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea">Windows API - CreateFileA</a>
     */
    public int ShareAccess;

    /**
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea">Windows API - CreateFileA</a>
     */
    public int CreateOptions;

    /**
     * @see <a href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea">Windows API - CreateFileA</a>
     */
    public int CreateStatus;

    /**
     * Information class for security/directory/information I/O
     */
    public int InfoClass;

    /**
     * @see <a href="https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55">NTSTATUS</a>
     */
    public int Status;

    /**
     * Return I/O (read/write) length.
     */
    public int ReturnLength;

    /**
     * File name length, in bytes.
     */
    public int FileNameLength;

    public static final int MAX_FILE_NAME_LENGTH = 1024;
    /**
     * Name of the file affected by the I/O.
     */
    public char[] FileName = new char[MAX_FILE_NAME_LENGTH];

    /**
     * Security identifier length.
     */
    public int SidLength;

    public static final int MAX_SID_LENGTH = 256;
    /**
     * Security identifier.
     */
    public byte[] Sid = new byte[MAX_SID_LENGTH];

    /**
     * Length of data buffer.
     */
    public int DataBufferLength;

    public static final int MAX_BUFFER_SIZE = 16384;
    /**
     * Data for read/write/query info/set info operations.
     */
    public byte[] DataBuffer = new byte[MAX_BUFFER_SIZE];
}
