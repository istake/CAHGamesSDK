
using UnityEngine;

namespace CAH.Editor.Utilities
{
    class PathExtention
    {
        /// <summary>
        /// 절대 경로를 상대 경로로 변경해줍니다
        /// C:\Project\Assets\Example.. => Assets\Example
        /// </summary>
        /// <param name="path"></param>
        public static void AbsoluteToRelative(string path)
        {
            string relativepath = null;
            if (path.StartsWith(Application.dataPath))
            {
                relativepath = "Assets" + path.Substring(Application.dataPath.Length);
            }
        }
    }
}
