package com.easefilter.javademo.subcommands;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.enums.*;
import com.easefilter.easefilter.rules.BaseFilterRule;
import com.easefilter.easefilter.rules.FileRuleBuilder;
import com.easefilter.javademo.util.AccountData;
import com.easefilter.javademo.util.LogBuffer;
import com.easefilter.javademo.util.Utility;
import picocli.CommandLine;

import java.nio.file.Path;
import java.util.*;
import java.util.concurrent.Callable;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Collectors;


/**
 * File event CONTROL mode demo.
 */
@CommandLine.Command(name="control", description="File event controlling.", mixinStandardHelpOptions = true)
public class ControlDemo implements Callable<Integer> {
    @CommandLine.Parameters(index = "0", description="File, or glob (for example C:\\*) to control events in.")
    private String pathMask;

    static class BlockGroup {
        @CommandLine.Option(names = {"--block-files"}, description = "List of files to block access to.", split = ",")
        private final List<Path> denyList = new ArrayList<>();

        @CommandLine.Option(names = {"--block-perms"}, description = "List of comma separated file permissions to deny. Pass 'HELP' to get a list of all possible permissions.", defaultValue = "")
        private String blockPermissions = "";
    }
    @CommandLine.ArgGroup(exclusive = true)
    BlockGroup blockGroup = new BlockGroup();

    private static final Logger LOGGER = Logger.getLogger(ControlDemo.class.getName());

    LogBuffer logBuffer = new LogBuffer();

    public Integer call() {
        if (blockGroup.blockPermissions.equalsIgnoreCase("HELP")) {
            logBuffer.appendfln("List of file permissions:");
            for (AccessFlag perm : AccessFlag.values()) {
                if (perm.getNumeric() <= 0xf) {
                    continue;
                }
                logBuffer.appendfln("\t- %s", perm.name());
            }
            LOGGER.info(logBuffer.toString());
            return 0;
        }

        EnumSet<AccessFlag> blockPermissionsSet = Arrays.stream(blockGroup.blockPermissions.split(",")).filter(permStr -> !permStr.isEmpty()).map(AccessFlag::valueOf).collect(Collectors.toCollection(() -> EnumSet.noneOf(AccessFlag.class)));

        EaseFilter filter = EaseFilter.getInstance();
        filter.ensureInstalled();
        Utility.activateLicense();
        filter.resetConfig();

        Set<String> denySet = blockGroup.denyList.stream().map(Path::toString).collect(Collectors.toSet());


        filter.registerDisconnectHandler(Utility::disconnectHandler);
        filter.registerMsgHandler((msg, reply) -> {
            msg.verifyIntegrity();

            LogBuffer logBuffer = new LogBuffer();

            String filename = Utility.strFromArr(msg.FileName, msg.FileNameLength);
            logBuffer.formatEvent(msg);

            // if we are not allowed to reply, reply will be null ptr
            if (reply != null) {
                boolean is_denied = denySet.contains(filename);

                if (is_denied) {
                    logBuffer.appendfln("Decided to DENY this event.");
                    // deny message
                    reply.ReturnStatus = FileStatus.ACCESS_DENIED.getNumeric();

                    reply.FilterStatus = BitFlag.toNumericULong(EnumSet.of(
                            // reply struct has been changed
                            FilterStatus.FILTER_MESSAGE_IS_DIRTY,
                            // tell the filter we're done with this operation
                            FilterStatus.FILTER_COMPLETE_PRE_OPERATION
                    ));
                } else {
                    logBuffer.appendfln("Decided to ALLOW this event.");
                }
            }

            LOGGER.info(logBuffer.toString());

            return true;
        });
        filter.startFilter(20);
        filter.setFilterType(FilterType.CONTROL);

        EnumSet<AccessFlag> accessFlag = AccessFlag.getMaxAccessSet();
        accessFlag.removeAll(blockPermissionsSet);

        FileRuleBuilder ruleBuilder = new FileRuleBuilder()
                // monitor all simple file events
                .setChangeEventFilter(EnumSet.allOf(FileEventType.class))
                // apply to all files in the test directory
                .setFilePath(pathMask);

        if (!blockGroup.blockPermissions.isEmpty()) {
            // control events based on simple access flags
            ruleBuilder.setAccessFlag(accessFlag);
        } else {
            // send events to our callback code to decide whether to deny events or not
            ruleBuilder.setControlIOFilter(EnumSet.allOf(IOCallbackClass.class));
        }

        BaseFilterRule rule = ruleBuilder.createFileRule();
        rule.install();

        logBuffer.appendfln("CURRENTLY CONTROLLING: %s", pathMask);
        logBuffer.appendfln( "Blocking access to files: %s", denySet.toString());
        logBuffer.appendfln("Blocked permissions: %s", blockPermissionsSet.toString());
        LOGGER.info(logBuffer.toString());

        Utility.waitForUser();

        filter.stopFilter();
        return 0;
    }
}
