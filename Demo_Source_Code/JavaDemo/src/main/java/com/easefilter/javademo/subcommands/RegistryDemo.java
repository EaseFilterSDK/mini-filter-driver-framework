package com.easefilter.javademo.subcommands;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.enums.*;
import com.easefilter.easefilter.rules.BaseFilterRule;
import com.easefilter.easefilter.rules.RegistryProcNameRuleBuilder;
import com.easefilter.javademo.util.AccountData;
import com.easefilter.javademo.util.LogBuffer;
import com.easefilter.javademo.util.Utility;
import picocli.CommandLine;

import java.util.Arrays;
import java.util.EnumSet;
import java.util.concurrent.Callable;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Collectors;


/**
 * REGISTRY mode demo.
 * <br><br>
 * This demo monitors (or controls) all accesses to the Windows Registry.
 */
@CommandLine.Command(name="registry", description="Registry event monitoring/controlling.", mixinStandardHelpOptions = true)
public class RegistryDemo implements Callable<Integer> {
    private static final Logger LOGGER = Logger.getLogger(RegistryDemo.class.getName());

    @CommandLine.Parameters(index = "0", description = "Registry key, or glob (for example *) to monitor events in.", defaultValue = "*")
    private String keyMask;

    @CommandLine.Option(names = {"-b", "--block-perms"}, description = "List of comma separated registry permissions to deny. Pass 'HELP' to get a list of all possible permissions.", defaultValue = "")
    private String blockPermissions;

    public Integer call() {
        if (blockPermissions.equalsIgnoreCase("HELP")) {
            LogBuffer logBuffer = new LogBuffer();
            logBuffer.appendfln("List of registry permissions:");
            for (RegistryControlFlag perm : RegistryControlFlag.values()) {
                logBuffer.appendfln("\t- %s", perm.name());
            }
            LOGGER.info(logBuffer.toString());
            return 0;
        }

        EnumSet<RegistryControlFlag> blockPermissionsSet = Arrays.stream(blockPermissions.split(","))
                .filter(permStr -> !permStr.isEmpty())
                .map(RegistryControlFlag::valueOf)
                .collect(Collectors.toCollection(() -> EnumSet.noneOf(RegistryControlFlag.class)));

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
        filter.setFilterType(EnumSet.of(FilterType.REGISTRY, FilterType.CONTROL));

        EnumSet<RegistryControlFlag> permissions = EnumSet.allOf(RegistryControlFlag.class);
        permissions.removeAll(EnumSet.of(RegistryControlFlag.MAX_ACCESS_FLAG));
        permissions.removeAll(blockPermissionsSet);

        BaseFilterRule rule = new RegistryProcNameRuleBuilder()
                .setKeyMask(keyMask)
                .setProcName("*")                                                   // monitor all processes
                .setUsername("*")                                                   // monitor all usernames
                .setAccessFlag(permissions)                                         // set allowed registry operations
                .setCallbackClass(EnumSet.allOf(RegistryCallbackClass.class))       // monitor all event types
                .createRegistryProcNameRule();
        rule.install();

        LogBuffer logBuffer = new LogBuffer();
        logBuffer.appendfln("CURRENTLY MONITORING IN REGISTRY: %s", keyMask);
        logBuffer.appendfln("BLOCKED PERMISSIONS: %s", blockPermissionsSet);

        LOGGER.log(Level.INFO, logBuffer.toString());
        Utility.waitForUser();
        filter.stopFilter();

        return 0;
    }
}