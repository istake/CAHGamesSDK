using System.Threading.Tasks;

namespace CAH.Auth.Mobile
{
    public interface IAuthService
    {
        bool IsInitialize { get; }
        /// <summary>
        /// 초기화
        /// </summary>
        void Initialize();
        /// <summary>
        /// Sign에대한 결과가 테스크로 전달됨
        /// </summary> 
        void SignIn(System.Action<object> result);
        /// <summary>
        /// SignOut에대한 결과가 테스크로 전달됨
        /// </summary> 
        void SignOut(System.Action<object> result);  
        
        
    }
}