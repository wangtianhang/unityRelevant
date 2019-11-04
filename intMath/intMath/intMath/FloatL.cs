using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

//namespace FixPoint
//{

    [System.Serializable]
    public struct FloatL
    {
        public static FloatL MaxValue = new FloatL(Int32.MaxValue / m_denominator);
        public static FloatL MinValue = new FloatL(Int32.MinValue / m_denominator);
        public static FloatL Epsilon = new FloatL((double)1 / m_denominator);
        public int m_numerator; // 分子
        public const int m_denominator = 1000;

        public static double m_denominatorDouble = m_denominator;
        public static float m_denominatorFloat = m_denominator;
        //public const long m_maybeOverflow = m_denominator * 10000;


        public FloatL(float a)
        {
            m_numerator = (int)FixPointMath.RoundToLong(a * m_denominator);
        }

        public FloatL(double a)
        {
            m_numerator = FixPointMath.RoundToInt(a * m_denominator);
        }

        public FloatL(int a)
        {
            m_numerator = a * m_denominator;
        }

        public static implicit operator FloatL(int n)
        {
            FloatL ret = new FloatL(n);
            return ret;
        }

        public static implicit operator FloatL(float n)
        {
            FloatL ret = new FloatL(n);
            return ret;
        }

        public static implicit operator FloatL(double n)
        {
            FloatL ret = new FloatL(n);
            return ret;
        }

        public static FloatL operator -(FloatL a)
        {
            FloatL ret = new FloatL();
            ret.m_numerator = -a.m_numerator;
            return ret;
        }

    public static FloatL operator +(FloatL a)
    {
        //FloatL ret = new FloatL();
        //ret.m_numerator = -a.m_numerator;
        return a;
    }

    public static bool operator !=(FloatL lhs, FloatL rhs)
        {
            return lhs.m_numerator != rhs.m_numerator;
        }

        public static bool operator ==(FloatL lhs, FloatL rhs)
        {
            return lhs.m_numerator == rhs.m_numerator;
        }

        public static bool operator > (FloatL lhs, FloatL rhs)
        {
            return lhs.m_numerator > rhs.m_numerator;
        }

        public static bool operator <(FloatL lhs, FloatL rhs)
        {
            return lhs.m_numerator < rhs.m_numerator;
        }

        public static bool operator >= (FloatL lhs, FloatL rhs)
        {
            return lhs.m_numerator >= rhs.m_numerator;
        }

        public static bool operator <=(FloatL lhs, FloatL rhs)
        {
            return lhs.m_numerator <= rhs.m_numerator;
        }

        public static FloatL operator %(FloatL lhs, FloatL rhs)
        {
            FloatL ret = new FloatL();
            ret.m_numerator = lhs.m_numerator % rhs.m_numerator;
            return ret;
        }

        public static FloatL operator +(FloatL a, FloatL b)
        {
            FloatL ret = new FloatL();
            ret.m_numerator = a.m_numerator + b.m_numerator;
            return ret;
        }

        public static FloatL operator -(FloatL a, FloatL b)
        {
            FloatL ret = new FloatL();
            ret.m_numerator = a.m_numerator - b.m_numerator;
            return ret;
        }

        public static FloatL operator *(FloatL a, FloatL b)
        {
            
//         if (a.m_numerator > m_maybeOverflow
//             && b.m_numerator > m_maybeOverflow)
//         {
//             FloatLow ret = new FloatLow();
//             FloatLow tmpA = a.ToFloatLow();
//             FloatLow tmpB = b.ToFloatLow();
//             ret = tmpA * tmpB;
//             return ret;
//         }
//         else if (a.m_numerator > m_maybeOverflow)
//         {
//             FloatLow tmpA = a.ToFloatLow();
//             FloatL ret = new FloatL();
//             ret.m_numerator = tmpA.m_numerator * b.m_numerator / FloatLow.m_denominator;
//             return ret;
//         }
//         else if (b.m_numerator > m_maybeOverflow)
//         {
//             FloatLow tmpB = b.ToFloatLow();
//             FloatL ret = new FloatL();
//             ret.m_numerator = a.m_numerator * tmpB.m_numerator / FloatLow.m_denominator;
//             return ret;
//         }
//         else
//         {
//             FloatL ret = new FloatL();
//             ret.m_numerator = ((a.m_numerator * b.m_numerator) / m_denominator);
//             return ret;
//         }
            long tmp = a.m_numerator * b.m_numerator / m_denominator;
            FloatL ret = new FloatL();
            ret.m_numerator = (int)tmp;
            return ret;
        }

        public static FloatL operator /(FloatL a, FloatL b)
        {
            FloatL ret = new FloatL();
            
            if(b.m_numerator == 0)
            {
                UnityEngine.Debug.LogError("FloatL / 0");
                if(a.m_numerator >= 0)
                {
                    return MaxValue;
                }
                else if(a.m_numerator < 0)
                {
                    return MinValue;
                }
    //             else
    //             {
    //                 return 0;
    //             }
            }

        // 可以应对小数除大数时不为0
            long tmp = (a.m_numerator * m_denominator) / b.m_numerator;
            ret.m_numerator = (int)tmp;
            return ret;
        }

//         public float ToFloat()
//         {
//             // todo 据说各平台double降级为float会比较一致 有待实际测试
//             return (float)m_numerator / m_denominatorFloat;
//         }
// 
//         public double ToDouble()
//         {
//             return (double)m_numerator / m_denominatorDouble;
//         }

//         public int ToInt()
//         {
//             return (int)(m_numerator / m_denominator);
//         }

        public override string ToString()
        {
            double ret = (double)m_numerator / m_denominator;
            return ret.ToString("f6");
        }

        public override int GetHashCode()
        {
            return m_numerator.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (!(other is FloatL))
            {
                return false;
            }
            FloatL otherFloatL = (FloatL)other;
            return this.m_numerator == otherFloatL.m_numerator;
        }

    public static explicit operator float(FloatL a)
    {
        // todo 据说各平台double降级为float会比较一致 有待实际测试
        return (float)a.m_numerator / m_denominatorFloat;
    }

    public static explicit operator double(FloatL a)
    {
        // todo 据说各平台double降级为float会比较一致 有待实际测试
        return (double)a.m_numerator / m_denominatorDouble;
    }

    public static explicit operator int(FloatL a)
    {
        return (int)(a.m_numerator / m_denominator);
    }
}
//}
