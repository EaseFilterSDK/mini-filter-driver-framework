package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.Utility;
import com.easefilter.easefilter.filterapi.enums.*;

import java.util.EnumSet;
import java.util.logging.Logger;

/**
 * ENCRYPT mode rule
 */
public final class EncryptRule extends BaseFileRule {
    private static final Logger LOGGER = Logger.getLogger(EncryptRule.class.getName());

    /**
     * Encryption key to use, which must be 16, 24, or 32 bytes.
     * <br>
     * If set to null, a key will be requested from your callback code every time a file is opened.
     * In this case, you must enable {@link BooleanConfig#REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE}.
     */
    private byte[] encryptionKey;

    public EncryptRule(EnumSet<BooleanConfig> booleanConfig, EnumSet<AccessFlag> accessFlag, String filePath, byte[] encryptionKey) {
        this.accessFlag = accessFlag;
        this.filePath = filePath;
        this.booleanConfig = booleanConfig;
        this.encryptionKey = encryptionKey;
    }

    public void install() {
        EaseFilter filter = EaseFilter.getInstance();
        this.ruleId = filter.getSerialId();

        char[] filterMask = Utility.toWCharString(this.filePath);

        EaseFilter.handleError(
                FilterAPI.INSTANCE.AddFileFilterRule(
                        BitFlag.toNumericULong(this.accessFlag),
                        filterMask,
                        false, this.ruleId));

        EaseFilter.handleError(
                FilterAPI.INSTANCE.AddBooleanConfigToFilterRule(
                        filterMask,
                        BitFlag.toNumericULong(this.booleanConfig)
                ));

        if (encryptionKey == null && !booleanConfig.contains(BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE)) {
            LOGGER.warning("Encryption key is null, and BooleanConfig option REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE is not set.");
        } else if (encryptionKey != null) {
            int l = encryptionKey.length;
            if (l != 16 && l != 24 && l != 32) {
                throw new RuntimeException(String.format("Invalid encryption key length %d (expected 16, 24, 32 bytes)", l));
            }
            EaseFilter.handleError(FilterAPI.INSTANCE.AddEncryptionKeyToFilterRule(filterMask, encryptionKey.length, encryptionKey));
        }
    }

    /**
     * Wrapper over {@link FilterAPI#AddProcessRightsToFilterRule}.
     * @param processName Process executable path / glob to change rights of.
     * @param accessFlag Rights to assign to process.
     */
    public void addProcessRights(String processName, EnumSet<AccessFlag> accessFlag) {
        EaseFilter.handleError(FilterAPI.INSTANCE.AddProcessRightsToFilterRule(Utility.toWCharString(this.filePath), Utility.toWCharString(processName), BitFlag.toNumericULong(accessFlag), null, null));
    }
}
