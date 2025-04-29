#ifndef __PROCESSFILTER_H__
#define __PROCESSFILTER_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

VOID
SendProcessFilterNotification(PMESSAGE_SEND_DATA messageSend)
{
    ProcessEventArgs* processEventArgs = new ProcessEventArgs(messageSend);
    if (messageSend->FilterCommand == FILTER_SEND_DENIED_PROCESS_CREATION_EVENT)
    {
        processEventArgs->EventName = L"ProcessCreationWasBlocked";		
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT)
    {
        processEventArgs->EventName = L"ProcessTerminationWasBlocked";
    }
    else if (messageSend->FilterCommand == FILTER_SEND_PROCESS_TERMINATION_INFO)
    {
        processEventArgs->EventName = L"NotifyProcessTerminated";
    }
    else if (messageSend->FilterCommand == FILTER_SEND_THREAD_CREATION_INFO)
    {
		processEventArgs->EventName = L"NotifyThreadCreation";
    }
    else if (messageSend->FilterCommand == FILTER_SEND_THREAD_TERMINATION_INFO)
    {
        processEventArgs->EventName = L"NotifyThreadTerminated";
    }
    else if (messageSend->FilterCommand == FILTER_SEND_PROCESS_HANDLE_INFO)
    {
        processEventArgs->EventName = L"NotifyProcessHandleInfo";
    }
    else if (messageSend->FilterCommand == FILTER_SEND_THREAD_HANDLE_INFO)
    {
        processEventArgs->EventName = L"NotifyThreadHandleInfo";
    }
    else if (messageSend->FilterCommand == FILTER_SEND_LOAD_IMAGE_NOTIFICATION)
    {
        processEventArgs->EventName = L"NotifyImageWasLoaded";
    }
   
	DisplayProcessMessage(processEventArgs);

}


BOOL
ProcessFilterHandler( 
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply )
{
	BOOL retVal = TRUE;

	ProcessEventArgs* processEventArgs = new ProcessEventArgs(messageSend);

	if (messageSend->MessageType == FILTER_SEND_PROCESS_CREATION_INFO)
    {
        processEventArgs->EventName = L"OnProcessCreation";
		DisplayProcessMessage(processEventArgs);

         //you can block the process creation with below setting.
         //messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
         //messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
    }
    else if (messageSend->MessageType == FILTER_SEND_PRE_TERMINATE_PROCESS_INFO)
    {
		processEventArgs->EventName = L"OnProcessPreTermination";
		DisplayProcessMessage(processEventArgs);

        //you can block the process termination with below setting.
        //messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
        //messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
    }

	return retVal;
}


#endif