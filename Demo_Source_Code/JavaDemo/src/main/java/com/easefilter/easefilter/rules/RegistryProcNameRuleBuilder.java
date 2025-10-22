package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.filterapi.enums.RegistryCallbackClass;
import com.easefilter.easefilter.filterapi.enums.RegistryControlFlag;

import java.util.EnumSet;

public class RegistryProcNameRuleBuilder {
    private String keyMask = "*";
    private EnumSet<RegistryControlFlag> accessFlag = EnumSet.of(RegistryControlFlag.MAX_ACCESS_FLAG);
    private EnumSet<RegistryCallbackClass> callbackClass = EnumSet.of(RegistryCallbackClass.Max_Reg_Callback_Class);
    private String procName;
    private String username = "*";
    private boolean excludeFilter = false;

    public RegistryProcNameRuleBuilder setKeyMask(String keyMask) {
        this.keyMask = keyMask;
        return this;
    }

    public RegistryProcNameRuleBuilder setAccessFlag(EnumSet<RegistryControlFlag> accessFlag) {
        this.accessFlag = accessFlag;
        return this;
    }

    public RegistryProcNameRuleBuilder setCallbackClass(EnumSet<RegistryCallbackClass> callbackClass) {
        this.callbackClass = callbackClass;
        return this;
    }

    public RegistryProcNameRuleBuilder setProcName(String procName) {
        this.procName = procName;
        return this;
    }

    public RegistryProcNameRuleBuilder setUsername(String username) {
        this.username = username;
        return this;
    }

    public RegistryProcNameRuleBuilder setExcludeFilter(boolean excludeFilter) {
        this.excludeFilter = excludeFilter;
        return this;
    }

    public RegistryProcNameRule createRegistryProcNameRule() {
        return new RegistryProcNameRule(keyMask, accessFlag, callbackClass, procName, username, excludeFilter);
    }
}