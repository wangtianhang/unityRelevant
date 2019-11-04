//using UnityEngine;
using System.Collections;
using System;

//namespace FixPoint
//{
    public partial class FloatIMath
    {
        public const double Deg2Rad = 0.0174533d;
        public const double Epsilon = 1.4013e-045d;
        public const double Infinity = 1.0d / 0.0d;
        public const double NegativeInfinity = -1.0d / 0.0d;
        public const double PI = 3.14159d;
        public const double Rad2Deg = 57.2958d;

        public static FloatI Abs(FloatI f)
        {
            FloatI ret = f;
            if (ret.m_numerator < 0)
            {
                ret.m_numerator = -ret.m_numerator;
            }
            return ret;
        }



        public static int CeilToInt(FloatI f)
        {
            return (int)((f.m_numerator + FloatI.m_denominator - 1) / FloatI.m_denominator);
        }

        public static FloatI Clamp(FloatI value, FloatI min, FloatI max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
            return value;
        }

        public static int Clamp(int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
        return value;
    }

        public static FloatI Clamp01(FloatI value)
        {
            if (value < 0)
            {
                return 0;
            }
            if (value > 1)
            {
                return 1;
            }
            return value;
        }

    #region 三角函数

    public static FloatI Asin(FloatI f)
    {
        //return Math.Asin(f.ToDouble());
        // arcsinX = pi / 2 - arccosX;
        FloatI arccosX = Acos(f);
        return FloatIMath.PI / 2 - arccosX;
    }

    public static FloatI Atan(FloatI f)
    {
        //return Math.Atan(f.ToDouble());
        throw new Exception("不应该使用atan, 考虑用atan2替代");
    }

    public static FloatI Atan2(FloatI yL, FloatI xL)
    {
//         //return Math.Atan2(y.ToDouble(), x.ToDouble());
//         long y = yL.m_numerator;
//         long x = xL.m_numerator;
// 
//         int num;
//         int num2;
//         if (x < 0)
//         {
//             if (y < 0)
//             {
//                 x = -x;
//                 y = -y;
//                 num = 1;
//             }
//             else
//             {
//                 x = -x;
//                 num = -1;
//             }
//             num2 = -31416;
//         }
//         else
//         {
//             if (y < 0)
//             {
//                 y = -y;
//                 num = -1;
//             }
//             else
//             {
//                 num = 1;
//             }
//             num2 = 0;
//         }
//         int dIM = Atan2LookupTable.DIM;
//         long num3 = (long)(dIM - 1);
//         long b = (long)((x >= y) ? x : y);
//         int num4 = (int)FixPointMath.Divide((long)x * num3, b);
//         int num5 = (int)FixPointMath.Divide((long)y * num3, b);
//         int num6 = Atan2LookupTable.table[num5 * dIM + num4];
//         return new FloatL((num6 + num2) * num) / new FloatL(10000);
        return Math.Atan2((double)yL, (double)xL);
    }

    public static FloatI Sin(FloatI f)
    {
        //int index = SinCosLookupTable.getIndex(f.m_numerator, FloatL.m_denominator);
        //return new FloatL(SinCosLookupTable.sin_table[index]) / new FloatL(SinCosLookupTable.FACTOR);
        FloatI ret = Math.Sin((double)f);
        return ret;
    }

    public static FloatI Cos(FloatI f)
    {
//         int index = SinCosLookupTable.getIndex(f.m_numerator, FloatL.m_denominator);
//         return new FloatL(SinCosLookupTable.cos_table[index]) / new FloatL(SinCosLookupTable.FACTOR);
        FloatI ret = Math.Cos((double)f);
        return ret;
    }

    public static FloatI Acos(FloatI f)
    {
//         int num = (int)FixPointMath.Divide(f.m_numerator * (long)AcosLookupTable.HALF_COUNT, FloatL.m_denominator) + AcosLookupTable.HALF_COUNT;
//         num = FixPointMath.Clamp(num, 0, AcosLookupTable.COUNT);
//         return new FloatL((long)AcosLookupTable.table[num]) / new FloatL(10000);
        FloatI ret = Math.Acos((double)f);
        return ret;
    }

    public static FloatI Tan(FloatI f)
    {
        FloatI sin = Sin(f);
        FloatI cos = Cos(f);
        return sin / cos;
    }

    #endregion

    public static FloatI DeltaAngle(FloatI current, FloatI target)
        {
            FloatI num = FloatIMath.Repeat(target - current, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }
            return num;
        }

        public static FloatI Exp(FloatI power)
        {
            return Math.Exp((double)power);
        }

        public static FloatI Floor(FloatI f)
        {
            //return Math.Floor(f.ToDouble());
            return (int)f;
        }

        public static int FloorToInt(FloatI f)
        {
            return (int)f;
        }

        public static FloatI InverseLerp(FloatI from, FloatI to, FloatI value)
        {
            if (from < to)
            {
                if (value < from)
                {
                    return 0f;
                }
                if (value > to)
                {
                    return 1f;
                }
                value -= from;
                value /= to - from;
                return value;
            }
            else
            {
                if (from <= to)
                {
                    return 0f;
                }
                if (value < to)
                {
                    return 1f;
                }
                if (value > from)
                {
                    return 0f;
                }
                return 1f - (value - to) / (from - to);
            }
        }

        public static FloatI Lerp(FloatI from, FloatI to, FloatI t)
        {
            return from + (to - from) * FloatIMath.Clamp01(t);
        }

        public static FloatI LerpAngle(FloatI a, FloatI b, FloatI t)
        {
            FloatI num = FloatIMath.Repeat(b - a, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }
            return a + num * FloatIMath.Clamp01(t);
        }

        public static FloatI Log(FloatI f)
        {
            return Math.Log((double)f);
        }

        public static FloatI Log(FloatI f, FloatI p)
        {
            return Math.Log((double)f, (double)p);
        }

        public static FloatI Log10(FloatI f)
        {
            return Math.Log10((double)f);
        }

        public static FloatI Max(FloatI a, FloatI b)
        {
            return (a <= b) ? b : a;
        }

    public static FloatI Max(params FloatI[] values)
        {
            int num = values.Length;
            if (num == 0)
            {
                return 0f;
            }
            FloatI num2 = values[0];
            for (int i = 1; i < num; i++)
            {
                if (values[i] > num2)
                {
                    num2 = values[i];
                }
            }
            return num2;
        }

        public static FloatI Min(FloatI a, FloatI b)
        {
            return (a >= b) ? b : a;
        }

    public static int Min(int a, int b)
    {
        return (a >= b) ? b : a;
    }

    public static FloatI Min(params FloatI[] values)
        {
            int num = values.Length;
            if (num == 0)
            {
                return 0f;
            }
            FloatI num2 = values[0];
            for (int i = 1; i < num; i++)
            {
                if (values[i] < num2)
                {
                    num2 = values[i];
                }
            }
            return num2;
        }

        public static FloatI MoveTowards(FloatI current, FloatI target, FloatI maxDelta)
        {
            if (FloatIMath.Abs(target - current) <= maxDelta)
            {
                return target;
            }
            return current + FloatIMath.Sign(target - current) * maxDelta;
        }

        public static FloatI MoveTowardsAngle(FloatI current, FloatI target, FloatI maxDelta)
        {
            target = current + FloatIMath.DeltaAngle(current, target);
            return FloatIMath.MoveTowards(current, target, maxDelta);
        }

        public static FloatI PingPong(FloatI t, FloatI length)
        {
            t = FloatIMath.Repeat(t, length * 2f);
            return length - FloatIMath.Abs(t - length);
        }

        public static FloatI Pow(FloatI f, FloatI p)
        {
            return Math.Pow((double)f, (double)p);
        }

        public static FloatI Repeat(FloatI t, FloatI length)
        {
            return t - FloatIMath.Floor(t / length) * length;
        }

        public static FloatI Round(FloatI f)
        {
            //return Math.Round(f.ToDouble());
            return (f + 0.5d);
        }

        public static int RoundToInt(FloatI f)
        {
            //return (int)Math.Round(f.ToDouble());
            return (int)(f + 0.5d);
        }

        public static int RoundToInt(double d)
        {
            return (int)(d + 0.5f);
        }

        public static long RoundToLong(float t)
        {
            return (long)(t + 0.5f);
        }

        public static FloatI Sign(FloatI f)
        {
            return (f < 0f) ? -1f : 1f;
        }



//         public static FloatL SinOld(FloatL f)
//         {
//             return Math.Sin(f.ToDouble());
//         }
        /// <summary>
        /// 牛顿法求平方根
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FloatI SqrtOld(FloatI c)
        {
            if(c == 0)
            {
                return 0;
            }

            if (c < new FloatI(0))
            {
                return new FloatI(-1);
            }

            FloatI err = FloatI.Epsilon;
            FloatI t = c;
            int count = 0;
            while (FloatIMath.Abs(t - c / t) > err * t)
            {
                count++;
                t = (c / t + t) / new FloatI(2.0f);
                if(count >= 20)
                {
                    UnityEngine.Debug.LogWarning("FixPoint Sqrt " + c + " current sqrt " + t);
                    break;
                }
            }
            return t;
        }

    public static FloatI Sqrt(FloatI c)
    {
        ulong value = Sqrt64((ulong)(c.m_numerator * FloatI.m_denominator));
        FloatI ret = new FloatI();
        ret.m_numerator = (int)value;
        return ret;
    }

    public static ulong Sqrt64(ulong a)
    {
        ulong num = 0uL;
        ulong num2 = 0uL;
        for (int i = 0; i < 32; i++)
        {
            num2 <<= 1;
            num <<= 2;
            num += a >> 62;
            a <<= 2;
            if (num2 < num)
            {
                num2 += 1uL;
                num -= num2;
                num2 += 1uL;
            }
        }
        return num2 >> 1 & long.MaxValue;
    }

        public static int ClosestPowerOfTwo(int n)
        {
            int v = n;
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;

            int x = v >> 1;
            return (v - n) > (n - x) ? x : v;
        }

        public static bool IsPowerOfTwo(int n)
        {
            if (n <= 0)
            {
                return false;
            }
            else
            {
                return (n & (n - 1)) == 0;
            }
        }

        public static bool Approximately(FloatI a, FloatI b)
    {
        return a == b;
    }
}
//}
