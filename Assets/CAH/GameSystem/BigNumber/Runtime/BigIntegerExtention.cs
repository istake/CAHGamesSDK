using System.Numerics;

namespace CAH.GameSystem.BigNumber
{
    public static class BigIntegerExtention
    {
        /// <summary>
        /// Big Integer를 소수점 곱셈합니다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mul"></param>
        /// <returns></returns>
        public static BigInteger Mul(BigInteger value, double mul)
        {
            double n = (double) value;
            double result = n * mul;
            return new BigInteger(result); 
        }

        public static BigInteger Mul(BigInteger value, BigInteger value2)
        {
            return BigInteger.Multiply(value, value2);
        }
        
        
    }
}