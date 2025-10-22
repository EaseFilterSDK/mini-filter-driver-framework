package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.Utility;
import com.easefilter.easefilter.filterapi.enums.AccessFlag;
import com.easefilter.easefilter.filterapi.enums.BooleanConfig;

import java.util.EnumSet;

/**
 * Parent class for MONITOR/CONTROL/ENCRYPTION rules.
 */
abstract class BaseFileRule extends BaseFilterRule {

    /**
     * File path/mask that this rule applies to.
     * The path serves as a unique identifier for the rule.
     */
    String filePath;
    EnumSet<AccessFlag> accessFlag = EnumSet.of(AccessFlag.ALLOW_MAX_RIGHT_ACCESS);
    EnumSet<BooleanConfig> booleanConfig = EnumSet.of(BooleanConfig.NONE);

    public void uninstall() {
        FilterAPI.INSTANCE.RemoveFilterRule(Utility.toWCharString(this.filePath));
    }
}

