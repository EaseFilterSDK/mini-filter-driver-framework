package com.easefilter.javademo;

import com.easefilter.javademo.subcommands.*;
import picocli.CommandLine;

import java.util.concurrent.Callable;

@CommandLine.Command(name = "ef-demo", mixinStandardHelpOptions = true, subcommands = {MonitorDemo.class, RegistryDemo.class, ControlDemo.class, EncryptDemo.class, ProcessDemo.class})
public class Demo {
    public static void main(String... args) {
        CommandLine cmd = new CommandLine(new Demo());
        String envArgs = System.getenv("ARGS");
        if (envArgs != null) {
            String[] envArgsSplit = envArgs.split(";");
            System.exit(cmd.execute(envArgsSplit));
        } else {
            System.exit(cmd.execute(args));
        }
    }
}
