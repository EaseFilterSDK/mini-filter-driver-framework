///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies Inc.
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
///////////////////////////////////////////////////////////////////////////////

#pragma once

#include "FilterAPI.h"
#include "WinDataStructures.h"

void ChangeColour(WORD theColour);

void
PrintPassedMessage(WCHAR* message);

void
PrintFailedMessage(WCHAR* message);

void
PrintMessage(WCHAR* message,WORD theColour);

void
PrintLastErrorMessage(WCHAR* message);
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


