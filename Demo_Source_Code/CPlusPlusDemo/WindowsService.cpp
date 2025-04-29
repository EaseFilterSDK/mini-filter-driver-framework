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
#include "WindowsService.h"
#include "FilterControl.h"
#include "FilterRule.h"

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

	FilterControl* filterControl = FilterControl::GetSingleInstance();

    //  Periodically check if the service has been requested to stop
    while (WaitForSingleObject(g_ServiceStopEvent, 0) != WAIT_OBJECT_0)
    {        
            BOOL	ret = FALSE;
			DWORD	threadCount = 5;
			DWORD	connectionTimeout = 20;
			ULONG	filterType = FILE_SYSTEM_MONITOR;

			ULONG allPostIO = POST_CREATE|POST_FASTIO_READ|POST_CACHE_READ|POST_NOCACHE_READ|POST_PAGING_IO_READ;
				allPostIO |= POST_FASTIO_WRITE|POST_CACHE_WRITE|POST_NOCACHE_WRITE|POST_PAGING_IO_WRITE|POST_QUERY_INFORMATION;
				allPostIO |= POST_SET_INFORMATION|POST_DIRECTORY|POST_QUERY_SECURITY|POST_SET_SECURITY|POST_CLEANUP|POST_CLOSE;

			WCHAR* fileFilterMask = L"c:\\test\\*";
			ULONG ioCallbackClass = allPostIO;
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;		

			FileFilterRule fileFilterRule(fileFilterMask);
			fileFilterRule.AccessFlag = accessFlag;
			fileFilterRule.FileChangeEventFilter = FILE_WAS_CREATED|FILE_WAS_WRITTEN|FILE_WAS_RENAMED|FILE_WAS_DELETED;
			fileFilterRule.MonitorFileIOEventFilter = ioCallbackClass;

			filterControl->AddFileFilter(fileFilterRule);
			filterControl->StartFilter(filterType,threadCount,connectionTimeout,registerKey);

			//prevent the current process from being terminated.
			//AddProtectedProcessId(GetCurrentProcessId());
			
			system("pause");			

			//the process can be terminated now.
			//RemoveProtectedProcessId(GetCurrentProcessId());
			
    }

	filterControl->StopFilter();
	delete filterControl;	

    OutputDebugString(_T("EaseFilter Service: ServiceWorkerThread: Exit"));

    return ERROR_SUCCESS;
}