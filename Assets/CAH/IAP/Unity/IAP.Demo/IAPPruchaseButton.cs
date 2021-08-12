using CAH.IAP.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPPruchaseButton : MonoBehaviour
{
    public bool autoInjectionOnClick;
    public string proudctId;

    public void Awake()
    {
        if(autoInjectionOnClick)
        GetComponent<Button>()?.onClick.AddListener(() => {
            OnClick();
        });
    }

    public bool nocheckType = false;
    public void OnClick()
    {
        if (nocheckType)
        {
            var product2 = IAPManager.Instance.GetProductById(proudctId);

            Debug.Log(product2.metadata.localizedPriceString);
            Debug.Log(product2.receipt);
            Debug.Log(product2.transactionID);
            IAPManager.Instance.Purchase(proudctId);
            return;
        }

        Debug.Log("OnClick");
        var product = IAPManager.Instance.Store.LocalProducts.Find(x => x.PlatformProductId == this.proudctId);
        if(product == null)
        {
            Debug.LogError($"상품이 없습니다!! {proudctId}");
            return;
        }
        // 상품 타입이 소모형이 아닌경우
        if (product.ProductType != ProductType.Consumable)
        { 
            if (IAPManager.Instance.HadPurchase(proudctId))
            {
                Debug.Log($"이미 구매한 상품입니다.");
                return;
            } 
        }
      
        //구매
        IAPManager.Instance.Purchase(proudctId); 
    }
}
