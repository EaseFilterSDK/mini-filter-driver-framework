package com.easefilter.easefilter;

import com.easefilter.easefilter.filterapi.FilterAPI;
import com.easefilter.easefilter.filterapi.enums.BitFlag;
import com.easefilter.easefilter.filterapi.enums.FilterType;

import java.util.EnumSet;
import java.util.logging.Logger;


/**
 * High-level logic for EaseFilter.
 */
public final class EaseFilter {
    private static final Logger LOGGER = Logger.getLogger(EaseFilter.class.getName());

    private static EaseFilter INSTANCE;
    /**
     * The next serial rule ID that we can use.
     */
    private int serialId = 0;

    private FilterAPI.Proto_Message_Callback MsgHandler;

    public void registerMsgHandler(FilterAPI.Proto_Message_Callback handler) {
        MsgHandler = handler;
    }

    private FilterAPI.Proto_Disconnect_Callback DisconnectHandler;

    public void registerDisconnectHandler(FilterAPI.Proto_Disconnect_Callback handler) {
        DisconnectHandler = handler;
    }

    /**
     * Get a serial rule ID.
     * @return The serial ID.
     */
    public int getSerialId() {
        return serialId++;
    }

    public static EaseFilter getInstance() {
        if (INSTANCE == null) {
            INSTANCE = new EaseFilter();
        }
        return INSTANCE;
    }

    /**
     * Get an error string from EaseFilter.
     *
     * @return The error.
     */
    public static String getLastError() {
        int[] length = {0};
        FilterAPI.INSTANCE.GetLastErrorMessage(null, length);
        char[] buffer = new char[((int) length[0])];
        FilterAPI.INSTANCE.GetLastErrorMessage(buffer, length);
        return String.copyValueOf(buffer);
    }

    /**
     * Wrap a FilterAPI function and get an error string if it fails.
     *
     * @throws RuntimeException The status code is not a success code.
     */
    public static void handleError(int status) throws RuntimeException {
        if (status != 1) {
            throw new RuntimeException(getLastError());
        }
    }

    /**
     * Boolean version of
     * {@link EaseFilter#handleError}
     */
    public static void handleError(boolean status) throws RuntimeException {
        if (!status) {
            throw new RuntimeException(getLastError());
        }
    }

    /**
     * Install the EaseFilter system service and ensure that the service is running.
     *
     * @throws RuntimeException when the service is not running after installation.
     */
    public void ensureInstalled() throws RuntimeException {
        EaseFilter.handleError(FilterAPI.INSTANCE.UnInstallDriver());
        EaseFilter.handleError(FilterAPI.INSTANCE.InstallDriver());
        if (!FilterAPI.INSTANCE.IsDriverServiceRunning()) {
            throw new RuntimeException("Could not install EaseFilter system service.");
        }
    }

    /**
     * Activate EaseFilter.
     *
     * @param key License key.
     * @throws RuntimeException when the key is invalid.
     */
    public void setLicenseKey(String key) throws RuntimeException {
        if (FilterAPI.INSTANCE.SetRegistrationKey(key) != 1) {
            throw new RuntimeException(getLastError() + " Please edit `src/main/resources/config.properties` and use a valid license key.");
        }
    }

    /**
     * Reset filter driver configuration to default values.
     */
    public void resetConfig() {
        handleError(FilterAPI.INSTANCE.ResetConfigData());
    }

    private boolean filterStarted = false;

    /**
     * Stop the filter.
     */
    public void stopFilter() {
        if (!filterStarted) {
            LOGGER.warning("Attempted to stop the filter when it was already stopped.");
            return;
        }

        filterStarted = false;
        FilterAPI.INSTANCE.Disconnect();
    }

    /**
     * Start the filter, and register callbacks to your code.
     *
     * @param threadCount Number of threads used to run callbacks.
     */
    public void startFilter(int threadCount) {
        if (filterStarted) {
            LOGGER.warning("Attempted to start the filter when it was already started.");
            return;
        }

        LOGGER.info("Starting filter.");

        if (!FilterAPI.INSTANCE.IsDriverServiceRunning()) {
            ensureInstalled();
        }

        if (MsgHandler == null) {
            LOGGER.warning("No event handler has been registered; using a fallback no-op handler");
            MsgHandler = (a, b) -> true;
        }
        if (DisconnectHandler == null) {
            DisconnectHandler = () -> {
            };
        }

        handleError(FilterAPI.INSTANCE.RegisterMessageCallback(threadCount, MsgHandler, DisconnectHandler));
        filterStarted = true;
    }

    /**
     * Set a single mode for EaseFilter.
     * @param filterType The filter type.
     */
    public void setFilterType(FilterType filterType) {
        handleError(FilterAPI.INSTANCE.SetFilterType(filterType.getNumeric()));
    }

    /**
     * Set multiple modes on EaseFilter.
     * @param filterType The set of filter types.
     */
    public void setFilterType(EnumSet<FilterType> filterType) {
        handleError(FilterAPI.INSTANCE.SetFilterType(BitFlag.toNumericULong(filterType)));
    }
}
