package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.enums.RegistryCallbackClass;
import com.easefilter.easefilter.filterapi.enums.RegistryControlFlag;

import java.util.EnumSet;

public class RegistryProcIdRule extends BaseRegistryRule {
    public RegistryProcIdRule(String keyMask, EnumSet<RegistryControlFlag> accessFlag, EnumSet<RegistryCallbackClass> callbackClass, int procId, String username, boolean excludeFilter) {
        super(keyMask, accessFlag, callbackClass, procId, "*", username, excludeFilter);
    }

    public void uninstall() {
        EaseFilter.handleError(FilterAPI.INSTANCE.RemoveRegistryFilterRuleByProcessId(procId));
    }
}
