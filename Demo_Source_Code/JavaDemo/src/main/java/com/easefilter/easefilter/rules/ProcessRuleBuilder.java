package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.filterapi.enums.ProcessControlFlag;

import java.util.EnumSet;

public class ProcessRuleBuilder {
    private String executableMask;
    private EnumSet<ProcessControlFlag> controlFlag = EnumSet.of(ProcessControlFlag.PROCESS_CREATION_NOTIFICATION, ProcessControlFlag.PROCESS_TERMINATION_NOTIFICATION);

    public ProcessRuleBuilder setExecutableMask(String executableMask) {
        this.executableMask = executableMask;
        return this;
    }

    public ProcessRuleBuilder setControlFlag(EnumSet<ProcessControlFlag> controlFlag) {
        this.controlFlag = controlFlag;
        return this;
    }

    public ProcessRule createProcessRule() {
        return new ProcessRule(executableMask, controlFlag);
    }
}