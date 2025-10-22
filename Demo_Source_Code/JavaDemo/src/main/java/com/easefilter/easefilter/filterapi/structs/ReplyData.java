package com.easefilter.easefilter.filterapi.structs;

import com.sun.jna.Union;

/**
 * Helper type for {@link MESSAGE_REPLY_DATA}.
 */
@Union.FieldOrder({"Data", "AESData", "UserInfo", "FileInfo"})
public class ReplyData extends Union {
   public Data Data;
   public AESData AESData;
   public UserInfo UserInfo;
   public FileInfo FileInfo;
}
