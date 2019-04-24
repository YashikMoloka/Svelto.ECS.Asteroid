using System;
using System.Collections.Generic;

namespace Code.Game.Others.Extensions
{
    public static class ArrayQueryExt
    {
        public static ArraySegment<T> ToArraySegmentFull<T>(this T[] array, int offset, uint count)
        {
            return new ArraySegment<T>(array, offset, (int) count);
        }
        
        
        public static ArraySegment<T> ToArraySegment<T>(this T[] array, uint count)
        {
            return ToArraySegmentFull(array, 0, count);
        }
    }
}