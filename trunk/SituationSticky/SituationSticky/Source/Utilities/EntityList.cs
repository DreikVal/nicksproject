using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SituationSticky
{
    public class EntityList
    {
        public ThreadDictionary<UInt64, Entity> Loaded { get { return _Loaded; } }
        protected ThreadDictionary<UInt64, Entity> _Loaded = new ThreadDictionary<ulong,Entity>();

        public ThreadDictionary<UInt64, Entity> Unloaded { get { return _Unloaded; } }
        protected ThreadDictionary<UInt64, Entity> _Unloaded = new ThreadDictionary<ulong,Entity>();

        public Screen Parent { get { return _Parent; } }
        protected Screen _Parent;

        public EntityList(Screen parent) { _Parent = parent; }
    }
}
