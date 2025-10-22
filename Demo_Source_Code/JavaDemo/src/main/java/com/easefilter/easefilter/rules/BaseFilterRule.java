package com.easefilter.easefilter.rules;

/**
 * Base class for the different rule types.
 */
public abstract class BaseFilterRule {
    /**
     * Serial ID of the rule.
     */
    protected int ruleId;

    /**
     * Apply the rule (i.e. send it to the driver).
     */
    public abstract void install();

    /**
     * Remove the rule from the driver.
     */
    public abstract void uninstall();

    public int getRuleId() {
        return this.ruleId;
    }
}
