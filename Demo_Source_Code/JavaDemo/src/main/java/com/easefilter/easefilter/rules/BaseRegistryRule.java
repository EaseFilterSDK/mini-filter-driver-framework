package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.enums.BitFlag;
import com.easefilter.easefilter.filterapi.enums.RegistryCallbackClass;
import com.easefilter.easefilter.filterapi.enums.RegistryControlFlag;

import java.util.EnumSet;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * Base class for REGISTRY mode rules.
 */
abstract class BaseRegistryRule extends BaseFilterRule {

    /**
     * Registry key path/mask that this rule applies to (may be a glob).
     * The key serves as a unique identifier for the rule.
     */
    String keyMask;
    /**
     * Permission flags.
     */
    EnumSet<RegistryControlFlag> accessFlag;
    /**
     * Events to be notified for.
     */
    EnumSet<RegistryCallbackClass> callbackClass;

    int procId;
    String procName;
    String username;

    public BaseRegistryRule(String keyMask, EnumSet<RegistryControlFlag> accessFlag, EnumSet<RegistryCallbackClass> callbackClass, int procId, String procName, String username, boolean excludeFilter) {
        this.keyMask = keyMask;
        this.accessFlag = accessFlag;
        this.callbackClass = callbackClass;
        this.procId = procId;
        this.procName = procName;
        this.username = username;
        this.excludeFilter = excludeFilter;
    }

    /**
     * If true, this filter <i>prevents</i> matching events from being filtered.
     */
    boolean excludeFilter;

    public void install() {
        ruleId = EaseFilter.getInstance().getSerialId();
        EaseFilter.handleError(FilterAPI.INSTANCE.AddRegistryFilterRule(
                procName.length() * 2,
                procName.toCharArray(),
                procId,
                username.length() * 2,
                username.toCharArray(),
                keyMask.length() * 2,
                keyMask.toCharArray(),
                BitFlag.toNumericULong(accessFlag),
                BitFlag.toNumericULongLong(callbackClass),
                excludeFilter,
                ruleId
        ));
    }
}

