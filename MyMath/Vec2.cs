using System;
using UnityEngine;

namespace MyMath
{
    public class Vec2 : IEquatable<Vec2>
    {
        #region 字段

        public float x;
        public float y;

        public static readonly Vec2 Zero = new Vec2(0f, 0f);
        public static readonly Vec2 One = new Vec2(1f, 1f);
        public static readonly Vec2 Up = new Vec2(0f, 1f);
        public static readonly Vec2 Down = new Vec2(0f, -1f);
        public static readonly Vec2 Left = new Vec2(-1f, 0f);
        public static readonly Vec2 Right = new Vec2(1f, 0f);

        #endregion

        #region 属性

        public float Magnitude => Mathf.Sqrt(x * x + y * y);

        public float SqrMagnitude => x * x + y * y;
        
        public Vec2 Normalized
        {
            get
            {
                float m = Magnitude;
                if (m < 1e-5) return new Vec2(0f, 0f);
                return new Vec2(x / m, y / m);
            }
        }

        public UnityEngine.Vector2 ToVector2 => new Vector2(x, y);

        #endregion

        #region 构造函数

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region 运算符

        public static Vec2 operator -(Vec2 v)
        {
            return new Vec2(-v.x, -v.y);
        }

        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vec2 operator *(float p, Vec2 v)
        {
            return new Vec2(p * v.x, p * v.y);
        }
        
        public static Vec2 operator *(Vec2 v, float p)
        {
            return new Vec2(p * v.x, p * v.y);
        }

        public static Vec2 operator /(Vec2 v, float p)
        {
            return new Vec2(v.x / p, v.y / p);
        }

        public static bool operator !=(Vec2 v1, Vec2 v2)
        {
            return Mathf.Abs(v1.x - v2.x) > 1e-5 ||
                   Mathf.Abs(v1.y - v2.y) > 1e-5;
        }

        public static bool operator ==(Vec2 v1, Vec2 v2)
        {
            return !(v1 != v2);
        }

        public static implicit operator Vec2(Vec3 v)
        {
            return new Vec2(v.x, v.y);
        }

        public static implicit operator Vec3(Vec2 v)
        {
            return new Vec3(v.x, v.y, 0f);
        }
        
        public static implicit operator Vec2(UnityEngine.Vector2 v)
        {
            return new Vec2(v.x, v.y);
        }
        #endregion

        #region 方法

        public static float Angle(Vec2 v1, Vec2 v2)
        {
            Vec2 n1 = v1.Normalized;
            Vec2 n2 = v2.Normalized;
            float val = Mathf.Clamp(Dot(n1, n2), -1f, 1f);
            return Mathf.Acos(val) * Mathf.Rad2Deg;
        }

        public static float Distance(Vec2 v1, Vec2 v2)
        {
            return (v1 - v2).Magnitude;
        }

        public static Vec2 ClampMagnitude(Vec2 v, float maxLength)
        {
            float l = v.SqrMagnitude;
            if (l > maxLength * maxLength)
            {
                return v.Normalized * maxLength;
            }

            return v;
        }

        public static Vec2 Lerp(Vec2 start, Vec2 end, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vec2((end.x - start.x) * t + start.x, (end.y - start.y) * t + start.y);
        }

        public static float Dot(Vec2 v1, Vec2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        public static float Cross(Vec2 v1, Vec2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        public static Vec2 Reflect(Vec2 v, Vec2 normal)
        {
            return v - 2 * Dot(v, normal) * normal;
        }

        public static Vec2 FormCircularPosition(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vec2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        public static float FormCircularAngle(Vec2 v)
        {
            Vec2 normal = v.Normalized;
            return Mathf.Acos(v.x) * Mathf.Rad2Deg;
        }
        

        public bool Equals(Vec2 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vec2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"<{x} {y}>";
        }

        #endregion
    }
}