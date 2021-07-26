using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CAH.IAP.Unity
{
    public class Product
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
        /// �÷��̽����� ���� ������ �޾ƿ� �����͵�
        /// </summary>
        public Store store = new Store();


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
            //�ξ� ���� ���� ��������
            //StandardPurchasingModule = ����� �⺻����
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

            this.Log("����Ƽ IAP �ʱ�ȭ��!");  
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            this.Log($"����Ƽ IAP �ʱ�ȭ ���� {error}");
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            this.Log($"IAP ���� ���� {failureReason}");
        }

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
        public void PurchaseTest(string id)
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
        /// �ڵ� ���� (���Ÿ� �ߴµ� ������ ���������)
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
                        Debug.Log($"���� ���� �õ� ����");
                    }
                    else
                    {
                        Debug.Log($"���� ���� ����");
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