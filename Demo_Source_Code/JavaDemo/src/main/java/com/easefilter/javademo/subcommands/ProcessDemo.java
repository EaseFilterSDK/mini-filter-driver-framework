package com.easefilter.javademo.subcommands;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.enums.FilterCommand;
import com.easefilter.easefilter.filterapi.enums.FilterType;
import com.easefilter.easefilter.filterapi.enums.ProcessControlFlag;
import com.easefilter.easefilter.rules.BaseFilterRule;
import com.easefilter.easefilter.rules.ProcessRuleBuilder;
import com.easefilter.javademo.util.AccountData;
import com.easefilter.javademo.util.LogBuffer;
import com.easefilter.javademo.util.Utility;
import picocli.CommandLine;

import java.util.EnumSet;
import java.util.concurrent.Callable;
import java.util.logging.Level;
import java.util.logging.Logger;


/**
 * PROCESS mode demo.
 * <br><br>
 * This demo monitors when processes are started from executables in System32.
 */
@CommandLine.Command(name="process", description="Process monitoring/controlling.", mixinStandardHelpOptions = true)
public class ProcessDemo implements Callable<Integer> {
    private static final Logger LOGGER = Logger.getLogger(ProcessDemo.class.getName());

    @CommandLine.Parameters(index = "0", description="Path, or glob (for example C:\\Windows\\System32\\*) of the executable file to monitor/control.")
    private String pathMask;

    @CommandLine.Option(names={"--deny"}, description="Block processes from being created.")
    private boolean deny;

    public Integer call() {
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
        filter.setFilterType(EnumSet.of(FilterType.PROCESS, FilterType.CONTROL));

        EnumSet<ProcessControlFlag> controlFlag = EnumSet.of(ProcessControlFlag.PROCESS_CREATION_NOTIFICATION, ProcessControlFlag.PROCESS_TERMINATION_NOTIFICATION);
        if (deny) {
            controlFlag.add(ProcessControlFlag.DENY_NEW_PROCESS_CREATION);
        }

        BaseFilterRule rule = new ProcessRuleBuilder()
                .setControlFlag(controlFlag)
                .setExecutableMask(pathMask)
                .createProcessRule();
        rule.install();

        LOGGER.log(Level.INFO, "CURRENTLY MONITORING: {0}", pathMask);
        LOGGER.log(Level.INFO, "Control mode: {0}", controlFlag);

        Utility.waitForUser();

        filter.stopFilter();
        return 0;
    }
}