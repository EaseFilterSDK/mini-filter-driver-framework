#pragma once

VOID
SetAuthorizedProcess(WCHAR* processNames);

WCHAR*
GetAuthorizedProcess();

VOID
CreateTestFiles(WCHAR* stubFileFolder);