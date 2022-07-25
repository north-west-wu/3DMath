using System;
using System.Data.SqlTypes;
using UnityEngine;

namespace MyMath
{
    public class Mat3x3 : IEquatable<Mat3x3>
    {
        #region 字段

        public float m11, m12, m13;
        public float m21, m22, m23;
        public float m31, m32, m33;
        
        //零矩阵
        public static readonly Mat3x3 Zero = new Mat3x3(Vec3.Zero, Vec3.Zero, Vec3.Zero);
        //单位矩阵
        public static readonly Mat3x3 Identity = new Mat3x3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        #endregion

        #region 属性
        
        //转置矩阵
        public Mat3x3 Transpose
        {
            get => new Mat3x3(
                m11, m21, m31,
                m12, m22, m32,
                m13, m23, m33);
        }
        
        //行列式
        public float Determinant =>
            m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32
            - m13 * m22 * m31 - m12 * m21 * m33 - m11 * m23 * m32;

        #endregion

        #region 构造函数

        public Mat3x3()
        {
            m11 = 0f; m12 = 0f; m13 = 0f;
            m21 = 0f; m22 = 0f; m23 = 0f;
            m31 = 0f; m32 = 0f; m33 = 0f;
        }

        public Mat3x3(float m11, float m12, float m13, 
            float m21, float m22, float m23, 
            float m31, float m32, float m33)
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }

        public Mat3x3(Vec3 v1, Vec3 v2, Vec3 v3)
        {
            m11 = v1.x; m12 = v1.y; m13 = v1.z;
            m21 = v2.x; m22 = v2.y; m23 = v2.z;
            m31 = v3.x; m32 = v3.y; m33 = v3.z;
        }

        #endregion

        #region 运算符
        
        //矩阵乘以标量
        public static Mat3x3 operator *(float k, Mat3x3 p1)
        {
            return new Mat3x3(p1.m11 * k, p1.m12 * k, p1.m13 * k,
                p1.m21 * k, p1.m22 * k, p1.m23 * k,
                p1.m31 * k, p1.m32 * k, p1.m33 * k);
        }
        
        public static Mat3x3 operator *(Mat3x3 p1, float k)
        {
            return new Mat3x3(p1.m11 * k, p1.m12 * k, p1.m13 * k,
                p1.m21 * k, p1.m22 * k, p1.m23 * k,
                p1.m31 * k, p1.m32 * k, p1.m33 * k);
        }
        
        //矩阵与矢量相乘, 矢量在左边
        public static Vec3 operator *(Vec3 v1, Mat3x3 p1)
        {
            float num1 = v1.x * p1.m11 + v1.y * p1.m21 + v1.z * p1.m31;
            float num2 = v1.x * p1.m12 + v1.y * p1.m22 + v1.z * p1.m32;
            float num3 = v1.x * p1.m13 + v1.y * p1.m23 + v1.z * p1.m33;
            
            return new Vec3(
                Mathf.Abs(num1) < 1e-6 ? 0f : num1,
                Mathf.Abs(num2) < 1e-6 ? 0f : num2,
                Mathf.Abs(num3) < 1e-6 ? 0f : num3);
        }
        
        //矩阵的加法
        public static Mat3x3 operator +(Mat3x3 p1, Mat3x3 p2)
        {
            return new Mat3x3(p1.m11 - p2.m11, p1.m12 - p2.m12, p1.m13 - p2.m13,
                p1.m21 - p2.m21, p1.m22 - p2.m22, p1.m23 - p2.m23,
                p1.m31 - p2.m31, p1.m32 - p2.m32, p1.m33 - p2.m33);
        }
        
        //矩阵的减法
        public static Mat3x3 operator -(Mat3x3 p1, Mat3x3 p2)
        {
            return new Mat3x3(p1.m11 + p2.m11, p1.m12 + p2.m12, p1.m13 + p2.m13,
                p1.m21 + p2.m21, p1.m22 + p2.m22, p1.m23 + p2.m23,
                p1.m31 + p2.m31, p1.m32 + p2.m32, p1.m33 + p2.m33);
        }
        
