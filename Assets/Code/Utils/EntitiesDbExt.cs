using System;
using Svelto.ECS;

namespace Code.Game.Others.Extensions
{
    public static class EntitiesDbExt
    {
        public static ArraySegment<T> QueryEntitiesSegment<T>(this IEntitiesDB db, ExclusiveGroup.ExclusiveGroupStruct groupStruct) where T : struct, IEntityStruct
        {
            uint count;
            return db.QueryEntities<T>(groupStruct, out count).ToArraySegment(count);
        }
    }
}