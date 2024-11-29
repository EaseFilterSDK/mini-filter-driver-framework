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
#include "FilterAPI.h"
#include "FilterMessage.h"

#define PrintMessage	wprintf //ToDebugger

//
//Here displays the I/O information from filter driver for monitor filter driver and control filter driver
//For every I/O callback data, you always can get this information:
//
//The file related information: file name,file size, file attributes, file time.
//The user information who initiated the I/O: user name, user SID.
//The process information which initiated the I/O: process name, process Id, thread Id.
//The I/O result for post I/O requests: sucess code or the error code.

VOID
DisplayFileIOMessage(FileIOEventArgs* fileIOEventArgs)
{

	if( fileIOEventArgs->IoStatus >= STATUS_ERROR )
	{
		ChangeColour(FOREGROUND_RED);
	}
	else if ( fileIOEventArgs->IoStatus >= STATUS_WARNING )
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN);
	}	
	else
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  
	}

	PrintMessage( L"\n\nMessageId:%d  IOName:%ws, return status:0x%0x\nUserName[%ws], ProcessImagePath:%ws,ProcessId:%d, ThreadId:%d\nFileName:%ws\nFileSize:%I64d, FileAttributes:0x%0x\nDescription:%ws\n",
		fileIOEventArgs->MessageId,fileIOEventArgs->EventName.c_str(),fileIOEventArgs->IoStatus,fileIOEventArgs->UserName.c_str(),fileIOEventArgs->ProcessName.c_str(),
		fileIOEventArgs->ProcessId,fileIOEventArgs->ThreadId,fileIOEventArgs->FileName.c_str(), fileIOEventArgs->FileSize,fileIOEventArgs->FileAttributes,fileIOEventArgs->Description.c_str()); 

	ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  

}

VOID
DisplayProcessMessage(ProcessEventArgs* processEventArgs)
{

	if( processEventArgs->IoStatus >= STATUS_ERROR )
	{
		ChangeColour(FOREGROUND_RED);
	}
	else if ( processEventArgs->IoStatus >= STATUS_WARNING )
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN);
	}	
	else
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  
	}

	PrintMessage( L"\n\nMessageId:%d  IOName:%ws, return status:0x%0x\nUserName[%ws], ProcessImagePath:%ws,ProcessId:%d, ThreadId:%d\nDescription:%ws\n",
		processEventArgs->MessageId,processEventArgs->EventName.c_str(),processEventArgs->IoStatus,processEventArgs->UserName.c_str(),processEventArgs->ProcessName.c_str(),
		processEventArgs->ProcessId,processEventArgs->ThreadId,processEventArgs->Description.c_str()); 

	ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  

}

VOID
DisplayRegistryMessage(RegistryEventArgs* registryEventArgs)
{

	if( registryEventArgs->IoStatus >= STATUS_ERROR )
	{
		ChangeColour(FOREGROUND_RED);
	}
	else if ( registryEventArgs->IoStatus >= STATUS_WARNING )
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN);
	}	
	else
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  
	}

	PrintMessage( L"\n\nMessageId:%d  IOName:%ws, return status:0x%0x\nUserName[%ws], ProcessImagePath:%ws,ProcessId:%d, ThreadId:%d\nKeyName:%ws\nDescription:%ws\n",
		registryEventArgs->MessageId,registryEventArgs->EventName.c_str(),registryEventArgs->IoStatus,registryEventArgs->UserName.c_str(),registryEventArgs->ProcessName.c_str(),
		registryEventArgs->ProcessId,registryEventArgs->ThreadId,registryEventArgs->FileName.c_str(),registryEventArgs->Description.c_str()); 

	ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  

}

