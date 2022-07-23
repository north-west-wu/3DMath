using System;
using UnityEngine;

namespace MyMath
{
    public class Vec3 : IEquatable<Vec3>
    {
        #region 字段

        public float x;
        public float y;
        public float z;
        
        public static readonly Vec3 One = new Vec3(1f, 1f, 1f);
        public static readonly Vec3 Zero = new Vec3(0f, 0f, 0f);
        public static readonly Vec3 Up = new Vec3(0f, -1f, 0f);
        public static readonly Vec3 Down = new Vec3(0f, -1f, 0f);
        public static readonly Vec3 Left = new Vec3(-1f, 0f, 0f);
        public static readonly Vec3 Right = new Vec3(1f, 0f, 0f);
        public static readonly Vec3 Forward = new Vec3(0f, 0f, 1f);
        public static readonly Vec3 Back = new Vec3(0f, -1f, 0f);

        #endregion

        #region 属性

        //向量长度
        public float Magnitude => Mathf.Sqrt(x * x + y * y + z * z);
        
        //长度平方
        public float SqrMagnitude => x * x + y * y + z * z;

        //单位向量
        public Vec3 Normalize
        {
            get
            {
                float magnitude = Magnitude;
                if (magnitude < 1e-5) return new Vec3(0f, 0f, 0f);
                return new Vec3(x / magnitude, y / magnitude, z / magnitude);
            }
        }
        
        //转换为 UnityEngine.Vector3
        public UnityEngine.Vector3 ToVector3 => new Vector3(x, y, z);

        #endregion

        #region 构造函数

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region 运算符
        
        //负矢量
        public static Vec3 operator -(Vec3 v1)
        {
            return new Vec3(-v1.x, -v1.y, -v1.z);
        }
        
        //矢量加法
        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        
        //矢量减法
        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        
        //标量与矢量相乘
        public static Vec3 operator *(float p, Vec3 v1)
        {
            return new Vec3(p * v1.x, p * v1.y, p * v1.z);
        }
        
        public static Vec3 operator *(Vec3 v1, float p)
        {
            return new Vec3(p * v1.x, p * v1.y, p * v1.z);
        }
        
        //标量与矢量相除
        public static Vec3 operator /(Vec3 v1, float p)
        {
            return new Vec3(v1.x / p, v1.y / p, v1.z / p);
        }
        
        //== 与 !=
        public static bool operator !=(Vec3 v1, Vec3 v2)
        {
            return Mathf.Abs(v1.x - v2.x) > 1e-5 ||
                   Mathf.Abs(v1.y - v2.y) > 1e-5 ||
                   Mathf.Abs(v1.z - v2.z) > 1e-5;
        }
        
        public static bool operator ==(Vec3 v1, Vec3 v2)
        {
            return !(v1 != v2);
        }

        public static implicit operator Vec3(UnityEngine.Vector3 v)
        {
            return new Vec3(v.x, v.y, v.z);
        }

        #endregion

        #region 方法

        //求角度
        public static float Angle(Vec3 v1, Vec3 v2)
        {
            Vec3 n1 = v1.Normalize;
            Vec3 n2 = v2.Normalize;
            float val = Mathf.Clamp(Dot(n1, n2), -1f, 1f);
            return Mathf.Acos(val) * Mathf.Rad2Deg;
        }
        
        //求距离
        public static float Distance(Vec3 v1, Vec3 v2)
        {
            return (v1 - v2).Magnitude;
        }
        
        //点积
        public static float Dot(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
        
        //叉积
        public static Vec3 Cross(Vec3 v1, Vec3 v2)
        {
            return new Vec3(
                v1.y * v2.z - v2.y * v1.z,
                v1.z * v2.x - v2.z * v1.x,
                v1.x * v2.y - v2.x * v1.y);
        }
        
        //限定长度
        public static Vec3 ClampMagnitude(Vec3 v1, float maxLength)
        {
            float l = v1.SqrMagnitude;
            if (l > maxLength * maxLength)
            {
                return v1.Normalize * maxLength;
            }

            return v1;
        }
        
        //线性插值
        public static Vec3 Lerp(Vec3 start, Vec3 end, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vec3(
                start.x + (end.x - start.x) * t,
                start.y + (end.y - start.y) * t,    
                start.z + (end.z - start.z) * t);
        }
        
        //反射
        public static Vec3 Refract(Vec3 v1, Vec3 normal)
        {
            return v1 - 2 * Dot(v1, normal) * normal;
        }
        
        //投影的水平矢量
        public static Vec3 HorizontalProject(Vec3 v1, Vec3 normal)
        {
            return normal * Dot(v1, normal);
        }
        
        //投影的垂直矢量
        public static Vec3 VerticalProject(Vec3 v1, Vec3 normal)
        {
            return v1 - HorizontalProject(v1, normal);
        }
        
        //通过经纬获得球面位置
        public static Vec3 FormSphericalPosition(float latitude, float longitude)
        {
            return new Vec3(
                Mathf.Sin(latitude) * Mathf.Cos(longitude),
                Mathf.Sin(longitude),
                Mathf.Cos(latitude) * Mathf.Cos(longitude));
        }
        
        //通过位置获得球面经纬
        public static (float latitude, float longitude) SphericalPosition(Vec3 v1)
        {
            Vec3 v = v1.Normalize;
            return (Mathf.Atan2(v.x, v.z), Mathf.Acos(v.y));
        }
        
        //绕轴旋转
        public static Vec3 RotateAround(Vec3 v, Vec3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis.ToVector3) * v.ToVector3;
        }

        public override string ToString()
        {
            return $"<{x} {y} {z}>";
        }


        public bool Equals(Vec3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vec3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}