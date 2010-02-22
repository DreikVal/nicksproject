using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class ShadowRegion : Entity
    {
        public float Radius { get { return _Radius; } set { _Radius = value; } }
        protected float _Radius;

        public Entity Owner { get { return _Owner; } }
        protected Entity _Owner;

        public ShadowRegion(Entity owner, Vector2 pos, float radius) : base(owner.Parent)
        {
            _Owner = owner;
            _Geometry = new Geometry(this, pos, radius * 2, radius * 2, 0.0f);
            _Radius = radius;
        }

        public override void Update(GameTime time)
        {
            Geometry.Position = _Owner.Geometry.Position;
        }
    }
}
