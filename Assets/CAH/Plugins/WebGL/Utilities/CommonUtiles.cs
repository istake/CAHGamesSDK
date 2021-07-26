
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CAH.Plugins.WebGL.Utilities
{
    public static class CommonUtiles
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void IsMobile();
#endif
        public static bool isMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
#endif
            return false;
        }
    }
}