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

        // ��ǰ Ÿ���� �Ҹ����� �ƴѰ��
        if (product != null && product.ProductType != ProductType.Consumable)
        { 
            if (IAPManager.Instance.HadPurchase(proudctId))
            {
                Debug.Log($"�̹� ������ ��ǰ�Դϴ�.");
                return;
            } 
        }
        else if(product == null)
        {
            Debug.LogError($"��ǰ�� �����ϴ�!! {proudctId}");
            return;
        }

        //����
        IAPManager.Instance.Purchase(proudctId); 
    }
}
