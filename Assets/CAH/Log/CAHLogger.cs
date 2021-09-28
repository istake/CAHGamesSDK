using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CAHLoggerExtend
{
    public static void Log(this object mine, string message)
    {
        Debug.Log(mine.GetType().Name + " :: " + message);
    }
    public static void Log(this object mine, object message)
    {
        Debug.Log(mine.GetType().Name + " :: " + message);
    }
    public static void Error(this object mine, object message)
    {
        Debug.LogError(mine.GetType().Name + " :: " + message);
    } 
}
