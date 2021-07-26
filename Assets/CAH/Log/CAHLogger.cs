using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CAHLogger
{
    public static void Log(this object mine, string message)
    {
        Debug.Log(mine.GetType().Name + " :: " + message);
    }
}
