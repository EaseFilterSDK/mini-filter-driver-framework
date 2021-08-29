///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

//

#include "stdafx.h"
#include "Tools.h"
#include "FilterAPI.h"
#include "UnitTest.h"
#include "WindowsService.h"
#include "FilterWorker.h"

#define SERVICE_NAME  _T("EaseFilter Service")

SERVICE_STATUS        g_ServiceStatus = {0};
SERVICE_STATUS_HANDLE g_StatusHandle = NULL;
HANDLE                g_ServiceStopEvent = INVALID_HANDLE_VALUE;


int StartWindowsService()
{
    OutputDebugString(_T("EaseFilter Service: Main: Entry"));

    SERVICE_TABLE_ENTRY ServiceTable[] = 
    {
        {SERVICE_NAME, (LPSERVICE_MAIN_FUNCTION) ServiceMain},
        {NULL, NULL}
    };

    if (StartServiceCtrlDispatcher (ServiceTable) == FALSE)
    {
       OutputDebugString(_T("EaseFilter Service: Main: StartServiceCtrlDispatcher returned error"));
       return GetLastError ();
    }

    OutputDebugString(_T("EaseFilter Service: Main: Exit"));
    return 0;
}


VOID WINAPI ServiceMain (DWORD argc, LPTSTR *argv)
{
    DWORD Status = E_FAIL;

    OutputDebugString(_T("EaseFilter Service: ServiceMain: Entry"));

    g_StatusHandle = RegisterServiceCtrlHandler (SERVICE_NAME, ServiceCtrlHandler);

    if (g_StatusHandle == NULL) 
    {
        OutputDebugString(_T("EaseFilter Service: ServiceMain: RegisterServiceCtrlHandler returned error"));
        goto EXIT;
    }

    // Tell the service controller we are starting
    ZeroMemory (&g_ServiceStatus, sizeof (g_ServiceStatus));
    g_ServiceStatus.dwServiceType = SERVICE_WIN32_OWN_PROCESS;
    g_ServiceStatus.dwControlsAccepted = 0;
    g_ServiceStatus.dwCurrentState = SERVICE_START_PENDING;
    g_ServiceStatus.dwWin32ExitCode = 0;
    g_ServiceStatus.dwServiceSpecificExitCode = 0;
    g_ServiceStatus.dwCheckPoint = 0;

    if (SetServiceStatus (g_StatusHandle, &g_ServiceStatus) == FALSE) 
    {
        OutputDebugString(_T("EaseFilter Service: ServiceMain: SetServiceStatus returned error"));
    }

    /* 
     * Perform tasks neccesary to start the service here
     */
    OutputDebugString(_T("EaseFilter Service: ServiceMain: Performing Service Start Operations"));

    // Create stop event to wait on later.
    g_ServiceStopEvent = CreateEvent (NULL, TRUE, FALSE, NULL);
    if (g_ServiceStopEvent == NULL) 
    {
        OutputDebugString(_T("EaseFilter Service: ServiceMain: CreateEvent(g_ServiceStopEvent) returned error"));

        g_ServiceStatus.dwControlsAccepted = 0;
        g_ServiceStatus.dwCurrentState = SERVICE_STOPPED;
        g_ServiceStatus.dwWin32ExitCode = GetLastError();
        g_ServiceStatus.dwCheckPoint = 1;

        if (SetServiceStatus (g_StatusHandle, &g_ServiceStatus) == FALSE)
	    {
		    OutputDebugString(_T("EaseFilter Service: ServiceMain: SetServiceStatus returned error"));
	    }
        goto EXIT; 
    }    

    // Tell the service controller we are started
    g_ServiceStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP;
    g_ServiceStatus.dwCurrentState = SERVICE_RUNNING;
    g_ServiceStatus.dwWin32ExitCode = 0;
    g_ServiceStatus.dwCheckPoint = 0;

    if (SetServiceStatus (g_StatusHandle, &g_ServiceStatus) == FALSE)
    {
	    OutputDebugString(_T("EaseFilter Service: ServiceMain: SetServiceStatus returned error"));
    }

    // Start the thread that will perform the main task of the service
    HANDLE hThread = CreateThread (NULL, 0, ServiceWorkerThread, NULL, 0, NULL);

    OutputDebugString(_T("EaseFilter Service: ServiceMain: Waiting for Worker Thread to complete"));

    // Wait until our worker thread exits effectively signaling that the service needs to stop
    WaitForSingleObject (hThread, INFINITE);
    
    OutputDebugString(_T("EaseFilter Service: ServiceMain: Worker Thread Stop Event signaled"));
    
    
    /* 
     * Perform any cleanup tasks
     */
    OutputDebugString(_T("EaseFilter Service: ServiceMain: Performing Cleanup Operations"));

    CloseHandle (g_ServiceStopEvent);

    g_ServiceStatus.dwControlsAccepted = 0;
    g_ServiceStatus.dwCurrentState = SERVICE_STOPPED;
    g_ServiceStatus.dwWin32ExitCode = 0;
    g_ServiceStatus.dwCheckPoint = 3;

    if (SetServiceStatus (g_StatusHandle, &g_ServiceStatus) == FALSE)
    {
	    OutputDebugString(_T("EaseFilter Service: ServiceMain: SetServiceStatus returned error"));
    }
    
    EXIT:
    OutputDebugString(_T("EaseFilter Service: ServiceMain: Exit"));

    return;
}


