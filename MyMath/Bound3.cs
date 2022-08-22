using System;
using UnityEngine;

namespace MyMath
{
    public struct Bound3
    {
        #region 字段
        
        public Vec3 min;
        public Vec3 max;

        #endregion

        #region 属性

        public Vec3 this[int index]
        {
            get
            {
                if (index < 0 || index > 7)
                    throw new IndexOutOfRangeException();

                return new Vec3(
                    (index & 1) == 0 ? min.x : max.x,
                    (index & 2) == 0 ? min.y : max.y,
                    (index & 4) == 0 ? min.z : max.z);
            }
        }

        public bool IsEmpty => min.x > max.x || min.y > max.y || min.z > max.z;

        public Vec3 Center => (max + min) * 0.5f;

        public Vec3 Size => max - min;

        public Vec3 Extents => (max - min) * 0.5f;

        public float Width => max.x - min.x;

        public float Height => max.y - min.y;

        public float Depth => max.z - min.z;

        public Bounds ToBounds => new Bounds(Center.ToVector3, Size.ToVector3);

        #endregion

        #region 构造函数

        public Bound3(Vec3 min, Vec3 max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion

        #region 方法

        public bool Contains(Vec3 point)
        {
            return point.x >= min.x && point.x <= max.x &&
                   point.y >= min.y && point.y <= max.y &&
                   point.z >= min.z && point.z <= max.z;
        }

        public void Clear()
        {
            min.x = min.y = min.z = float.MaxValue;
            max.x = max.y = max.z = float.MinValue;
        }

        public void Encapsulate(Vec3 point)
        {
            if (point.x < min.x) min.x = point.x;
            if (point.x > max.x) max.x = point.x;

            if (point.y < min.y) min.y = point.y;
            if (point.y > max.y) max.y = point.y;

            if (point.z < min.z) min.z = point.z;
            if (point.z > max.z) max.z = point.z;
        }

        public void Encapsulate(Bound3 box)
        {
            if (box.min.x < min.x) min.x = box.min.x;
            if (box.max.x > max.x) max.x = box.max.x;

            if (box.min.y < min.y) min.y = box.min.y;
            if (box.max.y > max.y) max.y = box.max.y;

            if (box.min.z < min.z) min.z = box.min.z;
            if (box.max.z > max.z) max.z = box.max.z;
        }

        #endregion
    }
}
