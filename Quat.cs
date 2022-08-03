using UnityEngine;

namespace MyMath
{
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

        #endregion
    }
}
