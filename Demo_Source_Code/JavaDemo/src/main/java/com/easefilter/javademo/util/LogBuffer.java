package com.easefilter.javademo.util;

import com.easefilter.easefilter.filterapi.enums.BitFlag;
import com.easefilter.easefilter.filterapi.enums.FileEventType;
import com.easefilter.easefilter.filterapi.enums.FilterCommand;
import com.easefilter.easefilter.filterapi.enums.RegistryCallbackClass;
import com.easefilter.easefilter.filterapi.structs.MESSAGE_SEND_DATA;

import java.nio.charset.StandardCharsets;
import java.util.EnumSet;
import java.util.Optional;

/**
 * Log utility that buffers multiple lines.
 */
public class LogBuffer {
    final private StringBuilder buffer = new StringBuilder();

    /**
     * Formats a string and adds it to the buffer.
     * @param format printf-style format string.
     * @param params Values to substitute into the format string.
     */
    public void appendf(String format, Object... params) {
        buffer.append(String.format(format, params));
    }

    /**
     * Formats a line and adds it to the buffer.
     * @param format printf-style format string.
     * @param params Values to substitute into the format string.
     */
    public void appendfln(String format, Object... params) {
        appendf(format + "\n", params);
    }

    public String toString() {
        String s = buffer.toString();
        buffer.setLength(0);
        return s;
    }

    /**
     * Formats a filter event into the log buffer.
     * @param msg The event.
     */
    public void formatEvent(MESSAGE_SEND_DATA msg) {
        String filename = Utility.strFromArr(msg.FileName, msg.FileNameLength);

        Optional<FilterCommand> eventType = BitFlag.flagFromULong(FilterCommand.class, msg.FilterCommand);

        this.appendfln("EVENT RECEIVED: %s", eventType.map(FilterCommand::toString).orElse("Unknown"));
        this.appendfln("Message ID #%d", msg.MessageId);


        if (eventType.isEmpty()) {
            this.appendfln("Event type: 0x%x", msg.FilterCommand);
            return;
        }

        if (eventType.get() == FilterCommand.FILTER_SEND_FILE_CHANGED_EVENT) {
            this.appendfln("Filename: %s", filename);
            EnumSet<FileEventType> eventTypes = BitFlag.fromNumericULong(FileEventType.class, msg.InfoClass);
            this.appendfln("File access type: %s", eventTypes);
            boolean isCopy = eventTypes.contains(FileEventType.COPIED);
            boolean isRename = eventTypes.contains(FileEventType.RENAMED);
            if ((isCopy || isRename) && msg.DataBufferLength > 0) {
                String untruncated = new String(msg.DataBuffer, StandardCharsets.UTF_16LE);
                String newPath = Utility.strFromArr(untruncated.toCharArray(), msg.DataBufferLength);
                if (isCopy) {
                    this.appendfln("Copied from: %s", newPath);
                } else if (isRename) {
                    this.appendfln("Renamed to: %s", newPath);
                }
            }
        } else if (EnumSet.of(
                FilterCommand.FILTER_SEND_DENIED_PROCESS_EVENT,
                FilterCommand.FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT,
                FilterCommand.FILTER_SEND_PROCESS_TERMINATION_INFO,
                FilterCommand.FILTER_SEND_PROCESS_CREATION_INFO,
                FilterCommand.FILTER_SEND_PROCESS_HANDLE_INFO,
                FilterCommand.FILTER_SEND_PRE_TERMINATE_PROCESS_INFO,
                FilterCommand.FILTER_SEND_THREAD_CREATION_INFO,
                FilterCommand.FILTER_SEND_THREAD_HANDLE_INFO,
                FilterCommand.FILTER_SEND_THREAD_TERMINATION_INFO
        ).contains(eventType.get())) {
            this.appendfln("Executable file: %s", filename);
        } else if (eventType.get() == FilterCommand.FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT || eventType.get() == FilterCommand.FILTER_SEND_REG_CALLBACK_INFO) {
            this.appendfln("Registry key: %s", filename);
            this.appendfln("Access type: %s", BitFlag.fromNumericULongLong(RegistryCallbackClass.class, msg.MessageType));
        }

        this.appendfln("User: %s", AccountData.fromSid(msg.Sid, msg.SidLength));
    }
}