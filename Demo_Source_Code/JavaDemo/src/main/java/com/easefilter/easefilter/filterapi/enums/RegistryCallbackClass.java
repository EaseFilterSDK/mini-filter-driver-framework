package com.easefilter.easefilter.filterapi.enums;

/**
 * REGISTRY mode filter driver callback class.
 * These are events that can be subscribed to.
 */
public enum RegistryCallbackClass implements NumericEnumULongLong {
    Reg_Pre_Delete_Key(0x00000001),
    Reg_Pre_Set_Value_Key(0x00000002),
    Reg_Pre_Delete_Value_Key(0x00000004),
    Reg_Pre_SetInformation_Key(0x00000008),
    Reg_Pre_Rename_Key(0x00000010),
    Reg_Pre_Enumerate_Key(0x00000020),
    Reg_Pre_Enumerate_Value_Key(0x00000040),
    Reg_Pre_Query_Key(0x00000080),
    Reg_Pre_Query_Value_Key(0x00000100),
    Reg_Pre_Query_Multiple_Value_Key(0x00000200),
    Reg_Pre_Create_Key(0x00000400),
    Reg_Post_Create_Key(0x00000800),
    Reg_Pre_Open_Key(0x00001000),
    Reg_Post_Open_Key(0x00002000),
    Reg_Pre_Key_Handle_Close(0x00004000),
    Reg_Post_Delete_Key(0x00008000),
    Reg_Post_Set_Value_Key(0x00010000),
    Reg_Post_Delete_Value_Key(0x00020000),
    Reg_Post_SetInformation_Key(0x00040000),
    Reg_Post_Rename_Key(0x00080000),
    Reg_Post_Enumerate_Key(0x00100000),
    Reg_Post_Enumerate_Value_Key(0x00200000),
    Reg_Post_Query_Key(0x00400000),
    Reg_Post_Query_Value_Key(0x00800000),
    Reg_Post_Query_Multiple_Value_Key(0x01000000),
    Reg_Post_Key_Handle_Close(0x02000000),
    Reg_Pre_Create_KeyEx(0x04000000),
    Reg_Post_Create_KeyEx(0x08000000),
    Reg_Pre_Open_KeyEx(0x10000000),
    Reg_Post_Open_KeyEx(0x20000000),
    Reg_Pre_Flush_Key(0x40000000),
    Reg_Post_Flush_Key(0x80000000L),
    Reg_Pre_Load_Key(0x100000000L),
    Reg_Post_Load_Key(0x200000000L),
    Reg_Pre_UnLoad_Key(0x400000000L),
    Reg_Post_UnLoad_Key(0x800000000L),
    Reg_Pre_Query_Key_Security(0x1000000000L),
    Reg_Post_Query_Key_Security(0x2000000000L),
    Reg_Pre_Set_Key_Security(0x4000000000L),
    Reg_Post_Set_Key_Security(0x8000000000L),
    Reg_Callback_Object_Context_Cleanup(0x10000000000L),
    Reg_Pre_Restore_Key(0x20000000000L),
    Reg_Post_Restore_Key(0x40000000000L),
    Reg_Pre_Save_Key(0x80000000000L),
    Reg_Post_Save_Key(0x100000000000L),
    Reg_Pre_Replace_Key(0x200000000000L),
    Reg_Post_Replace_Key(0x400000000000L),
    Reg_Pre_Query_KeyName(0x800000000000L),
    Reg_Post_Query_KeyName(0x1000000000000L),
    Max_Reg_Callback_Class(0xFFFFFFFFFFFFFFFFL),


    ;

    private final long numeric;

    RegistryCallbackClass(long numeric) {
        this.numeric = numeric;
    }

    public long getNumeric() {
        return numeric;
    }
}
