package com.easefilter.javademo.subcommands;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.enums.BitFlag;
import com.easefilter.easefilter.filterapi.enums.FileEventType;
import com.easefilter.easefilter.filterapi.enums.FilterCommand;
import com.easefilter.easefilter.filterapi.enums.FilterType;
import com.easefilter.easefilter.rules.BaseFilterRule;
import com.easefilter.easefilter.rules.FileRuleBuilder;
import com.easefilter.javademo.util.AccountData;
import com.easefilter.javademo.util.LogBuffer;
import com.easefilter.javademo.util.Utility;
import picocli.CommandLine;

import java.nio.charset.StandardCharsets;
import java.util.EnumSet;
import java.util.concurrent.Callable;
import java.util.logging.Logger;


/**
 * File event MONITOR mode demo.
 * <br><br>
 * This demo watches for file change events and logs them to the console in real time.
 */
@CommandLine.Command(name="monitor", description="File monitoring.", mixinStandardHelpOptions = true)
public class MonitorDemo implements Callable<Integer> {
    @CommandLine.Parameters(index = "0", description="File, or glob (for example C:\\*) to monitor events in.")
    private String pathMask;

    private static final Logger LOGGER = Logger.getLogger(MonitorDemo.class.getName());

    @Override public Integer call() {
        EaseFilter filter = EaseFilter.getInstance();
        filter.ensureInstalled();
        Utility.activateLicense();
        filter.resetConfig();

        filter.registerDisconnectHandler(Utility::disconnectHandler);
        filter.registerMsgHandler((msg, reply) -> {
            msg.verifyIntegrity();

            LogBuffer logBuffer = new LogBuffer();
            logBuffer.formatEvent(msg);
            LOGGER.info(logBuffer.toString());

            return true;
        });
        filter.startFilter(20);
        filter.setFilterType(FilterType.MONITOR);

        LOGGER.info(String.format("CURRENTLY MONITORING: %s", pathMask));

        BaseFilterRule rule = new FileRuleBuilder()
                // monitor all simple file events
                .setChangeEventFilter(EnumSet.allOf(FileEventType.class))
                // monitor all files in the test directory
                .setFilePath(pathMask)
                // build rule from above options
                .createFileRule();
        rule.install();

        Utility.waitForUser();

        filter.stopFilter();
        return 0;
    }
}