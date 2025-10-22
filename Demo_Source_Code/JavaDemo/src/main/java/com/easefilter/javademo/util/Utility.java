package com.easefilter.javademo.util;

import com.easefilter.easefilter.EaseFilter;
import com.easefilter.javademo.subcommands.MonitorDemo;

import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * Code common to all the demos.
 */
public class Utility {
    private static final Logger LOGGER = Logger.getLogger(Utility.class.getName());

    public static void disconnectHandler() {
        LOGGER.info("Demo code has disconnected from EaseFilter.");
    }

    /**
     * Show the "Press ENTER to stop" message, and wait for a keypress.
     */
    public static void waitForUser() {
        System.err.println("Press ENTER to stop.");
        try {
            System.in.readNBytes(1);
        } catch (IOException e) {
            LOGGER.log(Level.WARNING, "Failed to wait for user input.", e);
        }
    }

    /**
     * Read the license key from the Java resources folder, then activate EaseFilter with it.
     *
     * @throws Exception EaseFilter could not be activated with this license key.
     */
    public static void activateLicense() {
        EaseFilter filter = EaseFilter.getInstance();

        Properties properties = new Properties();
        try (InputStream input = MonitorDemo.class.getClassLoader().getResourceAsStream("config.properties")) {
            if (input == null) {
                throw new IOException("Could not open `config.properties` file.");
            }
            properties.load(input);
            String key = properties.getProperty("config.licensekey");
            if (key.isBlank() || key.equals("XXXXX-XXXXX-XXXXX-XXXXX-XXXXX-XXXXX")) {
                throw new RuntimeException("Missing license key; edit `src/main/resources/config.properties` to set the key.");
            }
            filter.setLicenseKey(key);
        } catch (Exception ex) {
            LOGGER.log(Level.SEVERE, "Failed to read licence key", ex);
        }
    }

    /**
     * Create a demo directory to work in.
     * @return The path to the directory.
     */
    public static Path createDemoDirectory() {
        Path testDir = Paths.get("C:\\easefilterJavaDemo");
        try {
            Files.createDirectories(testDir);
            LOGGER.info(String.format("Created test directory at '%s'\n.", testDir));
        } catch (Exception ex) {
            LOGGER.log(Level.SEVERE, "Failed to create test directory.", ex);
        }
        return testDir;
    }

    /**
     * Construct a Java string from a C null-terminated string.
     * @param arr The C wchar array.
     * @param nBytes The length of the string in bytes (a wchar is 2 bytes)
     * @return A string.
     */
    public static String strFromArr(char[] arr, int nBytes) {
        StringBuilder builder = new StringBuilder();
        builder.append(arr);
        builder.setLength(nBytes/2);
        return builder.toString();
    }
}
