using UnityEngine;

namespace MyMath
{
    [System.Serializable]
    public class Quat
    {
        #region 字段

        public float x;
        public float y;
        public float z;
        public float w;

        public static readonly Quat Identity = new Quat(0f, 0f, 0f, 1f);

        #endregion

        #region 属性

        public float Length => Mathf.Sqrt(Dot(this, this));

        public Quat Normalized
        {
            get
            {
                float l = 1 / Length;
                return this * l;
            }
        }

        public Quat Conjugate => new Quat(-x, -y, -z, w);

        public Quat Inverse
        {
            get
            {
                float l = 1 / Length;
                return Conjugate * l;
            }
        }

        public Mat3x3 ToMat3x3
        {
            get
            {
                float num1 = 2 * x;
                float num2 = 2 * y;
                float num3 = 2 * z;
                float num4 = x * num1;
                float num5 = y * num2;
                float num6 = z * num3;
                float num7 = x * num2;
                float num8 = x * num3;
                float num9 = y * num3;
                float num10 = w * num1;
                float num11 = w * num2;
                float num12 = w * num3;

                return new Mat3x3(
                    1 - num5 - num6, num7 + num12, num8 - num11,
                    num7 - num12, 1 - num4 - num6, num9 + num10,
                    num8 + num11, num9 - num10, 1 - num4 - num5);
            }
        }

        public Vec3 ToEuler
        {
            get
            {
                float sp = -2.0f * (y * z - w * x);

                var (p, h, b) = (0f, 0f, 0f);
                if (Mathf.Abs(sp) > 0.9999f)
                {
                    p = Mathf.PI / 2 * sp;
                    h = Mathf.Atan2(-x * z + w * y, 0.5f - y * y - z * z);
                    b = 0f;
                }
                else
                {
                    p = Mathf.Asin(sp);
                    h = Mathf.Atan2(x * z + w * y, 0.5f - x * x - y * y);
                    b = Mathf.Atan2(x * y + w * z, 0.5f - x * x - z * z);
                }

                return new Vec3(p * Mathf.Rad2Deg, h * Mathf.Rad2Deg, b * Mathf.Rad2Deg);
            }
        }

        public Quaternion ToQuaternion => new Quaternion(x, y, z, w);

        #endregion

        #region 构造函数
        
        private Quat()
        {
            x = 0f;
            y = 0f;
            z = 0f;
            w = 1f;
        }

        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #endregion

        #region 运算符

        public static Quat operator -(Quat q1)
        {
            return new Quat(-q1.x, -q1.y, -q1.z, -q1.w);
        }

        public static Quat operator +(Quat q1, Quat q2)
        {
            return new Quat(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
        }

        public static Quat operator -(Quat q1, Quat q2)
        {
            return new Quat(q1.x - q2.x, q1.y - q2.y, q1.z - q2.z, q1.w - q2.w);
        }

        public static Quat operator *(float k, Quat q)
        {
            return new Quat(q.x * k, q.y * k, q.z * k, q.w * k);
        }
        
        public static Quat operator *(Quat q, float k)
        {
            return new Quat(q.x * k, q.y * k, q.z * k, q.w * k);
        }
        
        public static Quat operator /(float k, Quat q)
        {
            return new Quat(q.x / k, q.y / k, q.z / k, q.w / k);
        }
        
        public static Quat operator /(Quat q, float k)
        {
            return new Quat(q.x / k, q.y / k, q.z / k, q.w / k);
        }
        
        //旋转的连接来设置矢量需要右乘
        public static Quat operator *(Quat q1, Quat q2)
        {
            return new Quat
            {
                x = q1.w * q2.x + q2.w * q1.x + q1.y * q2.z - q2.y * q1.z,
                y = q1.w * q2.y + q2.w * q1.y + q1.z * q2.x - q2.z * q1.x,
                z = q1.w * q2.z + q2.w * q1.z + q1.x * q2.y - q1.y * q2.x,
                w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z,
            };
        }
        
        //这种写法并不是很好，还有一种写法，通过将四元数转换为矩阵，矩阵与矢量相乘，来旋转矢量。
        public static Vec3 operator *(Vec3 v, Quat q)
        {
            Quat q1 = new Quat(v.x, v.y, v.z, 0);
            Quat q2 = q * q1 * q.Conjugate;

            return new Vec3(q2.x, q2.y, q2.z);
        }

        public static bool operator ==(Quat q1, Quat q2)
        {
            return Dot(q1, q2) > 0.9999f;
        }

        public static bool operator !=(Quat q1, Quat q2)
        {
            return !(q1 == q2);
        }

        public static implicit operator Quat(Quaternion q)
        {
            return new Quat(q.x, q.y, q.z, q.w);
        }

        #endregion

        #region 方法

        public static float Dot(Quat q1, Quat q2)
        {
            return q1.x * q2.x + q1.y * q2.y + q1.z * q2.z + q1.w * q2.w;
        }

        public static Quat FromAxisAngle(Vec3 axis, float angle)
        {
            angle = angle * 0.5f * Mathf.Deg2Rad;
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            return new Quat(axis.x * sin, axis.y * sin, axis.z * sin, cos);
        }

        public static float Angle(Quat q1, Quat q2)
        {
            float p = Dot(q1, q2);

            if (p > 0.9999f) return 0f;

            return 2 * Mathf.Acos(Mathf.Abs(Mathf.Clamp(p, -1f, 1f))) * Mathf.Rad2Deg;
        }

        public static Quat Exp(Quat q, float t)
        {
            //提取半角
            float alpha = Mathf.Acos(q.w);

            float newAlpha = t * alpha;

            float mult = Mathf.Sin(newAlpha) / Mathf.Sin(alpha);

            return new Quat(q.x * mult, q.y * mult, q.z * mult, Mathf.Cos(newAlpha));
        }
        
        //Lerp 和 Slerp 效果相同，但 Slerp 效率更快，所以我们一般使用 Slerp
        public static Quat Lerp(Quat q1, Quat q2, float t)
        {
            //线性插值
            return Exp(q2 * q1.Conjugate, t) * q1;
        }

        public static Quat Slerp(Quat q1, Quat q2, float t)
        {
            float p = Dot(q1, q2);

            if (p < 0)
            {
                q2 = -q2;
                p = -p;
            }

            if (p > 0.9999f) return q1;

            float red = Mathf.Acos(p) * 2;
            float sin = Mathf.Sin(red);

            return Mathf.Sin((1 - t) * red) / sin * q1 + Mathf.Sin(t * red) / sin * q2;
        }

        public static Quat FormEuler(float x, float y, float z)
        {
            x = 0.5f * x * Mathf.Deg2Rad;
            y = 0.5f * y * Mathf.Deg2Rad;
            z = 0.5f * z * Mathf.Deg2Rad;
            
            float ch = Mathf.Cos(y);
            float sh = Mathf.Sin(y);
            float cp = Mathf.Cos(x);
            float sp = Mathf.Sin(x);
            float cb = Mathf.Cos(z);
            float sb = Mathf.Sin(z);
            
            //由于四元数从右向左乘，所以顺序是 YXZ
            return new Quat(
                ch * sp * cb + sh * cp * sb,
                sh * cp * cb - ch * sp * sb,
                ch * cp * sb - sh * sp * cb,
                ch * cp * cb + sh * sp * sb);
        }

        public static Quat FormEuler(Vec3 v) => FormEuler(v.x, v.y, v.z);
        
        public bool Equals(Quat other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Quat) obj);
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

        public override string ToString()
        {
            return $"<{x} {y} {z} {w}>";
        }

        #endregion
    }
}
