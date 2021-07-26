using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CAH.Plugins.WebGL.Utilities
{
    public static class CommonUtiles
    {
        [DllImport("__Internal")]
        private static extern void IsMobile();
        public static bool isMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
#endif
            return false;
        }
    }
}