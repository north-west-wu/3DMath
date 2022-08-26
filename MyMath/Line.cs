namespace MyMath
{
    public struct Line
    {
        #region 字段

        public Vec2 normal;
        public float distance;

        #endregion

        #region 构造函数

        public Line(Vec2 normal, float distance)
        {
            this.normal = normal;
            this.distance = distance;
        }

        public Line(Vec2 p1, Vec2 p2)
        {
            Vec2 v = p2 - p1;
            normal = (v * Mat2x2.FromRotationAxis(90f)).Normalized;
            distance = Vec2.Dot(p1, normal);
        }

        #endregion

        #region 方法

        public float DistanceToPoint(Vec2 p)
        {
            return Vec2.Dot(p, normal) - distance;
        }

        public Vec2 ClosestPoint(Vec2 p)
        {
            float a = DistanceToPoint(p);
            return p - a * normal;
        }

        public bool IntersectToLine(Line l1, Line l2, out Vec2 p)
        {
            float t = l1.normal.x * l2.normal.y - l1.normal.y * l2.normal.x;

            if (t < 1e-6)
            {
                p = Vec2.Zero;
                return false;
            }

            p = new Vec2(
                (l1.distance * l2.normal.y - l1.normal.y * l2.distance) / t,
                (l1.normal.x * l2.distance - l1.distance * l2.normal.x) / t);
            return true;
        }

        #endregion
    }
}