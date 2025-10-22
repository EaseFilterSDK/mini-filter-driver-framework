package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.filterapi.enums.AccessFlag;
import com.easefilter.easefilter.filterapi.enums.BooleanConfig;
import com.easefilter.easefilter.filterapi.enums.FileEventType;
import com.easefilter.easefilter.filterapi.enums.IOCallbackClass;

import java.util.EnumSet;

public class FileRuleBuilder {
    private boolean isResident = false;
    private EnumSet<IOCallbackClass> monitorIOFilter = EnumSet.noneOf(IOCallbackClass.class);
    private EnumSet<IOCallbackClass> controlIOFilter = EnumSet.noneOf(IOCallbackClass.class);
    private EnumSet<FileEventType> changeEventFilter = EnumSet.noneOf(FileEventType.class);
    private EnumSet<BooleanConfig> booleanConfig = EnumSet.noneOf(BooleanConfig.class);
    private EnumSet<AccessFlag> accessFlag = EnumSet.of(AccessFlag.ALLOW_MAX_RIGHT_ACCESS);
    private String filePath;

    public FileRuleBuilder setIsResident(boolean isResident) {
        this.isResident = isResident;
        return this;
    }

    public FileRuleBuilder setMonitorIOFilter(EnumSet<IOCallbackClass> monitorIOFilter) {
        this.monitorIOFilter = monitorIOFilter;
        return this;
    }

    public FileRuleBuilder setControlIOFilter(EnumSet<IOCallbackClass> controlIOFilter) {
        this.controlIOFilter = controlIOFilter;
        return this;
    }

    public FileRuleBuilder setChangeEventFilter(EnumSet<FileEventType> changeEventFilter) {
        this.changeEventFilter = changeEventFilter;
        return this;
    }

    public FileRuleBuilder setBooleanConfig(EnumSet<BooleanConfig> booleanConfig) {
        this.booleanConfig = booleanConfig;
        return this;
    }

    public FileRuleBuilder setAccessFlag(EnumSet<AccessFlag> accessFlag) {
        this.accessFlag = accessFlag;
        return this;
    }

    public FileRuleBuilder setFilePath(String filePath) {
        this.filePath = filePath;
        return this;
    }

    public FileRule createFileRule() {
        return new FileRule(isResident, monitorIOFilter, controlIOFilter, changeEventFilter, booleanConfig, accessFlag, filePath);
    }
}