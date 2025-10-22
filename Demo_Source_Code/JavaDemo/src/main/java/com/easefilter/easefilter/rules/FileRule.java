package com.easefilter.easefilter.rules;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.Utility;
import com.easefilter.easefilter.filterapi.enums.*;

import java.util.EnumSet;
import java.util.logging.Logger;

/**
 * File rule for MONITOR or CONTROL mode.
 */
public final class FileRule extends BaseFileRule {
    /**
     * 'Resident' CONTROL mode rules are always active, even at boot time.
     */
    private boolean isResident = false;

    /**
     * List of I/O events to monitor.
     */
    private EnumSet<IOCallbackClass> monitorIOFilter = EnumSet.of(IOCallbackClass.NONE);

    /**
     * List of I/O events to control.
     */
    private EnumSet<IOCallbackClass> controlIOFilter = EnumSet.of(IOCallbackClass.NONE);

    /**
     * List of file change events to monitor.
     */
    private EnumSet<FileEventType> changeEventFilter = EnumSet.of(FileEventType.NONE);

    public FileRule(boolean isResident, EnumSet<IOCallbackClass> monitorIOFilter, EnumSet<IOCallbackClass> controlIOFilter, EnumSet<FileEventType> changeEventFilter, EnumSet<BooleanConfig> booleanConfig, EnumSet<AccessFlag> accessFlag, String filePath) {
        this.isResident = isResident;
        this.monitorIOFilter = monitorIOFilter;
        this.controlIOFilter = controlIOFilter;
        this.changeEventFilter = changeEventFilter;
        this.booleanConfig = booleanConfig;
        this.accessFlag = accessFlag;
        this.filePath = filePath;
    }

    public void install() {
        EaseFilter filter = EaseFilter.getInstance();
        this.ruleId = filter.getSerialId();

        char[] filterMask = Utility.toWCharString(filePath);

        EaseFilter.handleError(
                FilterAPI.INSTANCE.AddFileFilterRule(
                        BitFlag.toNumericULong(this.accessFlag),
                        filterMask,
                        this.isResident, this.ruleId));

        EaseFilter.handleError(
                FilterAPI.INSTANCE.AddBooleanConfigToFilterRule(
                        filterMask,
                        BitFlag.toNumericULong(this.booleanConfig)
                ));
        EaseFilter.handleError(
                FilterAPI.INSTANCE.RegisterFileChangedEventsToFilterRule(
                        filterMask,
                        BitFlag.toNumericULong(this.changeEventFilter)
                )
        );

        EaseFilter.handleError(
                FilterAPI.INSTANCE.RegisterMonitorIOToFilterRule(
                        filterMask,
                        BitFlag.toNumericULongLong(this.monitorIOFilter)
                )
        );

        EaseFilter.handleError(
                FilterAPI.INSTANCE.RegisterControlIOToFilterRule(
                        filterMask,
                        BitFlag.toNumericULongLong(this.controlIOFilter)
                )
        );
    }
}
