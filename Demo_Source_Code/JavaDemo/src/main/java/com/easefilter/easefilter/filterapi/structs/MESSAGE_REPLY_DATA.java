package com.easefilter.easefilter.filterapi.structs;

import com.easefilter.easefilter.rules.BaseFilterRule;
import com.sun.jna.Pointer;
import com.sun.jna.Structure;

/**
 * C struct for userspace-to-driver communication.
 */
@Structure.FieldOrder({"MessageId", "MessageType", "ReturnStatus", "FilterStatus", "ReplyData"})
public class MESSAGE_REPLY_DATA extends Structure {
   public int MessageId;
   public int MessageType;
   public int ReturnStatus;
   public int FilterStatus;
   public ReplyData ReplyData;
}
