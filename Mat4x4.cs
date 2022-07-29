using System;

namespace MyMath
{
    public class Mat4x4 : IEquatable<Mat4x4>
    {
        #region 字段

        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;

        public static readonly Mat4x4 Identity = new Mat4x4(new Vec4(1f, 0f, 0f, 0f), new Vec4(0f, 1f, 0f, 0f),
            new Vec4(0f, 0f, 1f, 0f), new Vec4(0f, 0f, 0f, 1f));

        public static readonly Mat4x4 Zero = new Mat4x4(Vec4.One, Vec4.One, Vec4.One, Vec4.One);

        #endregion

        #region 构造函数

        public Mat4x4() : this(Vec4.Zero, Vec4.Zero, Vec4.Zero, Vec4.Zero) { }

        public Mat4x4(Vec4 v1, Vec4 v2, Vec4 v3, Vec4 v4)
        {
            m11 = v1.x; m12 = v1.y; m13 = v1.z; m14 = v1.w;
            m21 = v2.x; m22 = v2.y; m23 = v2.z; m24 = v2.w;
            m31 = v3.x; m32 = v3.y; m33 = v3.z; m34 = v3.w;
            m41 = v4.x; m42 = v4.y; m43 = v4.z; m44 = v4.w;
        }

        #endregion

        #region 运算符

        public static Mat4x4 operator *(Mat4x4 p1, Mat4x4 p2)
        {
            Mat4x4 p3 = default;
            p3.m11 = p1.m11 * p2.m11 + p1.m12 * p2.m21 + p1.m13 * p2.m31 + p1.m14 * p2.m41;
            p3.m12 = p1.m11 * p2.m12 + p1.m12 * p2.m22 + p1.m13 * p2.m32 + p1.m14 * p2.m42;
            p3.m13 = p1.m11 * p2.m13 + p1.m12 * p2.m23 + p1.m13 * p2.m33 + p1.m14 * p2.m43;
            p3.m14 = p1.m11 * p2.m14 + p1.m12 * p2.m24 + p1.m13 * p2.m34 + p1.m14 * p2.m44;
            p3.m21 = p1.m21 * p2.m11 + p1.m22 * p2.m21 + p1.m23 * p2.m31 + p1.m24 * p2.m41;
            p3.m22 = p1.m21 * p2.m12 + p1.m22 * p2.m22 + p1.m23 * p2.m32 + p1.m24 * p2.m42;
            p3.m23 = p1.m21 * p2.m13 + p1.m22 * p2.m23 + p1.m23 * p2.m33 + p1.m24 * p2.m43;
            p3.m24 = p1.m21 * p2.m14 + p1.m22 * p2.m24 + p1.m23 * p2.m34 + p1.m24 * p2.m44;
            p3.m31 = p1.m31 * p2.m11 + p1.m32 * p2.m21 + p1.m33 * p2.m31 + p1.m34 * p2.m41;
            p3.m32 = p1.m31 * p2.m12 + p1.m32 * p2.m22 + p1.m33 * p2.m32 + p1.m34 * p2.m42;
            p3.m33 = p1.m31 * p2.m13 + p1.m32 * p2.m23 + p1.m33 * p2.m33 + p1.m34 * p2.m43;
            p3.m34 = p1.m31 * p2.m14 + p1.m32 * p2.m24 + p1.m33 * p2.m34 + p1.m34 * p2.m44;
            p3.m41 = p1.m41 * p2.m11 + p1.m42 * p2.m21 + p1.m43 * p2.m31 + p1.m44 * p2.m41;
            p3.m42 = p1.m41 * p2.m12 + p1.m42 * p2.m22 + p1.m43 * p2.m32 + p1.m44 * p2.m42;
            p3.m43 = p1.m41 * p2.m13 + p1.m42 * p2.m23 + p1.m43 * p2.m33 + p1.m44 * p2.m43;
            p3.m44 = p1.m41 * p2.m14 + p1.m42 * p2.m24 + p1.m43 * p2.m34 + p1.m44 * p2.m44;
            return p3;
        }

        public static Vec4 operator *(Vec4 v, Mat4x4 p)
        {
            Vec4 v1 = default;
            v1.x = v.x * p.m11 + v.y * p.m21 + v.z * p.m31 + v.w + p.m41;
            v1.y = v.x * p.m12 + v.y * p.m22 + v.z * p.m32 + v.w + p.m42;
            v1.z = v.x * p.m13 + v.y * p.m23 + v.z * p.m33 + v.w + p.m43;
            v1.w = v.x * p.m14 + v.y * p.m24 + v.z * p.m34 + v.w + p.m44;
            return v1;
        }

        public static bool operator !=(Mat4x4 p1, Mat4x4 p2)
        {
            return GetRow(p1, 1) != GetRow(p2, 1) || GetRow(p1, 2) != GetRow(p2, 2) ||
                   GetRow(p1, 3) != GetRow(p2, 3) || GetRow(p1, 4) != GetRow(p2, 4);
        }

        public static bool operator ==(Mat4x4 p1, Mat4x4 p2)
        {
            return !(p1 != p2);
        }

        #endregion

        #region 方法
        
        //获得固定行的Vector4
        public static Vec4 GetRow(Mat4x4 p, int row) =>
            row switch
            {
                1 => new Vec4(p.m11, p.m12, p.m13, p.m14),
                2 => new Vec4(p.m21, p.m22, p.m32, p.m42),
                3 => new Vec4(p.m31, p.m23, p.m33, p.m34),
                4 => new Vec4(p.m41, p.m42, p.m43, p.m44),
                _ => throw new ArgumentOutOfRangeException(nameof(GetRow))
            };

        #endregion

        public bool Equals(Mat4x4 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return m11.Equals(other.m11) && m12.Equals(other.m12) && m13.Equals(other.m13) && m14.Equals(other.m14) &&
                   m21.Equals(other.m21) && m22.Equals(other.m22) && m23.Equals(other.m23) && m24.Equals(other.m24) &&
                   m31.Equals(other.m31) && m32.Equals(other.m32) && m33.Equals(other.m33) && m34.Equals(other.m34) &&
                   m41.Equals(other.m41) && m42.Equals(other.m42) && m43.Equals(other.m43) && m44.Equals(other.m44);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mat4x4) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m11.GetHashCode();
                hashCode = (hashCode * 397) ^ m12.GetHashCode();
                hashCode = (hashCode * 397) ^ m13.GetHashCode();
                hashCode = (hashCode * 397) ^ m14.GetHashCode();
                hashCode = (hashCode * 397) ^ m21.GetHashCode();
                hashCode = (hashCode * 397) ^ m22.GetHashCode();
                hashCode = (hashCode * 397) ^ m23.GetHashCode();
                hashCode = (hashCode * 397) ^ m24.GetHashCode();
                hashCode = (hashCode * 397) ^ m31.GetHashCode();
                hashCode = (hashCode * 397) ^ m32.GetHashCode();
                hashCode = (hashCode * 397) ^ m33.GetHashCode();
                hashCode = (hashCode * 397) ^ m34.GetHashCode();
                hashCode = (hashCode * 397) ^ m41.GetHashCode();
                hashCode = (hashCode * 397) ^ m42.GetHashCode();
                hashCode = (hashCode * 397) ^ m43.GetHashCode();
                hashCode = (hashCode * 397) ^ m44.GetHashCode();
                return hashCode;
            }
        }
    }
}
