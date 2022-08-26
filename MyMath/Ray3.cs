using UnityEngine;

namespace MyMath
{
    public struct Ray3
    {
        #region 字段

        public Vec3 origin;
        public Vec3 direction;

        #endregion
        
        #region 属性

        public Ray ToRay => new Ray(origin.ToVector3, direction.ToVector3);

        #endregion

        #region 构造函数

        public Ray3(Vec3 origin, Vec3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        #endregion

        #region 方法

        public Vec3 Point(float distance)
        {
            return origin + distance * direction;
        }

        public Vec3 ClosestPoint(Vec3 p)
        {
            float t = Vec3.Dot(direction, p - origin);
            return origin + t * direction;
        }

        public bool IntersectToRay(Ray r1, Ray r2, out Vec3 v)
        {
            v = Vec3.Zero;
            return true;
        }

        #endregion
    }
}
