package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.enums.RegistryCallbackClass;
import com.easefilter.easefilter.filterapi.enums.RegistryControlFlag;

import java.util.EnumSet;

public class RegistryProcNameRule extends BaseRegistryRule {
    public RegistryProcNameRule(String keyMask, EnumSet<RegistryControlFlag> accessFlag, EnumSet<RegistryCallbackClass> callbackClass, String procName, String username, boolean excludeFilter) {
        super(keyMask, accessFlag, callbackClass, 0, procName, username, excludeFilter);
    }

    public void uninstall() {
        EaseFilter.handleError(FilterAPI.INSTANCE.RemoveRegistryFilterRuleByProcessName(procName.length() * 2, procName.toCharArray()));
    }
}
