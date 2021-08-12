using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSLog : MonoBehaviour
{

    public struct LogQueueData
    {
        public string Log;
        public DateTime DateTime;
        /// <summary>
        /// 0 = log
        /// 1 = warning
        /// 2 = error
        /// </summary>
        public int Type;

        public bool Sended;
        public bool Cancel;
    }

    public void WebSocketConnect()
    {

    }

    public static void Log(string log)
    {

    }
    public static void Error(string log)
    {

    }
    public static void Warning(string log)
    {

    }

}
