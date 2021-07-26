using CAH.IAP.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        var product = IAPManager.Instance.Store?.localProducts.Any(x => x.PlatformProductId == this.proudctId);
        if (product != null)
        { 
            if (IAPManager.Instance.HadPurchase(proudctId))
            {
                Debug.Log($"이미 구매한 상품입니다.");
                return;
            } 
        }
        else
        {
            Debug.LogError($"상품이 없습니다!! {proudctId}");
            return;
        }

        //구매
        IAPManager.Instance.Purchase(proudctId);

    }
}
