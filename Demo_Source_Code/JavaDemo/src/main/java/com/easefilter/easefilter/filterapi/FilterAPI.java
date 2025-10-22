package com.easefilter.easefilter.filterapi;

import com.easefilter.easefilter.filterapi.enums.AccessFlag;
import com.easefilter.easefilter.filterapi.enums.BooleanConfig;
import com.easefilter.easefilter.filterapi.structs.MESSAGE_REPLY_DATA;
import com.easefilter.easefilter.filterapi.structs.MESSAGE_SEND_DATA;
import com.sun.jna.Native;
import com.sun.jna.Pointer;
import com.sun.jna.win32.StdCallLibrary;


/**
 * Native (low-leve) interface to FilterAPI.dll.
 * The interface described here is identical to FilterAPI.h in the C++ demo.
 * This is not an exhaustive port of FilterAPI.h; refer to that file for all available functions.
 */
public interface FilterAPI extends StdCallLibrary {
    FilterAPI INSTANCE = Native.load("FilterAPI", FilterAPI.class);

    /**
     * Install the EaseFilter system service.
     *
     * @return Success (boolean).
     */
    int InstallDriver();

    /**
     * Uninstall the EaseFilter system service.
     *
     * @return Success (boolean).
     */
    int UnInstallDriver();

    /**
     * @return Whether the EaseFilter system service is running.
     */
    boolean IsDriverServiceRunning();

    /**
     * Stop the filter service.
     */
    void Disconnect();

    /**
     * Activate EaseFilter with a license key.
     *
     * @param key The license key.
     * @return Success (boolean).
     */
    int SetRegistrationKey(String key);

    /**
     * Get the last error message from EaseFilter.
     *
     * @param Buffer       Buffer to copy the message to. Use NULL to request a buffer length.
     * @param BufferLength Pointer to an unsigned long, where the requested buffer length will be copied to.
     */
    void GetLastErrorMessage(char[] Buffer, int[] BufferLength);

    /**
     * Reset the filter driver configuration to default values.
     *
     * @return If successful, true.
     */
    boolean ResetConfigData();

    /**
     * Remove a file filter rule from the driver.
     *
     * @param FilterMask The file path of the rule (the path serves as the rule's unique ID).
     */
    void RemoveFilterRule(char[] FilterMask);

    /**
     * Set events to get notified for in a filter rule.
     *
     * @param filterMask File path/mask of the filter rule.
     * @param eventType  Bitmask of events to register for (FileEventType).
     * @return Success (boolean).
     */
    int RegisterFileChangedEventsToFilterRule(char[] filterMask, int eventType);

    /**
     * Set I/O events to monitor in a filter rule.
     *
     * @param filterMask File path/mask of the rule.
     * @param registerIO Bitmask of events to register for (IOCallbackClass).
     * @return Success (boolean).
     */
    int RegisterMonitorIOToFilterRule(char[] filterMask, long registerIO);

    /**
     * Set I/O events to control in a filter rule.
     *
     * @param filterMask File path/mask of the rule.
     * @param registerIO Bitmask of events to register for (IOCallbackClass).
     * @return Success (boolean).
     */
    int RegisterControlIOToFilterRule(char[] filterMask, long registerIO);

    /**
     * Add a new file filter rule to the driver.
     *
     * @param accessFlag   Bitmask ({@link AccessFlag}) for access control rights to match.
     * @param filterMask   File path/mask for this rule (must be unique).
     * @param isResident   Determines if the rule is active at boot time.
     * @param filterRuleId Numerical ID for this rule, that appears in {@link MESSAGE_SEND_DATA#FilterRuleId}.
     * @return Success (boolean).
     */
    boolean AddFileFilterRule(int accessFlag, char[] filterMask, boolean isResident, int filterRuleId);

    /**
     * Apply configuration flags.
     *
     * @param booleanConfig The values, as defined in the enum {@link BooleanConfig}.
     * @return Success (boolean).
     */
    int SetBooleanConfig(int booleanConfig);

    /**
     * Apply configuration flags to a specific file rule.
     *
     * @param filterMask    File path/mask to apply the rule to.
     * @param booleanConfig {@link BooleanConfig} flags.
     * @return Success.
     */
    boolean AddBooleanConfigToFilterRule(char[] filterMask, int booleanConfig);

