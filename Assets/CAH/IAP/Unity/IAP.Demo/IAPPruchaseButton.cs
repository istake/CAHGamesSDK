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
    public void OnClick()
    {
        var product = IAPManager.Instance.Store.LocalProducts.Find(x => x.PlatformProductId == this.proudctId);

        // 상품 타입이 소모형이 아닌경우
        if (product != null && product.ProductType != ProductType.Consumable)
        { 
            if (IAPManager.Instance.HadPurchase(proudctId))
            {
                Debug.Log($"이미 구매한 상품입니다.");
                return;
            } 
        }
        else if(product == null)
        {
            Debug.LogError($"상품이 없습니다!! {proudctId}");
            return;
        }

        //구매
        IAPManager.Instance.Purchase(proudctId); 
    }
}
