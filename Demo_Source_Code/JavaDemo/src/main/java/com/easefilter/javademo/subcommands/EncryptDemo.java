package com.easefilter.javademo.subcommands;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.easefilter.filterapi.enums.AccessFlag;
import com.easefilter.easefilter.filterapi.enums.FilterType;
import com.easefilter.easefilter.rules.EncryptRule;
import com.easefilter.easefilter.rules.EncryptRuleBuilder;
import com.easefilter.javademo.util.LogBuffer;
import com.easefilter.javademo.util.Utility;
import picocli.CommandLine;

import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.KeySpec;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Scanner;
import java.util.concurrent.Callable;
import java.util.logging.Level;
import java.util.logging.Logger;


/**
 * ENCRYPT mode demo.
 * <br><br>
 * This demo encrypts a directory with a password.
 */
@CommandLine.Command(name="encrypt", description="Transparent file encryption.", mixinStandardHelpOptions = true)
public class EncryptDemo implements Callable<Integer> {
    @CommandLine.Parameters(index = "0", description="File, or glob (for example C:\\*) to encrypt.")
    private String pathMask;

    @CommandLine.Option(names = {"--allow-proc"}, description = "Whitelist of processes to give decryption access to (comma-separated). If this option is specified, no other processes can decrypt files.", split = ",")
    private final List<String> allowedProcList = new ArrayList<>();

    private static final Logger LOGGER = Logger.getLogger(EncryptDemo.class.getName());

    public Integer call() {
        EaseFilter filter = EaseFilter.getInstance();
        filter.ensureInstalled();
        Utility.activateLicense();
        filter.resetConfig();

        filter.registerDisconnectHandler(Utility::disconnectHandler);
        filter.registerMsgHandler((msg, reply) -> {
            msg.verifyIntegrity();
            return true;
        });
        filter.startFilter(20);

        // Allowing certain processes to decrypt files requires PROCESS mode.
        filter.setFilterType(EnumSet.of(FilterType.ENCRYPTION, FilterType.PROCESS));

        LogBuffer logBuffer = new LogBuffer();

        System.out.println("Enter encryption password:");
        Scanner scan = new Scanner(System.in);
        String password = scan.nextLine();
        byte[] salt = "use-a-random-salt-in-production".getBytes();
        KeySpec spec = new PBEKeySpec(password.toCharArray(), salt, 65536, 256);

        EnumSet<AccessFlag> noDecryptRights = AccessFlag.getMaxAccessSet();
        noDecryptRights.add(AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
        noDecryptRights.remove(AccessFlag.ALLOW_READ_ENCRYPTED_FILES);

        EnumSet<AccessFlag> accessFlag = allowedProcList.isEmpty() ? EnumSet.of(AccessFlag.ALLOW_MAX_RIGHT_ACCESS, AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) : noDecryptRights;

        try {
            SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
            byte[] key = factory.generateSecret(spec).getEncoded();
            EncryptRule rule = new EncryptRuleBuilder()
                    .setFilePath(pathMask)
                    .setEncryptionKey(key)
                    .setAccessFlag(accessFlag)
                    .createEncryptRule();

            rule.install();

            logBuffer.appendfln("CURRENTLY ENCRYPTING: %s", pathMask);

            if (!allowedProcList.isEmpty()) {
                logBuffer.appendfln("Processes allowed to decrypt: %s", allowedProcList);
                for (String proc : allowedProcList) {
                    rule.addProcessRights(proc, EnumSet.of(AccessFlag.ALLOW_MAX_RIGHT_ACCESS));
                }
            }

        } catch (NoSuchAlgorithmException | InvalidKeySpecException e) {
            LOGGER.log(Level.SEVERE, "failed to derive key from password", e);
        }

        LOGGER.info(logBuffer.toString());

        Utility.waitForUser();

        filter.stopFilter();
        return 0;
    }
}