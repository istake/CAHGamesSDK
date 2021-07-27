using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;

namespace CAH.Auth.Mobile
{
    
    class GoogleAuthWebClientInfo
    {
        public string WebClientId;
    }
    public class GoogleAuthService : IAuthService
    {
        public bool IsInintialize => _sign != null;

        private GoogleSignIn _sign;
        public void Initialize()
        {
            if (!IsInintialize)
            {
                var webClientId = Resources.Load<TextAsset>("GoogleAuthService/WebClientId");
                if (webClientId == null)
                    throw new Exception("Resources GoogleAuthService/WebClientId.txt Is Null!");
                if (webClientId.text.Length <= 1)
                    throw new Exception("Resources GoogleAuthService/WebClientId.txt Is Invalid");
                var idMap = UnityEngine.JsonUtility.FromJson<GoogleAuthWebClientInfo>(webClientId.text);
                this.Log("Loaded Webclient id => " + idMap.WebClientId);
                
                #if UNITY_ANDROID && !UNITY_EDITOR
                Google.GoogleSignIn.Configuration = new GoogleSignInConfiguration()
                {
                    RequestIdToken = true,
                    RequestEmail = true,
                    WebClientId = idMap.WebClientId
                };
                _sign = GoogleSignIn.DefaultInstance;
                #endif
            } 
        }
        public void SignIn(System.Action<object> result)
        {
            Initialize();
            if (IsInintialize == false) return;
            _sign.SignIn().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    this.Log($"로그인이 Cancel 되었습니다.");
                }

                if (task.IsFaulted)
                {
                    this.Error($"로그인 실패 오류 {task.Exception} ");
                }

                if (task.IsCompleted)
                {
                    this.Log("로그인 성공");
                    GoogleSignInUser signInUser = task.Result;
                    
                    this.Log(signInUser.Email);
                    this.Log(signInUser.IdToken);
                    this.Log(signInUser.UserId);  
                } 
            });
        }

        public void SignOut(System.Action<object> result)
        {            
            if (IsInintialize == false) return;
            _sign.SignOut(); 
            result?.Invoke(true);
        }
 
    }
}