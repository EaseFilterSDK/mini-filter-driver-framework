package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.filterapi.enums.AccessFlag;
import com.easefilter.easefilter.filterapi.enums.BooleanConfig;

import java.util.EnumSet;

public class EncryptRuleBuilder {
    private EnumSet<BooleanConfig> booleanConfig = EnumSet.noneOf(BooleanConfig.class);
    private EnumSet<AccessFlag> accessFlag = EnumSet.of(AccessFlag.ALLOW_MAX_RIGHT_ACCESS, AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
    private String filePath;
    private byte[] encryptionKey;

    public EncryptRuleBuilder setBooleanConfig(EnumSet<BooleanConfig> booleanConfig) {
        this.booleanConfig = booleanConfig;
        return this;
    }

    public EncryptRuleBuilder setAccessFlag(EnumSet<AccessFlag> accessFlag) {
        this.accessFlag = accessFlag;
        return this;
    }

    public EncryptRuleBuilder setFilePath(String filePath) {
        this.filePath = filePath;
        return this;
    }

    public EncryptRuleBuilder setEncryptionKey(byte[] encryptionKey) {
        this.encryptionKey = encryptionKey;
        return this;
    }

    public EncryptRule createEncryptRule() {
        return new EncryptRule(booleanConfig, accessFlag, filePath, encryptionKey);
    }
}