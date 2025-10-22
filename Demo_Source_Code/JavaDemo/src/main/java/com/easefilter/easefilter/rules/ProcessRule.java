package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.enums.BitFlag;
import com.easefilter.easefilter.filterapi.enums.ProcessControlFlag;

import java.util.EnumSet;

/**
 * PROCESS mode rule.
 */
public class ProcessRule extends BaseFilterRule {

    /**
     * File path/mask to executables that this rule applies to.
     * The path serves as a unique identifier for the rule.
     */
    String executableMask;

    EnumSet<ProcessControlFlag> controlFlag;

    public ProcessRule(String executableMask, EnumSet<ProcessControlFlag> controlFlag) {
        this.executableMask = executableMask;
        this.controlFlag = controlFlag;
    }

    public void install() {
        ruleId = EaseFilter.getInstance().getSerialId();
        EaseFilter.handleError(FilterAPI.INSTANCE.AddProcessFilterRule(executableMask.length() * 2, executableMask.toCharArray(), BitFlag.toNumericULong(controlFlag), ruleId));
    }

    public void uninstall() {
        EaseFilter.handleError(FilterAPI.INSTANCE.RemoveProcessFilterRule(executableMask.length() * 2, executableMask.toCharArray()));
    }
}

