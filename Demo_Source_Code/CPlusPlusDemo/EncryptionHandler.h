#ifndef __ENCRYPTION_H__
#define __ENCRYPTION_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

#include "TestData.h"

#ifndef STATUS_FILE_NOT_ENCRYPTED
#define STATUS_FILE_NOT_ENCRYPTED        ((NTSTATUS)0xC0000291L)
#endif

#ifndef STATUS_FILE_ENCRYPTED
#define STATUS_FILE_ENCRYPTED            ((NTSTATUS)0xC0000293L)
#endif

#ifndef STATUS_RWRAW_ENCRYPTED_FILE_NOT_ENCRYPTED
#define STATUS_RWRAW_ENCRYPTED_FILE_NOT_ENCRYPTED ((NTSTATUS)0xC00004A7L)
#endif

#define AES_MAX_TAG_DATA_SIZE 914

BOOL
EncryptionHandler(
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply)
{
	//this is encryption filter rule with boolean config "REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE" enabled.                        
	//the filter driver request the IV and key to open or create the encrypted file.                        

	BOOL retVal = FALSE;

	EncryptEventArgs* encryptEventArgs = new EncryptEventArgs(messageSend);


	//if you don't want to authorize the process to read the encrypted file,you can set the value as below:
	//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
	//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION; 

	if (encryptEventArgs->isNewCreatedFile)
	{
		//this is new created file to request the encryption key and iv,
		//you can set the custom tag data to the header of the encrypted file if you set the below value.							

		//if you want to block the new file creation, return  STATUS_ACCESS_DENIED
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		//if you don't want to encrypt this new file , return STATUS_FILE_NOT_ENCRYPTED	
		//messageReply->ReturnStatus = STATUS_FILE_NOT_ENCRYPTED;

		//if you want to create the file without the encryption, only embed the tag data to the file, 
		//then return STATUS_RWRAW_ENCRYPTED_FILE_NOT_ENCRYPTED.
		//messageReply->ReturnStatus = STATUS_RWRAW_ENCRYPTED_FILE_NOT_ENCRYPTED;

		//if you want to return the encryption key and iv for new created file, then return  STATUS_SUCCESS
		messageReply->ReturnStatus = STATUS_SUCCESS;

		//------Setup the custom DRM data to the header of the encrypted file here------------------------------------------------
		//make sure the tag data length is less than the AES_MAX_TAG_DATA_SIZE;
		//if (AES_MAX_TAG_DATA_SIZE > messageSend->FileNameLength)
		//{
		//	//for new created file, you can set your own custom tag data to the header.
		//	//here we put the file name as the tag data for test purpose.

		//	messageReply->ReplyData.AESData.Data.TagDataLength = messageSend->FileNameLength;
		//	memcpy(messageReply->ReplyData.AESData.Data.TagData, messageSend->FileName, messageSend->FileNameLength);
		//}
		//------END Setup the custom DRM data to the header of the encrypted file here------------------------------------------------

		//or

		//------Setup the embedding DRM data to the header of the encrypted file here------------------------------------------------
		PAES_TAG_CONTROL_DATA pDRMdata = (PAES_TAG_CONTROL_DATA)messageReply->ReplyData.AESData.Data.TagData;
		pDRMdata->VerificationKey = AES_VERIFICATION_KEY;
		
		SYSTEMTIME currentSystemTime;
		FILETIME currentFileTime;
		LARGE_INTEGER currentTime;

		GetSystemTime(&currentSystemTime);
		SystemTimeToFileTime(&currentSystemTime, &currentFileTime);
		currentTime.LowPart = currentFileTime.dwLowDateTime;
		currentTime.HighPart = currentFileTime.dwHighDateTime;
		//setup the creation time of the encrypted file here
		pDRMdata->CreationTime = currentTime.QuadPart;
		
		//to test only, let's set the expire time one day after today
		//currentSystemTime.wDay += 1;
		//SystemTimeToFileTime(&currentSystemTime, &currentFileTime);
		//currentTime.LowPart = currentFileTime.dwLowDateTime;
		//currentTime.HighPart = currentFileTime.dwHighDateTime;
		////if you want to setup the expire time, do it here
		//pDRMdata->AESFlags |= Flags_Enabled_Expire_Time;
		//pDRMdata->ExpireTime = currentTime.QuadPart; //setup the expire time here

		//here is the offset to store the string data.
		ULONG offset = sizeof(AES_TAG_CONTROL_DATA);		

		//if you want to setup the authorized process names, do it here
		//test authorized process names
		WCHAR* authorizedProcessNames =  GetAuthorizedProcess();
		ULONG strLen = wcslen(authorizedProcessNames) * sizeof(WCHAR);
		pDRMdata->AESFlags |= Flags_Enabled_Check_ProcessName;
		pDRMdata->LengthOfIncludeProcessNames = strLen;
		pDRMdata->OffsetOfIncludeProcessNames = offset;
		memcpy((PUCHAR)pDRMdata + offset, authorizedProcessNames, strLen);
		offset += strLen;

		//if you want to setup the unauthorized process names, do it here
		/*WCHAR* unAuthorizedProcessNames = L"cmd.exe;explorer.exe";
		strLen = wcslen(unAuthorizedProcessNames) * sizeof(WCHAR);
		pDRMdata->AESFlags |= Flags_Enabled_Check_ProcessName;
		pDRMdata->LengthOfExcludeProcessNames = strLen;
		pDRMdata->OffsetOfExcludeProcessNames = offset;
		memcpy((PUCHAR)pDRMdata + offset, unAuthorizedProcessNames, strLen);
		offset += strLen;*/

		//if you want to setup the authorized user names, do it here
		/*WCHAR* authorizedUserNames = L"domain/user1;domain/user2";
		strLen = wcslen(authorizedUserNames) * sizeof(WCHAR);
		pDRMdata->AESFlags |= Flags_Enabled_Check_UserName;
		pDRMdata->LengthOfIncludeUserNames = strLen;
		pDRMdata->OffsetOfIncludeUserNames = offset;
		memcpy((PUCHAR)pDRMdata + offset, authorizedUserNames, strLen);
		offset += strLen;*/

		//if you want to setup the unAuthorized user names, do it here
		/*WCHAR* unAuthorizedUserNames = L"domain/user3;domain/user4";
		strLen = wcslen(unAuthorizedUserNames) * sizeof(WCHAR);
		pDRMdata->AESFlags |= Flags_Enabled_Check_UserName;
		pDRMdata->LengthOfExcludeUserNames = strLen;
		pDRMdata->OffsetOfExcludeUserNames = offset;
		memcpy((PUCHAR)pDRMdata + offset, authorizedUserNames, strLen);
		offset += strLen;*/

		//setup the total size of the DRM data
		messageReply->ReplyData.AESData.Data.TagDataLength = offset;

		//------END Setup the embedding DRM data to the header of the encrypted file here------------------------------------------------

		wprintf(L"\nNew created file :%ws is requesting encryption key, iv and tag data, return status:%0x\n", messageSend->FileName, messageReply->ReturnStatus);

	}
	else if (encryptEventArgs->isFileEncrypted)
	{
		//WCHAR subjectName[MAX_PATH];
		//ULONG sizeofSubjectName = MAX_PATH * sizeof(WCHAR);
		//
		////verify if the process was signed with the right certificate.
		//if (!GetSignerInfo((WCHAR *)encryptEventArgs->ProcessName.c_str(), subjectName, &sizeofSubjectName))
		//{
		//	//can't get the signer information of the process.
		//	wprintf(L"\nCan't get signer information for process:%ws\n", encryptEventArgs->ProcessName.c_str());
		//}
		//else
		//{
		//	wprintf(L"\nGet signer information for process:%ws, certificateName:%ws\n", encryptEventArgs->ProcessName.c_str(),subjectName);
		//}

		//opening the existing encrypted file, request the encryption key and iv.

		//here is the custom tag data for the encrypted file which was embedded in the encryption header.
		ULONG tagDataLength = encryptEventArgs->tagDataLength;
		UCHAR* tagData = encryptEventArgs->tagData.data();

		//if you want to block the encrypted file open, return  STATUS_ACCESS_DENIED
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		//if you want to return the raw encrypted data, then return STATUS_FILE_ENCRYPTED, i.e., for backup software or other application requre raw encrypted data.
		//messageReply->ReturnStatus = STATUS_FILE_ENCRYPTED;		

		//if you want to decrypt the file, then return status success, and the encryption key and iv.
		messageReply->ReturnStatus = STATUS_SUCCESS;

		wprintf(L"\nencrypted file :%ws is requesting encryption key, iv,tagData:%ws return status:%0x\n", messageSend->FileName, (WCHAR*)tagData, messageReply->ReturnStatus);

	}
	else
	{
		//opening the existing no encrypted file with tag data.

		//here is the custom tag data for the encrypted file which was embedded in the encryption header.
		ULONG tagDataLength = encryptEventArgs->tagDataLength;
		UCHAR* tagData = encryptEventArgs->tagData.data();

		//if you want to block the encrypted file open, return  STATUS_ACCESS_DENIED
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		//if you want to decrypt the file, then return status success, and the encryption key and iv.
		messageReply->ReturnStatus = STATUS_SUCCESS;

		wprintf(L"NOT encrypted file :%ws is requesting the file open,tagData:%ws return status:%0x\n", messageSend->FileName, (WCHAR*)tagData, messageReply->ReturnStatus);
	}

	//Here we return the default test iv and key to the filter driver, you can replace it with your own iv and key.	
	unsigned char testiv[] = { 0xf0,0xf1,0xf2,0xf3,0xf4,0xf5,0xf6,0xf7,0xf8,0xf9,0xfa,0xfb,0xfc,0xfd,0xfe,0xff };// Initialization vector
	unsigned char testkey[] = { 0x60,0x3d,0xeb,0x10,0x15,0xca,0x71,0xbe,0x2b,0x73,0xae,0xf0,0x85,0x7d,0x77,0x81,0x1f,0x35,0x2c,0x07,0x3b,0x61,0x08,0xd7,0x2d,0x98,0x10,0xa3,0x09,0x14,0xdf,0xf4 };// 32bytes encryption key	

	//if you want to use your own iv for the encrypted file, set the value here, 
	//or set the IVLength to 0, then the unique auto generated iv will be assigned to the file.
	if (testiv && 16 == sizeof(testiv))
	{
		messageReply->ReplyData.AESData.Data.IVLength = 16;
		memcpy(messageReply->ReplyData.AESData.Data.IV, testiv, 16);
	}
	else
	{
		messageReply->ReplyData.AESData.Data.IVLength = 0;
	}

	messageReply->ReplyData.AESData.Data.AccessFlag = ALLOW_MAX_RIGHT_ACCESS;

	//here is the encryption key for the encrypted file, you can set it with your own key.
	messageReply->ReplyData.AESData.Data.EncryptionKeyLength = 32;
	memcpy(messageReply->ReplyData.AESData.Data.EncryptionKey, testkey, 32);

	//the total return size
	messageReply->ReplyData.AESData.SizeOfData = sizeof(messageReply->ReplyData.AESData.Data) + messageReply->ReplyData.AESData.Data.TagDataLength;


	return retVal;
}


#endif