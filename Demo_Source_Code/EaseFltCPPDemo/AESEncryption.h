///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2015 EaseFilter Technologies Inc.
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
///////////////////////////////////////////////////////////////////////////////

#ifndef __AES_ENCRYPTION_H__
#define __AES_ENCRYPTION_H__

VOID
EncryptionUnitTest();

BOOL 
EncryptionRequestHandler( IN	PMESSAGE_SEND_DATA pSendMessage,IN OUT	PMESSAGE_REPLY_DATA pReplyMessage );

#endif