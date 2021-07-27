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

    [System.Serializable]
    public class Store
    {
        public IExtensionProvider StoreExtenstionProvider;
        public IStoreController StoreController;

        /// <summary>
        /// ���⿡ ���� �ִ� ��ǰ���� �־���մϴ�.
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
        /// IAP���� �޾ƿ� �����͵�
        /// </summary>
        public Store Store = new Store();

        
        /// <summary>
        /// Unity IAP ���� ������ ����
        /// </summary>
        private IStoreController _storeController;
        /// <summary>
        /// Unity IAP �ٸ� �÷��� �߰��� Ȯ���������̽�
        /// </summary>
        private IExtensionProvider _storeExtenstionProvider;   
        /// <summary>
        /// �ʱ�ȭ����
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
        /// IAP�� �ʱ�ȭ�Ǹ� ȣ��˴ϴ�
        /// </summary>
        void InitUnityIAP()
        {
            //�ξ� ���� ���� ��������
            //StandardPurchasingModule = ����� �⺻����
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());



            foreach (var product in Store.LocalProducts)
            {
                builder.AddProduct(product.ProductName, product.ProductType, new IDs(){
                    { product.IOSProductId, AppleAppStore.Name },
                    { product.AndroidProudctId, GooglePlay.Name },
                });

                this.Log("��ǰ ���� ���� " + product.AndroidProudctId);
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
            this._storeController = controller;
            this._storeExtenstionProvider = extensions;

            this.Store.StoreController = controller;
            this.Store.StoreExtenstionProvider = extensions;
            this.Store.ReceivedProducts = controller.products;
           
            this.Log("����Ƽ IAP �ʱ�ȭ��!");

            this.Log("����� ��ǰ �о������..");

            foreach (var product in Store.StoreController.products.all)
            {
                this.Log("����� ��ǰ �޾ƿ� => " + product.definition.id);
            }
            this.Log("���� ��ǰ �о������..");
            foreach (var product in Store.LocalProducts)
            {
                this.Log("���� ��ǰ �޾ƿ� => " + product.PlatformProductId);
            }


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

            var product = _storeController.products.WithID(id);
            if(product != null)
            {
                Debug.Log("���� �õ� - " + product.definition.id);
                _storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"���� �õ� ����, product�� null�Դϴ�. �õ��� ��ǰ id=> {id}");
            }
        }
        public bool Purchase(LocalProduct product) => HadPurchase(product.PlatformProductId);
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
                var extenstion = _storeExtenstionProvider.GetExtension<IAppleExtensions>();
                extenstion.RestoreTransactions((success) => {
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
                var extenstion = _storeExtenstionProvider.GetExtension<IGooglePlayStoreExtensions>();
                extenstion.RestoreTransactions((success) => {
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
        /// ������ ��ǰ���� Ȯ��
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