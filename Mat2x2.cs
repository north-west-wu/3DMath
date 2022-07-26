using System;
using UnityEngine;

namespace MyMath
{
    public class Mat2x2
    {
        #region 字段

        public float m11, m12;
        public float m21, m22;

        public static readonly Mat2x2 Identity = new Mat2x2(1f, 0f, 0f, 1f);
        public static readonly Mat2x2 Zero = new Mat2x2(0f, 0f, 0f, 0f);

        #endregion

        #region 属性

        public Mat2x2 Transpose => new Mat2x2(m11, m21, m12, m22);

        public float Determinant => m11 * m22 - m12 * m21;
        
        public bool IsOrthogonal
        {
            get
            {
                Vec2 r1 = new Vec2(m11, m12);
                Vec2 r2 = new Vec2(m21, m22);

                return Math.Abs(Vec3.Dot(r1, r1) - 1) < 1e-6 &&
                       Math.Abs(Vec3.Dot(r2, r2) - 1) < 1e-6 &&
                       Math.Abs(Vec3.Dot(r1, r2)) < 1e-6 &&
                       Math.Abs(Vec3.Dot(r2, r1)) < 1e-6;
            }
        }

        #endregion

        #region 构造函数

        public Mat2x2()
        {
            m11 = 0;
            m22 = 0;
            m21 = 0;
            m22 = 0;
        }
        
        public Mat2x2(float m11, float m12, float m21, float m22)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
        }

        public Mat2x2(Vec2 v1, Vec2 v2)
        {
            m11 = v1.x;
            m12 = v1.y;
            m21 = v2.x;
            m22 = v2.y;
        }

        #endregion

        #region 运算符

        public static Mat2x2 operator *(float k, Mat2x2 p1)
        {
            return new Mat2x2(
                k * p1.m11, k * p1.m12,
                k * p1.m21, k * p1.m22);
        }
        
        public static Mat2x2 operator *(Mat2x2 p1, float k)
        {
            return new Mat2x2(
                k * p1.m11, k * p1.m12,
                k * p1.m21, k * p1.m22);
        }

        public static Mat2x2 operator *(Mat2x2 p1, Mat2x2 p2)
        {
            Mat2x2 res = default;
            res.m11 = p1.m11 * p2.m11 + p1.m12 * p2.m21;
            res.m12 = p1.m11 * p2.m12 + p1.m12 * p2.m22;
            res.m21 = p1.m21 * p2.m11 + p1.m22 * p2.m21;
            res.m22 = p1.m21 * p2.m12 + p1.m22 * p2.m22;
            return res;
        }

        public static Vec2 operator *(Vec2 v1, Mat2x2 p1)
        {
            float num1 = v1.x * p1.m11 + v1.y * p1.m21;
            float num2 = v1.x * p1.m12 + v1.y * p1.m22;
            return new Vec2(
                Mathf.Abs(num1) < 1e-6 ? 0 : num1,
                Mathf.Abs(num2) < 1e-6 ? 0 : num2);
        }

        public static Mat2x2 operator +(Mat2x2 p1, Mat2x2 p2)
        {
            return new Mat2x2(p1.m11 + p2.m11, p1.m12 + p2.m12, p1.m21 + p2.m21, p1.m22 + p2.m22);
        }

        public static Mat2x2 operator -(Mat2x2 p1, Mat2x2 p2)
        {
            return new Mat2x2(p1.m11 - p2.m11, p1.m12 - p2.m12, p1.m21 - p2.m21, p1.m22 - p2.m22);
        }

        public static Mat2x2 operator -(Mat2x2 p1)
        {
            return new Mat2x2(-p1.m11, -p1.m12, -p1.m21, -p1.m22);
        }

        public static bool operator !=(Mat2x2 p1, Mat2x2 p2)
        {
            return Mathf.Abs(p1.m11 - p2.m11) < 1e-6 &&
                   Mathf.Abs(p1.m12 - p2.m12) < 1e-6 &&
                   Mathf.Abs(p1.m21 - p2.m21) < 1e-6 &&
                   Mathf.Abs(p1.m22 - p2.m22) < 1e-6;
        }

        public static bool operator ==(Mat2x2 p1, Mat2x2 p2)
        {
            return !(p1 != p2);
        }

        #endregion

        #region 方法
        
        //二维旋转矩阵
        public static Mat2x2 FromRotationAxis(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Mat2x2(
                Mathf.Cos(rad), Mathf.Sin(rad),
                -Mathf.Sin(rad), Mathf.Cos(rad));
        }
        
        //二维缩放矩阵
        public static Mat2x2 FormScale(float k)
        {
            return new Mat2x2(k, 0f, 0f, k);
        }
        
        //反射矩阵, 相对于 Scale矩阵 以 Axis轴 k 取-1
        public static Mat2x2 FormReflectionAxis(Vec2 axis)
        {
            return new Mat2x2(
                1 - 2 * axis.x * axis.x, -2 * axis.x * axis.y,
                -2 * axis.x * axis.y, 1 - 2 * axis.y);
        }
        
        //逆矩阵
        public static Mat2x2 Inverse(Mat2x2 p1)
        {
            float determinant = p1.Determinant;
            if (determinant == 0)
            {
                throw new AggregateException(nameof(Inverse));
            }

            determinant = 1 / determinant;
            return new Mat2x2(
                p1.m22 * determinant, -p1.m12 * determinant,
                -p1.m21 * determinant, p1.m11 * determinant);
        }

        public bool Equals(Mat2x2 other)
        {
            return m11.Equals(other.m11) && m12.Equals(other.m12) && 
                   m21.Equals(other.m21) && m22.Equals(other.m22);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mat2x2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m11.GetHashCode();
                hashCode = (hashCode * 397) ^ m12.GetHashCode();
                hashCode = (hashCode * 397) ^ m21.GetHashCode();
                hashCode = (hashCode * 397) ^ m22.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"<{m11} {m12}>\n<{m21} {m22}>";
        }

        #endregion

    }
}