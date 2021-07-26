using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CAH.IAP.Unity
{
    public class Product
    {
        /// <summary>
        /// 상품이름
        /// </summary>
        public string ProductName;

        /// <summary>
        /// IOS 에서의 상품의 ID
        /// </summary>
        public string IOSProductId;

        /// <summary>
        /// Android 에서의 상품 ID
        /// </summary>
        public string AndroidProudctId;


        /// <summary>
        /// 상품의 타입
        /// </summary>
        public ProductType ProductType;
    }

    public class Store
    {
        public List<Product> products = new List<Product>();

        public void LoadProduct(string path)
        {

        }

    }
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        public static IAPManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<IAPManager>();
                if (_instance == null)
                    return new GameObject().AddComponent<IAPManager>();

                return _instance;
            }
        }
        static IAPManager _instance;

        /// <summary>
        /// 플레이스토어와 같은 스토어에서 받아온 데이터들
        /// </summary>
        public Store store = new Store();


        /// <summary>
        /// Unity IAP 구매 과정을 제어
        /// </summary>
        private IStoreController storeController;
        /// <summary>
        /// Unity IAP 다른 플랫폼 추가용 확장인터페이스
        /// </summary>
        private IExtensionProvider storeExtnetionProvider;  


        /// <summary>
        /// 초기화여부
        /// </summary>
        public bool IsInitialized => storeController != null && storeExtnetionProvider != null;
        void Awake()
        {
            if (_instance != null && _instance != this) 
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            InitUnityIAP();
            store.products.Add(new Product()
            {

            });
        }
         

        void InitUnityIAP()
        {
            //인앱 관련 설정 빌더생성
            //StandardPurchasingModule = 스토어 기본설정
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var product in store.products)
            {
                builder.AddProduct(product.ProductName, product.ProductType, new IDs(){
                { product.IOSProductId, AppleAppStore.Name },
                { product.AndroidProudctId, GooglePlay.Name },
            });
            }
            UnityPurchasing.Initialize(this, builder);
        }
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.storeController = controller;
            this.storeExtnetionProvider = extensions;

            this.Log("유니티 IAP 초기화됨!");  
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            this.Log($"유니티 IAP 초기화 실패 {error}");
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            this.Log($"IAP 구매 실패 {failureReason}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            this.Log($"IAP 구매 성공 => {args.purchasedProduct.definition.id}");
           
            // 구매한 상품
            var id = args.purchasedProduct.definition.id;


            return PurchaseProcessingResult.Complete;
        } 



        /// <summary>
        /// 구매 시도
        /// </summary>
        /// <param name="id"></param>
        public void PurchaseTest(string id)
        {
            if (!IsInitialized) return;

            var product = storeController.products.WithID(id);
            if(product != null)
            {
                Debug.Log("구매 시도 - " + product.definition.id);
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"구매 시도 실패, product가 null입니다. 시도한 상품 id=> {id}");
            }
        }

        /// <summary>
        /// 자동 복구 (구매를 했는데 보상을 못받은경우)
        /// </summary>
        public void RestorePurchase()
        {
            if (!IsInitialized) return;
            var platform = Application.platform;
            if (platform == RuntimePlatform.IPhonePlayer || 
                platform == RuntimePlatform.OSXPlayer)
            {
                var extention = storeExtnetionProvider.GetExtension<IAppleExtensions>();
                extention.RestoreTransactions((success) => { 
                    if(success == false)
                    {
                        Debug.Log($"구매 복구 시도 실패");
                    }
                    else
                    {
                        Debug.Log($"구매 복구 성공");
                    }
                });
            }
        }

        public bool HadPurchase(string productId)
        {
            if (!IsInitialized) return false;
            var product = storeController.products.WithID(productId);

            if (product != null)
                return product.hasReceipt;

            return false;
        }
    }

}