    /**
     * Encrypt a folder, giving each file its own IV.
     * Encryption information will be prepended to the file in a header.
     * @param filterMask File path of the filter rule.
     * @param encryptionKeyLength Number of bytes in the key.
     * @param encryptionKey The key data.
     * @return True if successful.
     */
    boolean AddEncryptionKeyToFilterRule(char[] filterMask, int encryptionKeyLength, byte[] encryptionKey);

    /**
     * Apply a new rule for PROCESS mode.
     * @param processNameMaskLength Length (in bytes) of the process mask.
     * @param processNameMask Executable file path this rule applies to (may be a glob)
     * @param controlFlag Rules to apply (defined in {@link com.easefilter.easefilter.filterapi.enums.ProcessControlFlag})
     * @param filterRuleId Rule ID, which appears in {@link MESSAGE_SEND_DATA#FilterRuleId}.
     * @return True if successful.
     */
    boolean AddProcessFilterRule(int processNameMaskLength, char[] processNameMask, int controlFlag, int filterRuleId);

    /**
     * Remove a rule for PROCESS mode.
     * @param processNameMaskLength Length (in bytes) of the process mask.
     * @param processNameMask The processNameMask of the rule.
     * @return True if successful.
     */
    boolean RemoveProcessFilterRule(int processNameMaskLength, char[] processNameMask);

    /**
     * Create a new REGISTRY filter rule.
     * @param processNameLength Length in bytes of processName.
     * @param processName Process to filter, or * for all processes.
     * @param processId Process ID to filter, instead of processName.
     * @param userNameLength Length in bytes of userName
     * @param userName User mask to filter (* for all users).
     * @param keyNameLength Length in bytes of keyName.
     * @param keyName Registry key mask to filter.
     * @param accessFlag Access control flag for the registry.
     * @param regCallbackClass Registry events to listen for.
     * @param isExcludeFilter If true, do not filter events that match this rule.
     * @param filterRuleId Rule ID.
     * @return True if successful.
     */
    boolean AddRegistryFilterRule(int processNameLength, char[] processName, int processId, int userNameLength, char[] userName, int keyNameLength, char[] keyName, int accessFlag, long regCallbackClass, boolean isExcludeFilter, int filterRuleId);

    /**
     * Remove a REGISTRY filter rule that matches on PID.
     * @param processId PID.
     * @return True if successful.
     */
    boolean RemoveRegistryFilterRuleByProcessId(int processId);

    /**
     * Remove a REGISTRY filter rule that matches no process name.
     * @param processNameLength Length in bytes of processName.
     * @param processName Process mask.
     * @return True if successful.
     */
    boolean RemoveRegistryFilterRuleByProcessName(int processNameLength, char[] processName);

    /**
     * Registers callback functions to handle events from EaseFilter.
     *
     * @param ThreadCount        Number of worker threads used for callbacks.
     * @param MessageCallback    Callback used for handling all events coming from the filter.
     * @param DisconnectCallback Callback used for when the filter disconnects.
     * @return True if successful.
     */
    boolean RegisterMessageCallback(int ThreadCount, Proto_Message_Callback MessageCallback, Proto_Disconnect_Callback DisconnectCallback);

    /**
     * Sets access rights for a specific process, within a specific filter rule.
     * @param filterMask File filter mask of the filter rule to affect.
     * @param processName Executable file path (or glob to paths) to affect (e.g. notepad.exe or C:\Windows\*.exe
     * @param accessFlags {@link AccessFlag} options to apply to the process.
     * @param certificateName If non-null, the driver will check that the process was signed with this certificate.
     * @param imageSha256Hash If non-null, will check that the process executable's hash matches.
     * @return True if successful.
     */
    boolean AddProcessRightsToFilterRule(char[] filterMask, char[] processName, int accessFlags, char[] certificateName, char[] imageSha256Hash);

    /**
     * Sets EaseFilter's mode.
     *
     * @param FilterType {@link com.easefilter.easefilter.filterapi.enums.FilterType}
     * @return True if successful.
     */
    boolean SetFilterType(int FilterType);

    interface Proto_Message_Callback extends StdCallCallback {
        /**
         * Handle a single filter event.
         *
         * @param pSendMessage  Pointer to a {@link MESSAGE_SEND_DATA}, contains event information
         * @param pReplyMessage Pointer to a {@link MESSAGE_REPLY_DATA}, which can be modified to reply to the driver
         * @return If successful, true.
         */
        boolean callback(MESSAGE_SEND_DATA pSendMessage, MESSAGE_REPLY_DATA pReplyMessage);
    }

    interface Proto_Disconnect_Callback extends StdCallCallback {
        void callback();
    }
}

