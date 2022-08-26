using System;
using Plane = MyMath.Plane;

namespace MyMath
{
    public struct Plane
    {
        #region 字段

        public Vec3 normal;
        public float distance;

        #endregion

        #region 属性

        public Plane Flipped => new Plane(-normal, -distance);

        #endregion

        #region 构造函数

        public Plane(Vec3 normal, float distance)
        {
            this.normal = normal;
            this.distance = distance;
        }

        //三点取面的写法
        public Plane(Vec3 p1, Vec3 p2, Vec3 p3)
        {
            Vec3 v1 = p2 - p1;
            Vec3 v2 = p3 - p2;

            normal = Vec3.Cross(v1, v2).Normalized;
            distance = Vec3.Dot(p1, normal);
        }

        //对于多边形的最佳拟合平面
        public Plane(params Vec3[] arr)
        {
            int n = arr.Length;
            if (n < 3) throw new AggregateException("矢量数量不足3");

            Vec3 v = Vec3.Zero;

            Vec3 p1 = arr[n - 1];

            for (int i = 0; i < n; i++)
            {
                Vec3 p2 = arr[i];

                v.x += (p2.z + p1.z) * (p2.y - p1.y);
                v.y += (p2.x + p1.x) * (p2.z - p1.z);
                v.z += (p2.y + p1.y) * (p2.x - p1.x);

                p1 = p2;
            }

            normal = v.Normalized;
            distance = Vec3.Dot(arr[0], normal);
        }

        #endregion

        #region 方法
        
        public Vec3 ClosestPoint(Vec3 p)
        {
            float a = DistanceToPoint(p);
            return p - a * normal;
        }

        public float DistanceToPoint(Vec3 p)
        {
            return Vec3.Dot(p, normal) - distance;
        }

        public bool Side(Vec3 p)
        {
            return DistanceToPoint(p) > 0f;
        }

        public bool SameSide(Vec3 p1, Vec3 p2)
        {
            float n1 = DistanceToPoint(p1);
            float n2 = DistanceToPoint(p2);

            return (n1 >= 0f && n2 >= 0f) || (n1 < 0f && n2 < 0f);
        }

        #endregion
    }
}