///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
///////////////////////////////////////////////////////////////////////////////

#pragma once

#include <strsafe.h>
#include "FilterAPI.h"
#include "WinDataStructures.h"

//the user need to allocate the enough buffer to receive the subject name.
BOOL
GetSignerInfo(
	WCHAR* processName,
	WCHAR* subjectName,
	PULONG sizeofSubjectName);

void ChangeColour(WORD theColour);

void
PrintPassedMessage(WCHAR* message);

void
PrintFailedMessage(WCHAR* message);

void
PrintMessage(WCHAR* message,WORD theColour);

void
PrintLastErrorMessage(WCHAR* message);

BOOL
GetProcessNameByPid(ULONG pid, WCHAR* processName, ULONG processNameLength );

std::wstring
GetFileTimeStr(LONGLONG fileTime );

BOOLEAN
EnableDebugPrivileges();

//
//To display message in WinDbg or DbgView application.
//
void
ToDebugger(
    const WCHAR*	pszFormat, 
    ...);

void 
PrintErrorMessage( 
	LPWSTR	message,
    DWORD   errorCode );




