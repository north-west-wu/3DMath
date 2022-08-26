using UnityEngine;

namespace MyMath
{
    public struct Ray2
    {
        #region 字段

        public Vec2 origin;
        public Vec2 direction;

        #endregion

        #region 属性

        public Ray2D ToRay => new Ray2D(origin.ToVector2, direction.ToVector2);

        #endregion

        #region 构造函数

        public Ray2(Vec2 origin, Vec2 direction)
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
        
        public Vec2 ClosestPoint(Vec2 p)
        {
            float t = Vec2.Dot(direction, p - origin);
            return origin + t * direction;
        }
        
        #endregion
    }
}
