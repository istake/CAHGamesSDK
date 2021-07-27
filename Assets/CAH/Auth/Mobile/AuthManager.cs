using UnityEngine;

namespace CAH.Auth.Mobile
{
    public class AuthManager : MonoBehaviour
    {
        public static AuthManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AuthManager>();
                if (_instance == null)
                    return new GameObject().AddComponent<AuthManager>();

                return _instance;
            }
        }

        static AuthManager _instance;

        void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        private IAuthService service;

        public void SignInGoogle()
        {
            if (service == null || (service is GoogleAuthService) == false) 
                service = new GoogleAuthService(); 
            
            service.SignIn(x => { });
        }
        public void SIgnOutGoogle()
        {
            if (service == null || (service is GoogleAuthService) == false) 
                service = new GoogleAuthService(); 
            
            service.SignOut(x => { });
        }
    }
}