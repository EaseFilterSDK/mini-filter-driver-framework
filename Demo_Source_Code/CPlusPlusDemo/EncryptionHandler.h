#ifndef __ENCRYPTION_H__
#define __ENCRYPTION_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

#ifndef STATUS_FILE_NOT_ENCRYPTED
#define STATUS_FILE_NOT_ENCRYPTED        ((NTSTATUS)0xC0000291L)
#endif

#ifndef STATUS_FILE_ENCRYPTED
#define STATUS_FILE_ENCRYPTED            ((NTSTATUS)0xC0000293L)
#endif

#define AES_MAX_TAG_DATA_SIZE 914

unsigned char testiv[] = {0xf0,0xf1,0xf2,0xf3,0xf4,0xf5,0xf6,0xf7,0xf8,0xf9,0xfa,0xfb,0xfc,0xfd,0xfe,0xff};// Initialization vector
unsigned char testkey[] = {0x60,0x3d,0xeb,0x10,0x15,0xca,0x71,0xbe,0x2b,0x73,0xae,0xf0,0x85,0x7d,0x77,0x81,0x1f,0x35,0x2c,0x07,0x3b,0x61,0x08,0xd7,0x2d,0x98,0x10,0xa3,0x09,0x14,0xdf,0xf4};// 32bytes encrytpion key

BOOL
EncryptionHandler( 
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply )
{
	BOOL retVal = FALSE;
	
	if (	FILTER_REQUEST_ENCRYPTION_IV_AND_KEY == messageSend->FilterCommand
		||	FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA == messageSend->FilterCommand )
    {
        //this is encryption filter rule with boolean config "REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE" enabled.                        
        //the filter driver request the IV and key to open or create the encrypted file.                        

        //if you don't want to authorize the process to read the encrytped file,you can set the value as below:
        //messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
        //messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION; 

		if( FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA == messageSend->FilterCommand	)
		{
			//this is new created file to request the encryption key and iv,
			//you can set the custom tag data to the header of the encrypted file if you set the below value.							

			//if you want to block the new file creation, return  STATUS_ACCESS_DENIED
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			//if you don't want to encrypt this new file , return STATUS_FILE_NOT_ENCRYPTED	
			//messageReply->ReturnStatus = STATUS_FILE_NOT_ENCRYPTED;

			//if you want to return the encryption key and iv for new created file, then return  STATUS_SUCCESS
			//messageReply->ReturnStatus = STATUS_SUCCESS;		

			//make sure the tag data length is less than the AES_MAX_TAG_DATA_SIZE;
			if( AES_MAX_TAG_DATA_SIZE > messageSend->FileNameLength )
			{
				//for new created file encryption, you can append your own custom tag data to the encryption header.
				//here we put the file name as the tag data for test purpose.

				messageReply->ReplyData.AESData.Data.TagDataLength = messageSend->FileNameLength;
				memcpy(messageReply->ReplyData.AESData.Data.TagData,messageSend->FileName,messageSend->FileNameLength);	
			}

			wprintf(L"New created file :%ws is requesting encryption key, iv and tag data, return status:%0x\n",messageSend->FileName, messageReply->ReturnStatus );

		}
		else if(FILTER_REQUEST_ENCRYPTION_IV_AND_KEY == messageSend->FilterCommand)
		{
			//opening the existing encrypted file, request the encryption key.
			//please cache the encryption key and tag data in local, since the request will be sent very often.

			//here is the custom tag data for the encrypted file which was embedded in the encryption header.
			//messageSend->DataBufferLength;
			//messageSend->DataBuffer;

			//if you want to block the encrypted file open, return  STATUS_ACCESS_DENIED
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			//if you want to return the raw encrypted data, then return STATUS_FILE_ENCRYPTED, i.e., for backup software or other application requre raw encrypted data.
			//messageReply->ReturnStatus = STATUS_FILE_ENCRYPTED;

			//if you want to decrypt the file, then return status success, and the encryption key and iv.
			messageReply->ReturnStatus = STATUS_SUCCESS;

		}

        //Here we return the default test iv and key to the filter driver, you can replace it with your own iv and key.
		messageReply->ReplyData.AESData.Data.AccessFlag = ALLOW_MAX_RIGHT_ACCESS;

		//if you want to use your own iv for the encrypted file, set the value here, 
        //or set the IVLength to 0, then the unique auto generated iv will be assigned to the file.
		if(testiv && 16 == sizeof(testiv))
		{
			messageReply->ReplyData.AESData.Data.IVLength = 16;
			memcpy(messageReply->ReplyData.AESData.Data.IV,testiv,16); 
		}
		else
		{
			messageReply->ReplyData.AESData.Data.IVLength = 0;
		}

		//here is the encryption key for the encrypted file, you can set it with your own key.
		messageReply->ReplyData.AESData.Data.EncryptionKeyLength = 32;
        memcpy(messageReply->ReplyData.AESData.Data.EncryptionKey,testkey,32);
        
		//the total return size
		messageReply->ReplyData.AESData.SizeOfData = sizeof(messageReply->ReplyData.AESData.Data) + messageReply->ReplyData.AESData.Data.TagDataLength ;


    }

	return retVal;
}




#endif