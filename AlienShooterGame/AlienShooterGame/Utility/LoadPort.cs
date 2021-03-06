﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
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
            _Parent.InactiveEntities.ForEach(CheckInactive, null, null, null);
            _Parent.Entities.ForEach(CheckActive, null, null, null);
        }

        public bool CheckInactive(Entity ent, object p1, object p2, object p3)
        {
            
            if (!(ent.Geometry.Position.X + ent.Geometry.Radius < ActualLocation.X ||
                ent.Geometry.Position.Y + ent.Geometry.Radius < ActualLocation.Y ||
                ent.Geometry.Position.X - ent.Geometry.Radius > ActualLocation.X + Size.X ||
                ent.Geometry.Position.Y - ent.Geometry.Radius > ActualLocation.Y + Size.Y))
            {
                
                if (ent as Tile != null)
                    _Parent.BGEntities.Add(ent.ID, ent);
                else if (ent as ShadowRegion != null)
                    _Parent.Shadows.Add(ent.ID, ent as ShadowRegion);
                else if (ent as LightSource != null)
                    _Parent.Lights.Add(ent.ID, ent as LightSource);
                _Parent.InactiveEntities.Remove(ent.ID);
                _Parent.Entities.Add(ent.ID, ent);
            }
            return true;
        }
        public bool CheckActive(Entity ent, object p1, object p2, object p3)
        {
            if (ent.Geometry.Position.X + ent.Geometry.Radius < ActualLocation.X ||
                ent.Geometry.Position.Y + ent.Geometry.Radius < ActualLocation.Y ||
                ent.Geometry.Position.X - ent.Geometry.Radius > ActualLocation.X + Size.X ||
                ent.Geometry.Position.Y - ent.Geometry.Radius > ActualLocation.Y + Size.Y)
            {
                if (ent as Tile != null)
                    _Parent.BGEntities.Remove(ent.ID);
                else if (ent as LightSource != null)
                    _Parent.Lights.Remove(ent.ID);
                else if (ent as ShadowRegion != null)
                    _Parent.Shadows.Remove(ent.ID);
                else if (ent as Bullet != null)
                {
                    ent.Dispose();
                    return true;
                }
                _Parent.Entities.Remove(ent.ID);
                _Parent.InactiveEntities.Add(ent.ID, ent);
            }
            return true;
        }
    }
}
