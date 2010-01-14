using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FriendlyEngine
{
    public class NPC : AnimatedSprite
    {
        Dialog dialog;
        Script script;
        float speakingRadius = 80f;
        public AnimatedSprite Target;
        bool followingTarget = false;
        bool isHit = false;
        int health = 500;
        int maxHealth = 500;

        public float SpeakingRadius
        {
            get { return speakingRadius; }
            set { speakingRadius = (float)Math.Max(value, CollisionRadius); }
        }

        public NPC(Texture2D texture, Dialog dialog, Script script)
            :base(texture)
        {
            this.dialog = dialog;
            this.script = script;
        }

        public NPC(Texture2D texture)
            : base(texture)
        {
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth;}
            set { maxHealth = Math.Max(value, 1);}
        }

        public bool isHitWait
        {
            get { return isHit; }
            set { isHit = value; }
        }

        public override void Update(GameTime gameTime)
        {
            if (Target != null && followingTarget)
            {
                Position = Target.Center + new Vector2(Target.CollisionRadius + CollisionRadius, 0f);
            }

            base.Update(gameTime);
        }

        public bool InSpeakingRange(AnimatedSprite sprite)
        {
            Vector2 d = Origin - sprite.Origin;

            return (d.Length() < SpeakingRadius);
        }

        public bool InSpeakingRangeVector(Vector2 mouse)
        {
            Vector2 d = Origin - mouse;

            return (d.Length() < SpeakingRadius);
        }

        public void StartConversation(string conversationName)
        {
            if (script == null || dialog == null)
                return;
            dialog.Enabled = true;
            dialog.Visible = true;
            dialog.npc = this;
            dialog.startDelay = 1;
            dialog.Conversation = script[conversationName];
        }

        public void EndConversation()
        {
            if (script == null || dialog == null)
                return;
            dialog.prevCaption = null;
            dialog.Enabled = false;
            dialog.Visible = false;
        }

        public void SetMood(string modifier)
        {
            if (script == null || dialog == null)
                return;
            dialog.Mood += int.Parse(modifier);
        }

        public void StartFollowing()
        {
            followingTarget = true;
        }

        public void StopFollowing()
        {
            followingTarget = false;
        }
    }
}
