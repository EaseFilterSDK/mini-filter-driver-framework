///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Globalization;


namespace EaseFilter.CommonObjects
{
    public enum EventLevel
    {
        Off = 0,
        Error = 1,
        Warning = 2,
        Information = 3,
        Verbose = 4,
        Trace = 5,
    }

    public enum EventOutputType
    {
        EventView = 0,
        File,
        Console,
        CallbackDelegate,
        NamedPipe,
        DbgView,
    }    
  

    public class MessageEventArgs : EventArgs
    {
        EventLevel eventType = EventLevel.Verbose;
        string message = string.Empty;
        string callerName = string.Empty;
        int eventId = 0;
        DateTime time = DateTime.MinValue;

        public MessageEventArgs(int eventId, string callerName, EventLevel eventType, string message,DateTime messageTime)
        {
            this.eventType = eventType;
            this.message = message;
            this.callerName = callerName;
            this.eventId = eventId;
            this.time = messageTime;
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public EventLevel Type
        {
            get { return eventType; }
            set { eventType = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string CallerName
        {
            get { return callerName; }
            set { callerName = value; }
        }

        public int EventID
        {
            get { return eventId; }
            set { eventId = value; }
        }

    }


    public static class EventManager
    {
        public delegate void ShowNotificationDlgt(string message, bool isErrorMessage);
        public static ShowNotificationDlgt showNotificationDlgt = null;

        public delegate void MessageEventDlgt(MessageEventArgs messageEventArg);

        static event MessageEventDlgt messageEventHandler;
        static EventLevel level = GlobalConfig.EventLevel;
        static EventOutputType output = GlobalConfig.EventOutputType;
        static string logFileName = GlobalConfig.EventLogFileName;
        static string eventSource = GlobalConfig.EventSource;
        static string eventLogName = GlobalConfig.EventLogName;
        static EventLog eventLog = null;
      //  static string PipeName = "MessagePipe";

        static AutoResetEvent autoEvent = new AutoResetEvent(false);
        static Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();
        static Thread eventThread = null;

        public static AutoResetEvent logFileSyncEvent = new AutoResetEvent(true);

        static System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
        public static string locallAssemblyPath = Path.GetDirectoryName(assembly.Location);

        static char taskStringSeperator = '|';

        public static string FormatDateTime(DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }

        static EventManager()
        {
            if (eventThread == null)
            {
                eventThread = new Thread(new ThreadStart(ProcessMessage));
                eventThread.Name = "EventManagerProcessThread";
                eventThread.Start();
            }
        }

        public static void WriteMessage(int eventId, string callerName, EventLevel evnetType, string message)
        {
            if (evnetType > level)
            {
                return;
            }

            MessageEventArgs messageEventArgs = new MessageEventArgs(eventId, callerName, evnetType, message,DateTime.Now);

            lock(messageQueue)
            {
                messageQueue.Enqueue(messageEventArgs);
            }

            autoEvent.Set();

        }

        public static string EventSource
        {
            get { return eventSource; }
            set { eventSource = value; }
        }

        public static string EventLogName
        {
            get { return eventLogName; }
            set { eventLogName = value; }
        }

        public static void Stop()
        {
           logFileSyncEvent.Set();
           autoEvent.Set();

        }

        public static MessageEventDlgt MessageEventHandler
        {
            get { return messageEventHandler; }
            set { messageEventHandler = value; }
        }

        public static EventLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        public static EventOutputType Output
        {
            get { return output; }
            set { output = value; }
        }

        public static string LogFileName
        {
            get { return logFileName; }
            set { logFileName = value; }
        }

        private static void SendMessageToNamedpipe(MessageEventArgs messageEventArg)
        {
            try
            {
             

            }
            catch
            {
              
            }
        }

        private static string ConvertEventArgToSring(MessageEventArgs messageEventArg)
        {
            string text = FormatDateTime(messageEventArg.Time)
                  + taskStringSeperator + messageEventArg.Type
                  + taskStringSeperator + messageEventArg.EventID
                  + taskStringSeperator + messageEventArg.CallerName
                  + taskStringSeperator + messageEventArg.Message;

            return text;
        }


        public static MessageEventArgs ConvertSringToEventArg(string message)
        {
            string[] strs = message.Split(new char[] { taskStringSeperator });

            if (strs.Length < 5)
            {
                //the log message didn't log complete message, skip
                return null;
            }

            DateTime time = DateTime.ParseExact(strs[0], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            EventLevel type = (EventLevel)Enum.Parse(typeof(EventLevel), strs[1]); 
            int id = int.Parse(strs[2]);
            string callerName = strs[3];
            string txt = strs[4];

            MessageEventArgs messageArg = new MessageEventArgs(id,callerName,type,txt,time);

            return messageArg;
        }

        private static void SendMessageToFile(MessageEventArgs messageEventArg)
        {
            try
            {
                string logFileFullPath = Path.Combine(locallAssemblyPath, logFileName);

                if (File.Exists(logFileFullPath))
                {
                    FileInfo fileInfo = new FileInfo(logFileFullPath);

                    if (fileInfo.Length > GlobalConfig.MaxEventLogFileSize)
                    {
                        if (File.Exists(logFileFullPath + ".1"))
                        {
                            File.Delete(logFileFullPath + ".1");                           
                        }

                        File.Move(logFileFullPath, logFileFullPath + ".1");
                    }
                }


                EventManager.logFileSyncEvent.WaitOne();

                string text = ConvertEventArgToSring(messageEventArg).Replace("\n", "") + Environment.NewLine;
                File.AppendAllText(logFileFullPath, text);

                EventManager.logFileSyncEvent.Set();

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static void SendMessageToEventView(MessageEventArgs messageEventArg)
        {

            try
            {
                string text = DateTime.Now.ToString("G")
                    + ":" + messageEventArg.Type
                    + ":" + messageEventArg.CallerName
                    + ":" + messageEventArg.Message;

                if (null == eventLog)
                {
                    if (!EventLog.SourceExists(eventSource))
                    {
                        EventLog.CreateEventSource(eventSource, eventLogName);
                    }

                    eventLog = new EventLog(eventLogName, ".", eventSource);
                }

                switch (messageEventArg.Type)
                {
                    case EventLevel.Error:
                    case EventLevel.Warning:
                        eventLog.WriteEntry(messageEventArg.Message + "\n\n Caller:" + messageEventArg.CallerName
                                                , (EventLogEntryType)messageEventArg.Type, messageEventArg.EventID);
                        break;

                    case EventLevel.Information:
                    case EventLevel.Verbose:
                        eventLog.WriteEntry(messageEventArg.Message + "\n\n Caller:" + messageEventArg.CallerName
                                                , EventLogEntryType.Information, messageEventArg.EventID);
                        break;
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }


        static void PrintWarning(string info)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(info);
            Console.ForegroundColor = oldColor;
        }

        static void PrintError(string info)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(info);
            Console.ForegroundColor = oldColor;
        }

        private static void SendMessageToConsole(MessageEventArgs messageEventArg)
        {
            try
            {
                string text = "Id:" + messageEventArg.EventID
                  + ":" + messageEventArg.CallerName + Environment.NewLine
                  + messageEventArg.Message + Environment.NewLine;

                if (messageEventArg.Type == EventLevel.Error)
                {
                    PrintError(text);
                }
                else if (messageEventArg.Type == EventLevel.Warning)
                {
                    PrintWarning(text);
                }
                else
                {
                    Console.WriteLine(text);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static void ProcessMessage()
        {
             WaitHandle[] waitHandles = new WaitHandle[] { autoEvent , GlobalConfig.StopEvent };

            while (GlobalConfig.IsRunning)
            {
                if (messageQueue.Count == 0)
                {
                    int result = WaitHandle.WaitAny(waitHandles);

                    if (!GlobalConfig.IsRunning)
                    {
                        return;
                    }
                }

                MessageEventArgs message = null;

                lock( messageQueue)
                {
                    if( messageQueue.Count > 0)
                    {
                        message = messageQueue.Dequeue();    
                    }
                }

                if (message == null)
                {
                    continue;
                }

                try
                {
                    switch (output)
                    {
                        case EventOutputType.EventView: SendMessageToEventView(message); break;
                        case EventOutputType.File: SendMessageToFile(message); break;
                        case EventOutputType.Console: SendMessageToConsole(message); break;
                        case EventOutputType.CallbackDelegate: messageEventHandler(message); break;
                        case EventOutputType.NamedPipe: SendMessageToNamedpipe(message); break;
                        //  case EventOutputType.MessageView: messageForm.AddMessageToEventView(message); break;
                        case EventOutputType.DbgView: System.Diagnostics.Debug.WriteLine(ConvertEventArgToSring(message)); break;
                    }

                    if (GlobalConfig.EnableNotification && null != showNotificationDlgt && message.Type == EventLevel.Error)
                    {
                        showNotificationDlgt(message.Message, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Log message exception:" + ex.Message + ".\r\nMessage:" + message);
                }

                if (GlobalConfig.EnableLogTransaction)
                {
                    try
                    {
                        SendMessageToFile(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Write message exception:" + ex.Message + ".\r\nMessage:" + message);
                    }
                }
            }
        }
    }
}
