using System;
using UnityEngine;

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

        #region 属性

        public Vec3 Translate => new Vec3(m41, m42, m43);
        
        //TODO:获得四元素

        public float Determinant
        {
            get
            {
                float num1 = m33 * m44 - m34 * m43;
                float num2 = m32 * m44 - m34 * m42;
                float num3 = m32 * m43 - m33 * m42;
                float num4 = m31 * m44 - m34 * m41;
                float num5 = m31 * m43 - m33 * m41;
                float num6 = m31 * m42 - m32 * m41;

                return m11 * (m22 * num1 - m23 * num2 + m24 * num3) -
                       m12 * (m21 * num1 - m23 * num4 + m24 * num5) +
                       m13 * (m21 * num2 - m22 * num4 + m24 * num6) -
                       m14 * (m21 * num3 - m22 * num5 + m23 * num6);
            }
        }

        public Mat4x4 Transpose =>
            new Mat4x4(
                new Vec4(m11, m21, m31, m41),
                new Vec4(m12, m22, m32, m42),
                new Vec4(m13, m23, m33, m43),
                new Vec4(m14, m24, m34, m44));

        public Mat4x4 Invert
        {
            get
            {
                float determinant = 1 / Determinant;

                Vec4 x = new Vec4(m11, m12, m13, m14);
                Vec4 y = new Vec4(m21, m22, m23, m24);
                Vec4 z = new Vec4(m31, m32, m33, m34);
                Vec4 w = new Vec4(m41, m42, m43, m44);
                
                var mat = new Mat4x4
			    (
				    new Vec4
				    (
					    ((y.y * z.z * w.w) + (y.z * z.w * w.y) + (y.w * z.y * w.z) - (y.y * z.w * w.z) - (y.z * z.y * w.w) - (y.w * z.z * w.y)) * determinant,
					    ((x.y * z.w * w.z) + (x.z * z.y * w.w) + (x.w * z.z * w.y) - (x.y * z.z * w.w) - (x.z * z.w * w.y) - (x.w * z.y * w.z)) * determinant,
					    ((x.y * y.z * w.w) + (x.z * y.w * w.y) + (x.w * y.y * w.z) - (x.y * y.w * w.z) - (x.z * y.y * w.w) - (x.w * y.z * w.y)) * determinant,
					    ((x.y * y.w * z.z) + (x.z * y.y * z.w) + (x.w * y.z * z.y) - (x.y * y.z * z.w) - (x.z * y.w * z.y) - (x.w * y.y * z.z)) * determinant
				    ),
				    new Vec4
				    (
					    ((y.x * z.w * w.z) + (y.z * z.x * w.w) + (y.w * z.z * w.x) - (y.x * z.z * w.w) - (y.z * z.w * w.x) - (y.w * z.x * w.z)) * determinant,
					    ((x.x * z.z * w.w) + (x.z * z.w * w.x) + (x.w * z.x * w.z) - (x.x * z.w * w.z) - (x.z * z.x * w.w) - (x.w * z.z * w.x)) * determinant,
					    ((x.x * y.w * w.z) + (x.z * y.x * w.w) + (x.w * y.z * w.x) - (x.x * y.z * w.w) - (x.z * y.w * w.x) - (x.w * y.x * w.z)) * determinant,
					    ((x.x * y.z * z.w) + (x.z * y.w * z.x) + (x.w * y.x * z.z) - (x.x * y.w * z.z) - (x.z * y.x * z.w) - (x.w * y.z * z.x)) * determinant
				    ),
				    new Vec4
				    (
					    ((y.x * z.y * w.w) + (y.y * z.w * w.x) + (y.w * z.x * w.y) - (y.x * z.w * w.y) - (y.y * z.x * w.w) - (y.w * z.y * w.x)) * determinant,
					    ((x.x * z.w * w.y) + (x.y * z.x * w.w) + (x.w * z.y * w.x) - (x.x * z.y * w.w) - (x.y * z.w * w.x) - (x.w * z.x * w.y)) * determinant,
					    ((x.x * y.y * w.w) + (x.y * y.w * w.x) + (x.w * y.x * w.y) - (x.x * y.w * w.y) - (x.y * y.x * w.w) - (x.w * y.y * w.x)) * determinant,
					    ((x.x * y.w * z.y) + (x.y * y.x * z.w) + (x.w * y.y * z.x) - (x.x * y.y * z.w) - (x.y * y.w * z.x) - (x.w * y.x * z.y)) * determinant
				    ),
				    new Vec4
				    (
					    ((y.x * z.z * w.y) + (y.y * z.x * w.z) + (y.z * z.y * w.x) - (y.x * z.y * w.z) - (y.y * z.z * w.x) - (y.z * z.x * w.y)) * determinant,
					    ((x.x * z.y * w.z) + (x.y * z.z * w.x) + (x.z * z.x * w.y) - (x.x * z.z * w.y) - (x.y * z.x * w.z) - (x.z * z.y * w.x)) * determinant,
					    ((x.x * y.z * w.y) + (x.y * y.x * w.z) + (x.z * y.y * w.x) - (x.x * y.y * w.z) - (x.y * y.z * w.x) - (x.z * y.x * w.y)) * determinant,
					    ((x.x * y.y * z.z) + (x.y * y.z * z.x) + (x.z * y.x * z.y) - (x.x * y.z * z.y) - (x.y * y.x * z.z) - (x.z * y.y * z.x)) * determinant
				    )
			    );

                return mat.Transpose;
            }
        }

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
            Mat4x4 p3 = new Mat4x4
            {
                m11 = p1.m11 * p2.m11 + p1.m12 * p2.m21 + p1.m13 * p2.m31 + p1.m14 * p2.m41,
                m12 = p1.m11 * p2.m12 + p1.m12 * p2.m22 + p1.m13 * p2.m32 + p1.m14 * p2.m42,
                m13 = p1.m11 * p2.m13 + p1.m12 * p2.m23 + p1.m13 * p2.m33 + p1.m14 * p2.m43,
                m14 = p1.m11 * p2.m14 + p1.m12 * p2.m24 + p1.m13 * p2.m34 + p1.m14 * p2.m44,
                m21 = p1.m21 * p2.m11 + p1.m22 * p2.m21 + p1.m23 * p2.m31 + p1.m24 * p2.m41,
                m22 = p1.m21 * p2.m12 + p1.m22 * p2.m22 + p1.m23 * p2.m32 + p1.m24 * p2.m42,
                m23 = p1.m21 * p2.m13 + p1.m22 * p2.m23 + p1.m23 * p2.m33 + p1.m24 * p2.m43,
                m24 = p1.m21 * p2.m14 + p1.m22 * p2.m24 + p1.m23 * p2.m34 + p1.m24 * p2.m44,
                m31 = p1.m31 * p2.m11 + p1.m32 * p2.m21 + p1.m33 * p2.m31 + p1.m34 * p2.m41,
                m32 = p1.m31 * p2.m12 + p1.m32 * p2.m22 + p1.m33 * p2.m32 + p1.m34 * p2.m42,
                m33 = p1.m31 * p2.m13 + p1.m32 * p2.m23 + p1.m33 * p2.m33 + p1.m34 * p2.m43,
                m34 = p1.m31 * p2.m14 + p1.m32 * p2.m24 + p1.m33 * p2.m34 + p1.m34 * p2.m44,
                m41 = p1.m41 * p2.m11 + p1.m42 * p2.m21 + p1.m43 * p2.m31 + p1.m44 * p2.m41,
                m42 = p1.m41 * p2.m12 + p1.m42 * p2.m22 + p1.m43 * p2.m32 + p1.m44 * p2.m42,
                m43 = p1.m41 * p2.m13 + p1.m42 * p2.m23 + p1.m43 * p2.m33 + p1.m44 * p2.m43,
                m44 = p1.m41 * p2.m14 + p1.m42 * p2.m24 + p1.m43 * p2.m34 + p1.m44 * p2.m44
            };
            return p3;
        }

        public static Vec4 operator *(Vec4 v, Mat4x4 p)
        {
            Vec4 v1 = new Vec4
            {
                x = v.x * p.m11 + v.y * p.m21 + v.z * p.m31 + v.w * p.m41,
                y = v.x * p.m12 + v.y * p.m22 + v.z * p.m32 + v.w * p.m42,
                z = v.x * p.m13 + v.y * p.m23 + v.z * p.m33 + v.w * p.m43,
                w = v.x * p.m14 + v.y * p.m24 + v.z * p.m34 + v.w * p.m44
            };

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
        
        //TODO: 获得四元数，来设置4x4矩阵
        
        //TODO: 获得四元数与平移量，来设置4x4矩阵
        
        //设置旋转矩阵（通过3x3矩阵）
        public static Mat4x4 FormRotate(Mat3x3 p)
        {
            return new Mat4x4(
                new Vec4(p.m11, p.m12, p.m13, 0f),
                new Vec4(p.m21, p.m22, p.m23, 0f),
                new Vec4(p.m31, p.m32, p.m33, 0f),
                new Vec4(0f, 0f, 0f, 1f));
        }
        
        //设置仿射矩阵（通过3x3矩阵）
        public static Mat4x4 FromAffineTransform(Mat3x3 p, Vec3 v)
        {
            return new Mat4x4(
                new Vec4(p.m11, p.m12, p.m13, 0f),
                new Vec4(p.m21, p.m22, p.m23, 0f),
                new Vec4(p.m31, p.m32, p.m33, 0f),
                new Vec4(v.x, v.y, v.z, 1f));
        }

        //设置平移矩阵
        public static Mat4x4 FormTranslate(Vec3 v)
        {
            return new Mat4x4(
                new Vec4(1f, 0f, 0f, 0f),
                new Vec4(0f, 1f, 0f, 0f),
                new Vec4(0f, 0f, 1f, 0f),
                new Vec4(v.x, v.y, v.z, 1));
        }
	    
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
        
        #endregion


    }
}
