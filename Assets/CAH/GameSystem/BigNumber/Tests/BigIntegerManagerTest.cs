using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using CAH.GameSystem.BigNumber;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class BigIntegerManagerTest : MonoBehaviour
{
    #if ODIN_INSPECTOR
    public BigInteger TestNumber;
    [Button("Test")]
    public void Test(string value)
    {  
        Debug.Log(BigIntegerManager.UnitToValue(value));
    }
    #endif
}
