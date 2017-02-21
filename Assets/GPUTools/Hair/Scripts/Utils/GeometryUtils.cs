using UnityEngine;

namespace GPUTools.Hair.Scripts.Utils
{
    public class GeometryUtils
    {
        public static Vector2 To2D(int i, int sizeY)
        {
            var x = i / sizeY;
            var y = i % sizeY;
            return new Vector2(x, y);
        }
    }
}
