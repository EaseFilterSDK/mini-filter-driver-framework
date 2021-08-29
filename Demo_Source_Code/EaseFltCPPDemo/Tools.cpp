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

#include "stdafx.h"
#include "Tools.h"

#define	MAX_ERROR_MESSAGE_SIZE	1024

using namespace std;

void ChangeColour(WORD theColour)
{
	    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);  // Get handle to standard output
	    SetConsoleTextAttribute(hConsole,theColour);		// set the text attribute of the previous handle
}

void
PrintMessage(WCHAR* message,WORD theColour)
{
	ChangeColour(theColour);
	wprintf(message);
	ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);
}

void
PrintPassedMessage(WCHAR* message)
{
	PrintMessage(message,FOREGROUND_GREEN);
}

void
PrintFailedMessage(WCHAR* message)
{
	PrintMessage(message,FOREGROUND_RED);
}


//
//Get the last error message returned from FilterAPI, and print it.
//
void
PrintLastErrorMessage(WCHAR* message)
{
	ULONG messageLength = 0;
	WCHAR* buffer = NULL;

	if( !GetLastErrorMessage(buffer,&messageLength ))
	{
		buffer = (WCHAR*)malloc(messageLength);

		GetLastErrorMessage(buffer,&messageLength );

	}

	PrintFailedMessage(message);
	PrintFailedMessage(buffer);
	printf("\n");

	free(buffer);
}

//
//To display message in WinDbg or DbgView application.
//
void
ToDebugger(
    const WCHAR*	pszFormat, 
    ...)
{
	
	va_list					arglist;
	WCHAR					pBuffer[MAX_ERROR_MESSAGE_SIZE];
	
	memset( pBuffer, 0,MAX_ERROR_MESSAGE_SIZE*sizeof(WCHAR) );

	va_start(arglist, pszFormat);
	vswprintf(pBuffer,MAX_ERROR_MESSAGE_SIZE, pszFormat, arglist);
	va_end(arglist);

	OutputDebugStringW(pBuffer);
	OutputDebugStringW(L"\n");

}

void 
PrintErrorMessage( 
	LPWSTR	message,
    DWORD   errorCode )
{
   LPVOID lpMsgBuf = NULL;   	
   WCHAR   errorMessage[MAX_ERROR_MESSAGE_SIZE];

   __try
   {
	
	   if( errorCode != 0 )
	   {
	    FormatMessage( FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
					    NULL, errorCode, 
					    MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
					    (LPTSTR) &lpMsgBuf, 0, NULL );

		swprintf_s( errorMessage, MAX_ERROR_MESSAGE_SIZE, L"%ws,errorCode:0x%0x,%ws\n", message,errorCode,lpMsgBuf);        

	   }
	   else
	   {
			swprintf_s( errorMessage, MAX_ERROR_MESSAGE_SIZE, L"%ws\n", message);        
	   }

	   ChangeColour(FOREGROUND_RED);
	   wprintf(errorMessage);
	   OutputDebugStringW (errorMessage);	   
	   ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);

   }
   __except( EXCEPTION_EXECUTE_HANDLER  )
	{         
		OutputDebugStringW(L"PrintErrorMessage exception.");      
	}

    if( lpMsgBuf )
	{
		LocalFree( lpMsgBuf );
	}
}