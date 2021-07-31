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
    public void Test(int value)
    { 
        BigInteger a = BigInteger.Parse("10000000000000000000000000000000000000000000000000000000000000000000000000000");
        for (int i = 0; i < value; i++)
        {
            BigIntegerManager.GetUnit(a);
        }

        Debug.Log(BigIntegerManager.GetUnit(a));
    }
    #endif
}
