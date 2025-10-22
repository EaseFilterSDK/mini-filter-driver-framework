package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.filterapi.enums.RegistryCallbackClass;
import com.easefilter.easefilter.filterapi.enums.RegistryControlFlag;

import java.util.EnumSet;

public class RegistryProcIdRuleBuilder {

    private String keyMask = "*";
    private EnumSet<RegistryControlFlag> accessFlag = EnumSet.of(RegistryControlFlag.MAX_ACCESS_FLAG);
    private EnumSet<RegistryCallbackClass> callbackClass = EnumSet.of(RegistryCallbackClass.Max_Reg_Callback_Class);
    private int procId;
    private String username = "*";
    private boolean excludeFilter = false;

    public RegistryProcIdRuleBuilder setKeyMask(String keyMask) {
        this.keyMask = keyMask;
        return this;
    }

    public RegistryProcIdRuleBuilder setAccessFlag(EnumSet<RegistryControlFlag> accessFlag) {
        this.accessFlag = accessFlag;
        return this;
    }

    public RegistryProcIdRuleBuilder setCallbackClass(EnumSet<RegistryCallbackClass> callbackClass) {
        this.callbackClass = callbackClass;
        return this;
    }

    public RegistryProcIdRuleBuilder setProcId(int procId) {
        this.procId = procId;
        return this;
    }

    public RegistryProcIdRuleBuilder setUsername(String username) {
        this.username = username;
        return this;
    }

    public RegistryProcIdRuleBuilder setExcludeFilter(boolean excludeFilter) {
        this.excludeFilter = excludeFilter;
        return this;
    }

    public RegistryProcIdRule createRegistryProcIdRule() {
        return new RegistryProcIdRule(keyMask, accessFlag, callbackClass, procId, username, excludeFilter);
    }
}