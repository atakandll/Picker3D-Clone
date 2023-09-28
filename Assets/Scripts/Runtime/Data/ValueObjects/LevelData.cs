using System;
using System.Collections.Generic;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct LevelData
    {
        public List<PoolData> Pools;
    }
}