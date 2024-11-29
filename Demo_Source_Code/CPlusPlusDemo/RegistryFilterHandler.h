#ifndef __REGISTRYFILTER_H__
#define __REGISTRYFILTER_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

VOID
SendRegistryFilterNotification(PMESSAGE_SEND_DATA messageSend)
{
	RegistryEventArgs* registryEventArgs = new RegistryEventArgs(messageSend);
    
	if (messageSend->FilterCommand == FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT)
    {
        registryEventArgs->EventName = L"NotifyRegWasBlocked";
        DisplayRegistryMessage(registryEventArgs);

        return;
    }

    switch (registryEventArgs->RegCallbackClass)
    {

        case Reg_Post_Delete_Key:
            {
                registryEventArgs->EventName = L"NotifyDeleteKey";
                break;
            }

        case Reg_Post_Set_Value_Key:
            {
				registryEventArgs->EventName = L"NotifySetValueKey";
                break;
            }

        case Reg_Post_Delete_Value_Key:
            {
                    registryEventArgs->EventName = L"NotifyDeleteValueKey";

                break;
            }

        case Reg_Post_SetInformation_Key:
            {
                registryEventArgs->EventName = L"NotifySetInformationKey";
                break;
            }

        case Reg_Post_Rename_Key:
            {
                registryEventArgs->EventName = L"NotifyRenameKey";
                break;
            }

        case Reg_Post_Enumerate_Key:
            {
                registryEventArgs->EventName = L"NotifyEnumerateKey";
                break;
            }

        case Reg_Post_Enumerate_Value_Key:
            {
                registryEventArgs->EventName = L"NotifyEnumerateValueKey";
                break;
            }

        case Reg_Post_Query_Key:
            {
                registryEventArgs->EventName = L"NotifyQueryKey";
                break;
            }

        case Reg_Post_Query_Value_Key:
            {
                registryEventArgs->EventName = L"NotifyQueryValueKey";
                break;
            }

        case Reg_Post_Query_Multiple_Value_Key:
            {
                registryEventArgs->EventName = L"NotifyQueryMultipleValueKey";
                break;
            }

        case Reg_Post_Create_Key:
            {
                registryEventArgs->EventName = L"NotifyCreateKey";
                break;
            }

        case Reg_Post_Open_Key:
            {
                registryEventArgs->EventName = L"NotifyOpenKey";
                break;
            }

        case Reg_Post_Key_Handle_Close:
            {
                registryEventArgs->EventName = L"NotifyKeyHandleClose";
                break;
            }

        case Reg_Post_Create_KeyEx:
            {
                registryEventArgs->EventName = L"NotifyCreateKeyEx";
                break;
            }

        case Reg_Post_Open_KeyEx:
            {
                registryEventArgs->EventName = L"NotifyOpenKeyEx";
                break;
            }

        case Reg_Post_Flush_Key:
            {
                registryEventArgs->EventName = L"NotifyFlushKey";
                break;
            }

        case Reg_Post_Load_Key:
            {
                registryEventArgs->EventName = L"NotifyLoadKey";
                break;
            }

        case Reg_Post_UnLoad_Key:
            {
                registryEventArgs->EventName = L"NotifyUnLoadKey";
                break;
            }

        case Reg_Post_Query_Key_Security:
            {
                registryEventArgs->EventName = L"NotifyQueryKeySecurity";
                break;
            }

        case Reg_Post_Set_Key_Security:
            {
                registryEventArgs->EventName = L"NotifySetKeySecurity";
                break;
            }

        case Reg_Post_Restore_Key:
            {
                registryEventArgs->EventName = L"NotifyRestoreKey";
                break;
            }

        case Reg_Post_Save_Key:
            {
                registryEventArgs->EventName = L"NotifySaveKey";
                break;
            }

        case Reg_Post_Replace_Key:
            {
                registryEventArgs->EventName = L"NotifyReplaceKey";
                break;
            }

        case Reg_Post_Query_KeyName:
            {
                registryEventArgs->EventName = L"NotifyQueryKeyName";
                break;
            }

        default: break;
	}

	 DisplayRegistryMessage(registryEventArgs);
}

