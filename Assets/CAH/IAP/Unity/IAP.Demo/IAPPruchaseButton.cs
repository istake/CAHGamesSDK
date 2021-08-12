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
            Debug.LogError($"��ǰ�� �����ϴ�!! {proudctId}");
            return;
        }
        // ��ǰ Ÿ���� �Ҹ����� �ƴѰ��
        if (product.ProductType != ProductType.Consumable)
        { 
            if (IAPManager.Instance.HadPurchase(proudctId))
            {
                Debug.Log($"�̹� ������ ��ǰ�Դϴ�.");
                return;
            } 
        }
      
        //����
        IAPManager.Instance.Purchase(proudctId); 
    }
}
