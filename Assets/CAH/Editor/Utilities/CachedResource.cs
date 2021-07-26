

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace CAH.Editor.Utilities
{
    public class CachedResource
    {
        public static Dictionary<string, Texture2D> cached = new Dictionary<string, Texture2D>();
        public static Texture2D LoadTextureFromResource(string path)
        {

            if (cached.ContainsKey(path))
            {
                return cached[path];
            }
            else
            {
                var tex2D = Resources.Load<Texture2D>(path);
                cached.Add(path, tex2D);
                return tex2D;
            }
        }
    }
}
 
#endif