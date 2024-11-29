#pragma once

#include <string>
#include <vector>
#include "FilterControl.h"
#include "WinDataStructures.h"
#include "FilterAPI.h"

VOID
DisplayFileIOMessage(FileIOEventArgs* fileIOEventArgs);

VOID
DisplayProcessMessage(ProcessEventArgs* processEventArgs);

VOID
DisplayRegistryMessage(RegistryEventArgs* registryEventArgs);