using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CAHLoggerExtention
{
    public static void Log(this object mine, string message)
    {
#if CAH_LOGGER_DISABLED
        Debug.Log(mine.GetType().Name + " :: " + message);
#endif
    }
    public static void Log(this object mine, object message)
    {
#if CAH_LOGGER_DISABLED
        Debug.Log(mine.GetType().Name + " :: " + message);
#endif
    }
    public static void Error(this object mine, object message)
    {
#if CAH_LOGGER_DISABLED
        Debug.LogError(mine.GetType().Name + " :: " + message);
#endif
    } 
}