VOID WINAPI ServiceCtrlHandler (DWORD CtrlCode)
{
    OutputDebugString(_T("EaseFilter Service: ServiceCtrlHandler: Entry"));

    switch (CtrlCode) 
	{
     case SERVICE_CONTROL_STOP :

        OutputDebugString(_T("EaseFilter Service: ServiceCtrlHandler: SERVICE_CONTROL_STOP Request"));

        if (g_ServiceStatus.dwCurrentState != SERVICE_RUNNING)
           break;

        /* 
         * Perform tasks neccesary to stop the service here 
         */
        
        g_ServiceStatus.dwControlsAccepted = 0;
        g_ServiceStatus.dwCurrentState = SERVICE_STOP_PENDING;
        g_ServiceStatus.dwWin32ExitCode = 0;
        g_ServiceStatus.dwCheckPoint = 4;

        if (SetServiceStatus (g_StatusHandle, &g_ServiceStatus) == FALSE)
		{
			OutputDebugString(_T("EaseFilter Service: ServiceCtrlHandler: SetServiceStatus returned error"));
		}

        // This will signal the worker thread to start shutting down
        SetEvent (g_ServiceStopEvent);

        break;

     default:
         break;
    }

    OutputDebugString(_T("EaseFilter Service: ServiceCtrlHandler: Exit"));
}


DWORD WINAPI ServiceWorkerThread (LPVOID lpParam)
{
    OutputDebugString(_T("EaseFilter Service: ServiceWorkerThread: Entry"));

    //  Periodically check if the service has been requested to stop
    while (WaitForSingleObject(g_ServiceStopEvent, 0) != WAIT_OBJECT_0)
    {        
            BOOL ret = FALSE;
			WCHAR* filterFolder = GetFilterMask();
			ULONG ioRegistration = 0;
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;		
			DWORD	threadCount = 5;
			ULONG	filterType = FILE_SYSTEM_MONITOR;

			//Register the I/O request,which will be monitored or will be called back from filter.
			for (int i = 0; i < MAX_REQUEST_TYPE; i++ )
			{
				//register all post request
				if( (double)i/2 != i/2 )
				{
					ioRegistration |= 1<<i;
				}
			}     

			ret = SetRegistrationKey(registerKey);
			if( !ret )
			{
				PrintLastErrorMessage( L"SetRegistrationKey failed.");
				return 1;
			}

			ret = RegisterMessageCallback(threadCount,MessageCallback,DisconnectCallback);

			if( !ret )
			{
				PrintLastErrorMessage( L"RegisterMessageCallback failed.");
				return 1;
			}

			//this the demo how to use the control filter.
			SendConfigInfoToFilter(filterType,filterFolder,ioRegistration,accessFlag);		

    }

	Disconnect();
    OutputDebugString(_T("EaseFilter Service: ServiceWorkerThread: Exit"));

    return ERROR_SUCCESS;
}