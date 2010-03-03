using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace SituationSticky
{
    public class LoadPort
    {
        #region Members

        /// <summary>
        /// Distance from last load that the loadport needs to move before commencing a new content load.
        /// </summary>
        protected float _Threshold;
        protected Vector2 _LastLoad = new Vector2(-10000f, -10000f);

        public Vector2 Location { get { return _Location; } set { _Location = value; } }
        protected Vector2 _Location = new Vector2(-10000f, -10000f);

        public Vector2 Size { get { return _Size; } set { _Size = value; } }
        protected Vector2 _Size = new Vector2(1000, 800);

        /// <summary>
        /// The background worker that runs asynchronous content loading.
        /// </summary>
        protected BackgroundWorker _Worker = new BackgroundWorker();

        /// <summary>
        /// The screen for this loadport.
        /// </summary>
        protected Screen _Parent;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new load port which is used to load and unload content. Content within the bounds of the loadport is loaded.
        /// </summary>
        /// <param name="parent">The screen for the loadport.</param>
        /// <param name="position">The location of the loadport.</param>
        /// <param name="size">The size of the loadport.</param>
        /// <param name="threshold">The minimum distance the loadport has to move before a new content load begins.</param>
        public LoadPort(Screen parent, Vector2 position, Vector2 size, float threshold)
        {
            _Threshold = threshold;
            _Parent = parent;
            _Location = position;
            _Size = size;
            LoadContent(null, null);
        }

        #endregion

        #region Update

        public virtual void Update(GameTime time)
        {
            // Set location to position of viewport
            _Location.X = ((WorldScreen)_Parent).PlayerEntity.Position.X - _Size.X/2;
            _Location.Y = ((WorldScreen)_Parent).PlayerEntity.Position.Y - _Size.Y/2;
            Vector2 diff = _Location - _LastLoad;

            // Check if a new load is required.
            if (diff.Length() > _Threshold)
            {
                _Worker.DoWork += LoadContent;
                if (!_Worker.IsBusy)
                    _Worker.RunWorkerAsync();
            }
        }

        #endregion

        #region Utility

        public void LoadContent(object source, DoWorkEventArgs e)
        {
            // Set last load location
            _LastLoad = _Location;

            // Load/unload all entities.
            CheckList(_Parent.Entities);
            CheckList(_Parent.Tiles);
            CheckList(_Parent.Walls);
            CheckList(_Parent.Lights);
            CheckList(_Parent.Shadows);
        }

        protected virtual void CheckList(EntityList list)
        {
            // Unload all loaded entities that are outside loadport bounds.
            list.Loaded.ForEach(CheckActive, list.Loaded, list.Unloaded, null);

            // Load all unloaded entities that are inside loadport bounds.
            list.Unloaded.ForEach(CheckInactive, list.Loaded, list.Unloaded, null);
        }

        /// <summary>
        /// Checks if a given inactive entity should be loaded.
        /// </summary>
        /// <param name="ent">The entity to check.</param>
        /// <param name="activeList">The list for loaded entities.</param>
        /// <param name="inactiveList">The list for unloaded entities.</param>
        /// <param name="p3">Not used.</param>
        /// <returns>Not used.</returns>
        public bool CheckInactive(Entity ent, object activeList, object inactiveList, object p3)
        {
            // Cast lists
            ThreadDictionary<UInt64, Entity> alist = (ThreadDictionary<UInt64, Entity>)activeList;
            ThreadDictionary<UInt64, Entity> ilist = (ThreadDictionary<UInt64, Entity>)inactiveList;

            // Check if entity is within loadport bounds
            if (!(ent.Position.X + ent.Radius < Location.X ||
                ent.Position.Y + ent.Radius < Location.Y ||
                ent.Position.X - ent.Radius > Location.X + Size.X ||
                ent.Position.Y - ent.Radius > Location.Y + Size.Y))
            {
                ilist.Remove(ent.ID); // Remove the entity from the inactive list
                alist.Add(ent.ID, ent); // Add the entity to the active list
            }
            return true;
        }

        /// <summary>
        /// Checks if a given active entity should be unloaded.
        /// </summary>
        /// <param name="ent">The entity to check.</param>
        /// <param name="activeList">The list for loaded entities.</param>
        /// <param name="inactiveList">The list for unloaded entities.</param>
        /// <param name="p3">Not used.</param>
        /// <returns>Not used.</returns>
        public bool CheckActive(Entity ent, object activeList, object inactiveList, object p3)
        {
            // Cast lists
            ThreadDictionary<UInt64, Entity> alist = (ThreadDictionary<UInt64, Entity>)activeList;
            ThreadDictionary<UInt64, Entity> ilist = (ThreadDictionary<UInt64, Entity>)inactiveList;

            // Check if entity is outside the loadport bounds
            if (ent.Position.X + ent.Radius < Location.X ||
                ent.Position.Y + ent.Radius < Location.Y ||
                ent.Position.X - ent.Radius > Location.X + Size.X ||
                ent.Position.Y - ent.Radius > Location.Y + Size.Y)
            {
                // If it's a temporary entity we just delete it instead of unloading it
                if (ent.Temporary) { ent.Dispose(); return true; }
                if (ent as Crosshair != null) return true;

                alist.Remove(ent.ID); // Remove entity from loaded list
                ilist.Add(ent.ID, ent); // Add entity to unloaded list
            }
            return true;
        }

        #endregion
    }
}
