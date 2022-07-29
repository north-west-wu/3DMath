using System;
using UnityEngine;

namespace MyMath
{
    public class Vec4 : IEquatable<Vec4>
    {
        #region 字段

        public float x;
        public float y;
        public float z;
        public float w;

        public static readonly Vec4 Zero = new Vec4(0f, 0f, 0f, 0f);
        public static readonly Vec4 One = new Vec4(1f, 1f, 1f, 1f);

        #endregion

        #region 属性
    
        //长度
        public float Magnitude => Mathf.Sqrt(Dot(this, this));
    
        //平方长度
        public float SqtMagnitude => Dot(this, this);
    
        //单位方向
        public Vec4 Normalized
        {
            get
            {
                float magnitude = Magnitude;
                if (magnitude < 1e-5)
                    return Vec4.Zero;
                return this / magnitude;
            }
        }
    
        //转换为 UnityEngine.Vector4
        public Vector4 ToVector4 => new Vector4(x, y, z, w);

        #endregion

        #region 构造函数

        public Vec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vec4(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = 0f;
        }

        #endregion

        #region 运算符
    
        //负号
        public static Vec4 operator -(Vec4 v)
        {
            return new Vec4(-v.x, -v.y, -v.z, -v.w);
        }
    
        //加法
        public static Vec4 operator +(Vec4 v1, Vec4 v2)
        {
            return new Vec4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }
    
        //减法
        public static Vec4 operator -(Vec4 v1, Vec4 v2)
        {
            return new Vec4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }
    
        //乘法
        public static Vec4 operator *(Vec4 v1, float k)
        {
            return new Vec4(v1.x * k, v1.y * k, v1.z * k, v1.w * k);
        }
    
        public static Vec4 operator *(float k, Vec4 v1)
        {
            return new Vec4(v1.x * k, v1.y * k, v1.z * k, v1.w * k);
        }
    
        //除法
        public static Vec4 operator /(Vec4 v1, float k)
        {
            return new Vec4(v1.x / k, v1.y / k, v1.z / k, v1.w / k);
        }
    
        public static Vec4 operator /(float k, Vec4 v1)
        {
            return new Vec4(v1.x / k, v1.y / k, v1.z / k, v1.w / k);
        }

        public static bool operator !=(Vec4 v1, Vec4 v2)
        {
            return Mathf.Abs(v1.x - v2.x) > 1e-6 ||
                   Mathf.Abs(v1.y - v2.y) > 1e-6 ||
                   Mathf.Abs(v1.z - v2.z) > 1e-6 ||
                   Mathf.Abs(v1.w - v2.w) > 1e-6;
        }

        public static bool operator ==(Vec4 v1, Vec4 v2)
        {
            return !(v1 != v2);
        }
    
        public static implicit operator Vec4(UnityEngine.Vector4 v)
        {
            return new Vec4(v.x, v.y, v.z, v.w);
        }

        #endregion

        #region 方法

        public static float Dot(Vec4 v1, Vec4 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;
        }

        public static float Distance(Vec4 v1, Vec4 v2)
        {
            return (v1 - v2).Magnitude;
        }

        public static Vec4 Lerp(Vec4 v1, Vec4 v2, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vec4(v1.x + (v2.x - v1.x) * t, v1.y + (v2.y - v1.y) * t, v1.z + (v2.z - v1.z) * t,
                v1.w + (v2.w - v1.w) * t);
        }
    
        public override string ToString()
        {
            return $"<{x} {y} {z} {w}>";
        }

        public bool Equals(Vec4 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vec4) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }
    
        #endregion
    }
}
