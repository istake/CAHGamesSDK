using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CAH.IAP.Unity
{
    /// <summary>
    /// 앱 내부에서 미리 캐싱해놓고 저장할 상점데이터
    /// </summary>
    
    [System.Serializable]
    public class LocalProduct
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

        public string PlatformProductId
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                    return AndroidProudctId;
                if (Application.platform == RuntimePlatform.IPhonePlayer ) 
                    return IOSProductId;
                if (Application.isEditor)
                    Debug.Log("IAP 결제는 유니티 에디터에서 할 수 없습니다.");
                throw new System.Exception("IAPManager에 Platform Product ID가 없습니다.");
            }
        }
    }

    [System.Serializable]
    public class Store
    {
        public IExtensionProvider StoreExtenstionProvider;
        public IStoreController StoreController;

        /// <summary>
        /// 여기에 스토어에 있는 상품들을 넣어야합니다.
        /// </summary>
        public List<LocalProduct> LocalProducts = new List<LocalProduct>();
        public ProductCollection ReceivedProducts; 
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
        /// IAP에서 받아온 데이터들
        /// </summary>
        public Store Store = new Store();

        
        /// <summary>
        /// Unity IAP 구매 과정을 제어
        /// </summary>
        private IStoreController _storeController;
        /// <summary>
        /// Unity IAP 다른 플랫폼 추가용 확장인터페이스
        /// </summary>
        private IExtensionProvider _storeExtenstionProvider;   
        /// <summary>
        /// 초기화여부
        /// </summary>
        /// 

        public bool IsInitialized => _storeController != null && _storeExtenstionProvider != null;


        public Store GetStore()
        {
            return Store;
        }
        void Awake()
        {
            if (_instance != null && _instance != this) 
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            Store = new Store();
            Store.LocalProducts.Add(new LocalProduct()
            {
                AndroidProudctId = "diamond100",
                ProductName = "diamond100",
                IOSProductId = "diamond100",
                ProductType = ProductType.Consumable
            });

            InitUnityIAP(); 
        }
         

        /// <summary>
        /// IAP가 초기화되면 호출됩니다
        /// </summary>
        void InitUnityIAP()
        {
            //인앱 관련 설정 빌더생성
            //StandardPurchasingModule = 스토어 기본설정
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());



            foreach (var product in Store.LocalProducts)
            {
                builder.AddProduct(product.ProductName, product.ProductType, new IDs(){
                    { product.IOSProductId, AppleAppStore.Name },
                    { product.AndroidProudctId, GooglePlay.Name },
                });

                this.Log("상품 정보 갱신 " + product.AndroidProudctId);
            }

 
            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// 초기화가 완료된경우 호출되는 콜백
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this._storeController = controller;
            this._storeExtenstionProvider = extensions;

            this.Store.StoreController = controller;
            this.Store.StoreExtenstionProvider = extensions;
            this.Store.ReceivedProducts = controller.products;
           
            this.Log("유니티 IAP 초기화됨!");

            this.Log("스토어 상품 읽어오는중..");

            foreach (var product in Store.StoreController.products.all)
            {
                this.Log("스토어 상품 받아옴 => " + product.definition.id);
            }
            this.Log("로컬 상품 읽어오는중..");
            foreach (var product in Store.LocalProducts)
            {
                this.Log("로컬 상품 받아옴 => " + product.PlatformProductId);
            }


        }

        /// <summary>
        /// 실패 콜백
        /// </summary>
        /// <param name="error"></param>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            this.Log($"유니티 IAP 초기화 실패 {error}");
        }


        /// <summary>
        /// 구메 실패콜백
        /// </summary>
        /// <param name="product"></param>
        /// <param name="failureReason"></param>
        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            this.Log($"IAP 구매 실패 {failureReason}");
        }


        /// <summary>
        /// 구매 프로세싱 콜백
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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
        public void Purchase(string id)
        {
            if (!IsInitialized) return;

            var product = _storeController.products.WithID(id);
            if(product != null)
            {
                Debug.Log("구매 시도 - " + product.definition.id);
                _storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"구매 시도 실패, product가 null입니다. 시도한 상품 id=> {id}");
            }
        }
        public bool Purchase(LocalProduct product) => HadPurchase(product.PlatformProductId);
        /// <summary>
        /// 자동 복구 (구매를 했는데 보상을 못받은경우 호출)
        /// </summary>
        public void RestorePurchase()
        {
            if (!IsInitialized) return;
            var platform = Application.platform;


            if (platform == RuntimePlatform.IPhonePlayer ||
                platform == RuntimePlatform.OSXPlayer)
            {
                var extenstion = _storeExtenstionProvider.GetExtension<IAppleExtensions>();
                extenstion.RestoreTransactions((success) => {
                    if (success == false)
                    {
                        Debug.Log($"구매 복구 시도 실패");
                    }
                    else
                    {
                        Debug.Log($"구매 복구 성공");
                    }
                });
            }

            if (platform == RuntimePlatform.Android)
            {
                var extenstion = _storeExtenstionProvider.GetExtension<IGooglePlayStoreExtensions>();
                extenstion.RestoreTransactions((success) => {
                    if (success == false)
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


        /// <summary>
        /// 구매한 상품인지 확인
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool HadPurchase(string productId)
        {
            if (!IsInitialized) return false;
            var product = _storeController.products.WithID(productId);

            if (product != null)
                return product.hasReceipt;

            return false;
        }

        public bool HadPurchase(LocalProduct product) => HadPurchase(product.PlatformProductId);
    }

}