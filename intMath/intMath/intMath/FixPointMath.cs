//using UnityEngine;
using System.Collections;
using System;

//namespace FixPoint
//{
    public partial class FixPointMath
    {
        public const double Deg2Rad = 0.0174533d;
        public const double Epsilon = 1.4013e-045d;
        public const double Infinity = 1.0d / 0.0d;
        public const double NegativeInfinity = -1.0d / 0.0d;
        public const double PI = 3.14159d;
        public const double Rad2Deg = 57.2958d;

        public static FloatL Abs(FloatL f)
        {
            FloatL ret = f;
            if (ret.m_numerator < 0)
            {
                ret.m_numerator = -ret.m_numerator;
            }
            return ret;
        }



        public static int CeilToInt(FloatL f)
        {
            return (int)((f.m_numerator + FloatL.m_denominator - 1) / FloatL.m_denominator);
        }

        public static FloatL Clamp(FloatL value, FloatL min, FloatL max)
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

        public static FloatL Clamp01(FloatL value)
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

    public static FloatL Asin(FloatL f)
    {
        //return Math.Asin(f.ToDouble());
        // arcsinX = pi / 2 - arccosX;
        FloatL arccosX = Acos(f);
        return FixPointMath.PI / 2 - arccosX;
    }

    public static FloatL Atan(FloatL f)
    {
        //return Math.Atan(f.ToDouble());
        throw new Exception("不应该使用atan, 考虑用atan2替代");
    }

    public static FloatL Atan2(FloatL yL, FloatL xL)
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

    public static FloatL Sin(FloatL f)
    {
        //int index = SinCosLookupTable.getIndex(f.m_numerator, FloatL.m_denominator);
        //return new FloatL(SinCosLookupTable.sin_table[index]) / new FloatL(SinCosLookupTable.FACTOR);
        FloatL ret = Math.Sin((double)f);
        return ret;
    }

    public static FloatL Cos(FloatL f)
    {
//         int index = SinCosLookupTable.getIndex(f.m_numerator, FloatL.m_denominator);
//         return new FloatL(SinCosLookupTable.cos_table[index]) / new FloatL(SinCosLookupTable.FACTOR);
        FloatL ret = Math.Cos((double)f);
        return ret;
    }

    public static FloatL Acos(FloatL f)
    {
//         int num = (int)FixPointMath.Divide(f.m_numerator * (long)AcosLookupTable.HALF_COUNT, FloatL.m_denominator) + AcosLookupTable.HALF_COUNT;
//         num = FixPointMath.Clamp(num, 0, AcosLookupTable.COUNT);
//         return new FloatL((long)AcosLookupTable.table[num]) / new FloatL(10000);
        FloatL ret = Math.Acos((double)f);
        return ret;
    }

    public static FloatL Tan(FloatL f)
    {
        FloatL sin = Sin(f);
        FloatL cos = Cos(f);
        return sin / cos;
    }

    #endregion

    public static FloatL DeltaAngle(FloatL current, FloatL target)
        {
            FloatL num = FixPointMath.Repeat(target - current, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }
            return num;
        }

        public static FloatL Exp(FloatL power)
        {
            return Math.Exp((double)power);
        }

        public static FloatL Floor(FloatL f)
        {
            //return Math.Floor(f.ToDouble());
            return (int)f;
        }

        public static int FloorToInt(FloatL f)
        {
            return (int)f;
        }

        public static FloatL InverseLerp(FloatL from, FloatL to, FloatL value)
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

        public static FloatL Lerp(FloatL from, FloatL to, FloatL t)
        {
            return from + (to - from) * FixPointMath.Clamp01(t);
        }

        public static FloatL LerpAngle(FloatL a, FloatL b, FloatL t)
        {
            FloatL num = FixPointMath.Repeat(b - a, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }
            return a + num * FixPointMath.Clamp01(t);
        }

        public static FloatL Log(FloatL f)
        {
            return Math.Log((double)f);
        }

        public static FloatL Log(FloatL f, FloatL p)
        {
            return Math.Log((double)f, (double)p);
        }

        public static FloatL Log10(FloatL f)
        {
            return Math.Log10((double)f);
        }

        public static FloatL Max(FloatL a, FloatL b)
        {
            return (a <= b) ? b : a;
        }

    public static FloatL Max(params FloatL[] values)
        {
            int num = values.Length;
            if (num == 0)
            {
                return 0f;
            }
            FloatL num2 = values[0];
            for (int i = 1; i < num; i++)
            {
                if (values[i] > num2)
                {
                    num2 = values[i];
                }
            }
            return num2;
        }

        public static FloatL Min(FloatL a, FloatL b)
        {
            return (a >= b) ? b : a;
        }

    public static int Min(int a, int b)
    {
        return (a >= b) ? b : a;
    }

    public static FloatL Min(params FloatL[] values)
        {
            int num = values.Length;
            if (num == 0)
            {
                return 0f;
            }
            FloatL num2 = values[0];
            for (int i = 1; i < num; i++)
            {
                if (values[i] < num2)
                {
                    num2 = values[i];
                }
            }
            return num2;
        }

        public static FloatL MoveTowards(FloatL current, FloatL target, FloatL maxDelta)
        {
            if (FixPointMath.Abs(target - current) <= maxDelta)
            {
                return target;
            }
            return current + FixPointMath.Sign(target - current) * maxDelta;
        }

        public static FloatL MoveTowardsAngle(FloatL current, FloatL target, FloatL maxDelta)
        {
            target = current + FixPointMath.DeltaAngle(current, target);
            return FixPointMath.MoveTowards(current, target, maxDelta);
        }

        public static FloatL PingPong(FloatL t, FloatL length)
        {
            t = FixPointMath.Repeat(t, length * 2f);
            return length - FixPointMath.Abs(t - length);
        }

        public static FloatL Pow(FloatL f, FloatL p)
        {
            return Math.Pow((double)f, (double)p);
        }

        public static FloatL Repeat(FloatL t, FloatL length)
        {
            return t - FixPointMath.Floor(t / length) * length;
        }

        public static FloatL Round(FloatL f)
        {
            //return Math.Round(f.ToDouble());
            return (f + 0.5d);
        }

        public static int RoundToInt(FloatL f)
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

        public static FloatL Sign(FloatL f)
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
        public static FloatL SqrtOld(FloatL c)
        {
            if(c == 0)
            {
                return 0;
            }

            if (c < new FloatL(0))
            {
                return new FloatL(-1);
            }

            FloatL err = FloatL.Epsilon;
            FloatL t = c;
            int count = 0;
            while (FixPointMath.Abs(t - c / t) > err * t)
            {
                count++;
                t = (c / t + t) / new FloatL(2.0f);
                if(count >= 20)
                {
                    UnityEngine.Debug.LogWarning("FixPoint Sqrt " + c + " current sqrt " + t);
                    break;
                }
            }
            return t;
        }

    public static FloatL Sqrt(FloatL c)
    {
        ulong value = Sqrt64((ulong)(c.m_numerator * FloatL.m_denominator));
        FloatL ret = new FloatL();
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

        public static bool Approximately(FloatL a, FloatL b)
    {
        return a == b;
    }
}
//}
