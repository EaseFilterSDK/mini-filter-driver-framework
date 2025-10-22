package com.easefilter.easefilter.filterapi.enums;

import java.util.EnumSet;
import java.util.Optional;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class BitFlag {
    /**
     * Convert a numeric value to a single enum flag.
     * @param enumType The type of the enum.
     * @param value The numeric value.
     * @return The flag that was matched.
     */
    public static <T extends Enum<T> & NumericEnumULongLong> Optional<T> flagFromULongLong(Class<T> enumType, long value) {
        return Stream.of(enumType.getEnumConstants())
                .filter((flag) -> (flag.getNumeric() == value))
                .findFirst();
    }

    /**
     * Convert a numeric value to a single enum flag.
     * @param enumType The type of the enum.
     * @param value The numeric value.
     * @return The flag that was matched.
     */
    public static <T extends Enum<T> & NumericEnumULong> Optional<T> flagFromULong(Class<T> enumType, int value) {
        return Stream.of(enumType.getEnumConstants())
                .filter((flag) -> (flag.getNumeric() == value))
                .findFirst();
    }

    /**
     * Converts a Java EnumSet bitmask to a numerical representation.
     *
     * @param bitflag The enum set to convert.
     * @param <T>     The type of enum.
     * @return The numerical representation of the mask.
     */
    public static <T extends Enum<T> & NumericEnumULongLong> long toNumericULongLong(EnumSet<T> bitflag) {
        return bitflag.stream().map(T::getNumeric).reduce(0L, (x, y) -> x | y);
    }

    /**
     * Convert a numerical representation to a Java EnumSet bitmask.
     *
     * @param value The numerical value.
     * @param <T>   The type of the enum.
     * @return The corresponding bitmask.
     */
    public static <T extends Enum<T> & NumericEnumULongLong> EnumSet<T> fromNumericULongLong(Class<T> enumType, long value) {
        return Stream.of(enumType.getEnumConstants())
                .filter((flag) -> ((flag.getNumeric() & value) == flag.getNumeric() && flag.getNumeric() != 0))
                .collect(Collectors.toCollection(() -> EnumSet.noneOf(enumType)));
    }

    /**
     * Converts a Java EnumSet bitmask to a numerical representation.
     *
     * @param bitflag The enum set to convert.
     * @param <T>     The type of enum.
     * @return The numerical representation of the mask.
     */
    public static <T extends Enum<T> & NumericEnumULong> int toNumericULong(EnumSet<T> bitflag) {
        return bitflag.stream().map(T::getNumeric).reduce(0, (x, y) -> x | y);
    }

    /**
     * Convert a numerical representation to a Java EnumSet bitmask.
     *
     * @param value The numerical value.
     * @param <T>   The type of the enum.
     * @return The corresponding bitmask.
     */
    public static <T extends Enum<T> & NumericEnumULong> EnumSet<T> fromNumericULong(Class<T> enumType, int value) {
        return Stream.of(enumType.getEnumConstants())
                .filter((flag) -> ((flag.getNumeric() & value) == flag.getNumeric() && flag.getNumeric() != 0))
                .collect(Collectors.toCollection(() -> EnumSet.noneOf(enumType)));
    }

}

