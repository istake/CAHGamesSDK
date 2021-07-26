using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CAH.IAP.Unity
{
    /// <summary>
    /// �� ���ο��� �̸� ĳ���س��� ������ ����������
    /// </summary>
    
    [System.Serializable]
    public class LocalProduct
    {
        /// <summary>
        /// ��ǰ�̸�
        /// </summary>
        public string ProductName;

        /// <summary>
        /// IOS ������ ��ǰ�� ID
        /// </summary>
        public string IOSProductId;

        /// <summary>
        /// Android ������ ��ǰ ID
        /// </summary>
        public string AndroidProudctId; 

        /// <summary>
        /// ��ǰ�� Ÿ��
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
                    Debug.Log("IAP ������ ����Ƽ �����Ϳ��� �� �� �����ϴ�.");
                throw new System.Exception("IAPManager�� Platform Product ID�� �����ϴ�.");
            }
        }
    }

    public class Store
    {
        public IExtensionProvider storeExtnetionProvider;
        public IStoreController storeController;
        public List<LocalProduct> localProducts = new List<LocalProduct>();
        public ProductCollection receivedProducts;


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
        /// IAP���� �޾ƿ� �����͵�
        /// </summary>
        public Store Store = new Store();


        /// <summary>
        /// Unity IAP ���� ������ ����
        /// </summary>
        private IStoreController storeController;
        /// <summary>
        /// Unity IAP �ٸ� �÷��� �߰��� Ȯ���������̽�
        /// </summary>
        private IExtensionProvider storeExtnetionProvider;   
        /// <summary>
        /// �ʱ�ȭ����
        /// </summary>
        /// 

        public bool IsInitialized => storeController != null && storeExtnetionProvider != null;


        public void GetStore()
        {

        }
        void Awake()
        {
            if (_instance != null && _instance != this) 
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            Store = new Store();
            Store.localProducts.Add(new LocalProduct()
            {
                AndroidProudctId = "diamond100",
                ProductName = "diamond",
                IOSProductId = "diamond100",
                ProductType = ProductType.Consumable
            });

            InitUnityIAP(); 
        }
         

        /// <summary>
        /// IAP�� �ʱ�ȭ�Ǹ� ȣ��˴ϴ�
        /// </summary>
        void InitUnityIAP()
        {
            //�ξ� ���� ���� ��������
            //StandardPurchasingModule = ����� �⺻����
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var product in Store.localProducts)
            {
                builder.AddProduct(product.ProductName, product.ProductType, new IDs(){
                    { product.IOSProductId, AppleAppStore.Name },
                    { product.AndroidProudctId, GooglePlay.Name },
                });
            }
            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// �ʱ�ȭ�� �Ϸ�Ȱ�� ȣ��Ǵ� �ݹ�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.storeController = controller;
            this.storeExtnetionProvider = extensions;

            this.Store.storeController = controller;
            this.Store.storeExtnetionProvider = extensions;
            this.Store.receivedProducts = controller.products;
            this.Log("����Ƽ IAP �ʱ�ȭ��!");  
        }

        /// <summary>
        /// ���� �ݹ�
        /// </summary>
        /// <param name="error"></param>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            this.Log($"����Ƽ IAP �ʱ�ȭ ���� {error}");
        }


        /// <summary>
        /// ���� �����ݹ�
        /// </summary>
        /// <param name="product"></param>
        /// <param name="failureReason"></param>
        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            this.Log($"IAP ���� ���� {failureReason}");
        }


        /// <summary>
        /// ���� ���μ��� �ݹ�
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            this.Log($"IAP ���� ���� => {args.purchasedProduct.definition.id}");
           
            // ������ ��ǰ
            var id = args.purchasedProduct.definition.id;


            return PurchaseProcessingResult.Complete;
        } 



        /// <summary>
        /// ���� �õ�
        /// </summary>
        /// <param name="id"></param>
        public void Purchase(string id)
        {
            if (!IsInitialized) return;

            var product = storeController.products.WithID(id);
            if(product != null)
            {
                Debug.Log("���� �õ� - " + product.definition.id);
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"���� �õ� ����, product�� null�Դϴ�. �õ��� ��ǰ id=> {id}");
            }
        }

        /// <summary>
        /// �ڵ� ���� (���Ÿ� �ߴµ� ������ ��������� ȣ��)
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
                    if (success == false)
                    {
                        Debug.Log($"���� ���� �õ� ����");
                    }
                    else
                    {
                        Debug.Log($"���� ���� ����");
                    }
                });
            }

            if (platform == RuntimePlatform.Android)
            {
                var extention = storeExtnetionProvider.GetExtension<IGooglePlayStoreExtensions>();
                extention.RestoreTransactions((success) => {
                    if (success == false)
                    {
                        Debug.Log($"���� ���� �õ� ����");
                    }
                    else
                    {
                        Debug.Log($"���� ���� ����");
                    }
                });
            }
        }


        /// <summary>
        /// ���� ���δ�Ʈ id�� �ִ��� Ȯ��
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
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