BOOL
RegistryFilterHandler( 
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply )
{
	BOOL retVal = FALSE;

    RegistryEventArgs* registryEventArgs = new RegistryEventArgs(messageSend);

    switch (registryEventArgs->RegCallbackClass)
    {
        case Reg_Pre_Delete_Key:
            {
               registryEventArgs->EventName = L"OnPreDeleteKey";
                break;
            }

        case Reg_Pre_Set_Value_Key:
            {
               registryEventArgs->EventName = L"OnPreSetValueKey";
               break;
            }

        case Reg_Pre_Delete_Value_Key:
            {
                registryEventArgs->EventName = L"OnPreDeleteValueKey";
                break;
            }

        case Reg_Pre_SetInformation_Key:
            {
                registryEventArgs->EventName = L"OnPreSetInformationKey";
                break;
            }

        case Reg_Pre_Rename_Key:
            {
                registryEventArgs->EventName = L"OnPreRenameKey";
                break;
            }

        case Reg_Pre_Enumerate_Key:
            {
                registryEventArgs->EventName = L"OnPreEnumerateKey";
                break;
            }

        case Reg_Pre_Enumerate_Value_Key:
            {
                registryEventArgs->EventName = L"OnPreEnumerateValueKey";
                break;
            }

        case Reg_Pre_Query_Key:
            {
                registryEventArgs->EventName = L"OnPreQueryKey";
                break;
            }

        case Reg_Pre_Query_Value_Key:
            {
                registryEventArgs->EventName = L"OnPreQueryValueKey";
                break;
            }

        case Reg_Pre_Query_Multiple_Value_Key:
            {
                registryEventArgs->EventName = L"OnPreQueryMultipleValueKey";
                break;
            }

        case Reg_Pre_Create_Key:
            {
                registryEventArgs->EventName = L"OnPreCreateKey";
                break;
            }

        case Reg_Pre_Open_Key:
            {
                registryEventArgs->EventName = L"OnPreOpenKey";
                break;
            }

        case Reg_Pre_Key_Handle_Close:
            {
                registryEventArgs->EventName = L"OnPreKeyHandleClose";
                break;
            }

        case Reg_Pre_Create_KeyEx:
            {
                registryEventArgs->EventName = L"OnPreCreateKeyEx";
                break;
            }

        case Reg_Pre_Open_KeyEx:
            {
                registryEventArgs->EventName = L"OnPreOpenKeyEx";
                break;
            }

        case Reg_Pre_Flush_Key:
            {
               
                registryEventArgs->EventName = L"OnPreFlushKey";
                break;
            }

        case Reg_Pre_Load_Key:
            {
                registryEventArgs->EventName = L"OnPreLoadKey";
                break;
            }

        case Reg_Pre_UnLoad_Key:
            {
                registryEventArgs->EventName = L"OnPreUnLoadKey";
                break;
            }

        case Reg_Pre_Query_Key_Security:
            {
                registryEventArgs->EventName = L"OnPreQueryKeySecurity";
                break;
            }

        case Reg_Pre_Set_Key_Security:
            {
                registryEventArgs->EventName = L"OnPreSetKeySecurity";
                break;
            }

        case Reg_Pre_Restore_Key:
            {
                registryEventArgs->EventName = L"OnPreRestoreKey";
                break;
            }

        case Reg_Pre_Save_Key:
            {
                registryEventArgs->EventName = L"OnPreSaveKey";
                break;
            }

        case Reg_Pre_Replace_Key:
            {
                registryEventArgs->EventName = L"OnPreReplaceKey";
                break;
            }

        case Reg_Pre_Query_KeyName:
            {
                registryEventArgs->EventName = L"OnPreQueryKeyName";
                break;
            }

        default: break;
    }

	messageReply->ReturnStatus = STATUS_SUCCESS;

	//you can block the process creation with below setting.
    //messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
    //messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

	//
	//You can modify the I/O data with below setting.
	//
	//memcpy(messageReply->ReplyData.Data.DataBuffer,your custom data, custom data length);
	//messageReply->ReplyData.Data.DataBufferLength = dataLength;
	//messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;

	DisplayRegistryMessage(registryEventArgs);

	return retVal;
}


#endif