using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace SituationSticky
{
    public class LoadPort : ViewPort
    {
        protected float _Threshold;
        protected Vector2 _LastLoad = new Vector2(-10000f, -10000f);
        protected BackgroundWorker _Worker = new BackgroundWorker();
        protected Screen _Parent;

        public LoadPort(Screen parent, Vector2 position, Vector2 size, float threshold) : base(position, size) 
        {
            _Threshold = threshold;
            _Parent = parent;
            TargetLocation = _Parent.ViewPort.TargetLocation - ((Size - _Parent.ViewPort.Size) / 2);
            LoadContent(null, null);
        }

        public override void Update(GameTime time)
        {
            //base.Update(time);

            TargetLocation = _Parent.ViewPort.TargetLocation - ((Size-_Parent.ViewPort.Size)/2);
            Vector2 diff = ActualLocation - _LastLoad;

            if (diff.Length() > _Threshold)
            {
                _Worker.DoWork += LoadContent;
                if (!_Worker.IsBusy)
                    _Worker.RunWorkerAsync();
            }
        }

        public void LoadContent(object source, DoWorkEventArgs e)
        {
            _LastLoad = ActualLocation;
            CheckList(_Parent.Entities);
            CheckList(_Parent.Tiles);
            CheckList(_Parent.Walls);
            CheckList(_Parent.Lights);
            CheckList(_Parent.Shadows);
        }

        protected virtual void CheckList(EntityList list)
        {
            list.Loaded.ForEach(CheckActive, list.Loaded, list.Unloaded, null);
            list.Unloaded.ForEach(CheckInactive, list.Loaded, list.Unloaded, null);
        }

        public bool CheckInactive(Entity ent, object activeList, object inactiveList, object p3)
        {
            ThreadDictionary<UInt64, Entity> alist = (ThreadDictionary<UInt64, Entity>)activeList;
            ThreadDictionary<UInt64, Entity> ilist = (ThreadDictionary<UInt64, Entity>)inactiveList;
            if (!(ent.Position.X + ent.Radius < ActualLocation.X ||
                ent.Position.Y + ent.Radius < ActualLocation.Y ||
                ent.Position.X - ent.Radius > ActualLocation.X + Size.X ||
                ent.Position.Y - ent.Radius > ActualLocation.Y + Size.Y))
            {
                ilist.Remove(ent.ID);
                alist.Add(ent.ID, ent);
            }
            return true;
        }
        public bool CheckActive(Entity ent, object activeList, object inactiveList, object p3)
        {
            ThreadDictionary<UInt64, Entity> alist = (ThreadDictionary<UInt64, Entity>)activeList;
            ThreadDictionary<UInt64, Entity> ilist = (ThreadDictionary<UInt64, Entity>)inactiveList;
            if (ent.Position.X + ent.Radius < ActualLocation.X ||
                ent.Position.Y + ent.Radius < ActualLocation.Y ||
                ent.Position.X - ent.Radius > ActualLocation.X + Size.X ||
                ent.Position.Y - ent.Radius > ActualLocation.Y + Size.Y)
            {
                alist.Remove(ent.ID);
                ilist.Add(ent.ID, ent);
            }
            return true;
        }
    }
}