        //矩阵乘法
        public static Mat3x3 operator *(Mat3x3 p1, Mat3x3 p2)
        {
            Mat3x3 res = new Mat3x3(Vec3.Zero, Vec3.Zero, Vec3.Zero);
            res.m11 = p1.m11 * p2.m11 + p1.m12 * p2.m21 + p1.m13 * p2.m31;
            res.m12 = p1.m11 * p2.m12 + p1.m12 * p2.m22 + p1.m13 * p2.m32;
            res.m13 = p1.m11 * p2.m13 + p1.m12 * p2.m23 + p1.m13 * p2.m33;
            res.m21 = p1.m21 * p2.m11 + p1.m22 * p2.m21 + p1.m23 * p2.m31;
            res.m22 = p1.m21 * p2.m12 + p1.m22 * p2.m22 + p1.m23 * p2.m32;
            res.m23 = p1.m21 * p2.m13 + p1.m22 * p2.m23 + p1.m23 * p2.m33;
            res.m31 = p1.m31 * p2.m11 + p1.m32 * p2.m21 + p1.m33 * p2.m31;
            res.m32 = p1.m31 * p2.m12 + p1.m32 * p2.m22 + p1.m33 * p2.m32;
            res.m33 = p1.m31 * p2.m13 + p1.m32 * p2.m23 + p1.m33 * p2.m33;
            return res;
        }

        public static bool operator ==(Mat3x3 p1, Mat3x3 p2)
        {
            return Mathf.Abs(p1.m11 - p2.m11) < 1e-5 &&
                   Mathf.Abs(p1.m12 - p2.m12) < 1e-5 &&
                   Mathf.Abs(p1.m13 - p2.m13) < 1e-5 &&
                   Mathf.Abs(p1.m21 - p2.m21) < 1e-5 &&
                   Mathf.Abs(p1.m22 - p2.m22) < 1e-5 &&
                   Mathf.Abs(p1.m23 - p2.m23) < 1e-5 &&
                   Mathf.Abs(p1.m31 - p2.m31) < 1e-5 &&
                   Mathf.Abs(p1.m32 - p2.m32) < 1e-5 &&
                   Mathf.Abs(p1.m33 - p2.m33) < 1e-5;
        }

        public static bool operator !=(Mat3x3 p1, Mat3x3 p2)
        {
            return !(p1 == p2);
        }

        #endregion

        #region 方法
        
        //旋转矩阵
        public static Mat3x3 FromRotationAxis(Vec3 axis, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            var (sin, cos) = (Mathf.Sin(rad), Mathf.Cos(rad));
            var cos1 = 1 - cos;
            var (x, y, z) = (axis.x, axis.y, axis.z);
            
            return new Mat3x3(
                x * x * cos1 + cos, x * y * cos1 + z * sin, x * z * cos1 - y * sin,
                x * y * cos1 - z * sin, y * y * cos1 + cos, y * z * cos1 + x * sin,
                x * z * cos1 + y * sin, y * z * cos1 - x * sin, z * z * cos1 + cos);
        }
        
        //缩放矩阵
        public static Mat3x3 FromScale(float k)
        {
            return new Mat3x3(k, 0f, 0f, 0f, k, 0f, 0f, 0f, k);
        }
        
        //欧拉角转换为旋转矩阵
        public static Mat3x3 FromEuler(Vec3 euler)
        {
            float cx = Mathf.Cos(euler.x);
            float sx = Mathf.Sin(euler.x);
            float cy = Mathf.Cos(euler.y);
            float sy = Mathf.Sin(euler.y);
            float cz = Mathf.Cos(euler.z);
            float sz = Mathf.Sin(euler.z);

            // multiply ZYX
            return new Mat3x3(
                cy * cz, cz * sx * sy - cx * sz, cx * cz * sy + sx * sz,
                cy * sz, cx * cz + sx * sy * sz, -cz * sx + cx * sy * sz,
                -sy, cy * sx, cx * cy);
        }

        public bool Equals(Mat3x3 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return m11.Equals(other.m11) && m12.Equals(other.m12) && m13.Equals(other.m13) 
                   && m21.Equals(other.m21) && m22.Equals(other.m22) && m23.Equals(other.m23) 
                   && m31.Equals(other.m31) && m32.Equals(other.m32) && m33.Equals(other.m33);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mat3x3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m11.GetHashCode();
                hashCode = (hashCode * 397) ^ m12.GetHashCode();
                hashCode = (hashCode * 397) ^ m13.GetHashCode();
                hashCode = (hashCode * 397) ^ m21.GetHashCode();
                hashCode = (hashCode * 397) ^ m22.GetHashCode();
                hashCode = (hashCode * 397) ^ m23.GetHashCode();
                hashCode = (hashCode * 397) ^ m31.GetHashCode();
                hashCode = (hashCode * 397) ^ m32.GetHashCode();
                hashCode = (hashCode * 397) ^ m33.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"<{m11} {m12} {m13}>\n<{m21} {m22} {m23}>\n<{m31} {m32} {m33}>";
        }

        #endregion
    }
}
