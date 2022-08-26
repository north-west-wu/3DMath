using System;
using UnityEngine;

namespace MyMath
{
    public struct Bound2
    {
        #region 字段

        public Vec2 min;
        public Vec2 max;

        #endregion
        
        #region 属性

        public Vec3 this[int index]
        {
            get
            {
                if (index < 0 || index > 7)
                    throw new IndexOutOfRangeException();

                return new Vec2(
                    (index & 1) == 0 ? min.x : max.x,
                    (index & 2) == 0 ? min.y : max.y);
            }
        }

        public bool IsEmpty => min.x > max.x || min.y > max.y;
        
        public Vec2 Center => (max + min) * 0.5f;
        
        public Vec2 Size => max - min;

        public Vec2 Extents => (max - min) * 0.5f;

        public float Width => max.x - min.x;

        public float Height => max.y - min.y;

        public Rect ToRect => new Rect(min.x, max.y, Height, Width);

        #endregion

        #region 方法
        
        public Vec2 ClosestPoint(Vec2 p)
        {
            if (p.x < min.x) p.x = min.x;
            else if (p.x > max.x) p.x = max.x;

            if (p.y < min.y) p.y = min.y;
            else if (p.y > max.y) p.y = max.y;

            return p;
        }

        public bool Contains(Vec2 point)
        {
            return point.x >= min.x && point.x <= max.x &&
                   point.y >= min.y && point.y <= max.y;
        }

        public void Clear()
        {
            min.x = min.y = float.MaxValue;
            max.x = max.y = float.MinValue;
        }

        public void Encapsulate(Vec2 point)
        {
            if (point.x < min.x) min.x = point.x;
            if (point.x > max.x) max.x = point.x;

            if (point.y < min.y) min.y = point.y;
            if (point.y > max.y) max.y = point.y;
        }

        public void Encapsulate(Bound2 box)
        {
            if (box.min.x < min.x) min.x = box.min.x;
            if (box.max.x > max.x) max.x = box.max.x;

            if (box.min.y < min.y) min.y = box.min.y;
            if (box.max.y > max.y) max.y = box.max.y;
        }

        #endregion
    }
}