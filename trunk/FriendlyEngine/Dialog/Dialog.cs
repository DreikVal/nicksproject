using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FriendlyEngine
{
    public class Dialog : DrawableGameComponent
    {
        public Conversation Conversation = null;
        public NPC npc = null;

        public Rectangle Area = new Rectangle(0, 0, 800, 200);

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D background;
        Texture2D moodTex;
        ContentManager content;
        Color previousTextColor = new Color(65, 65, 65, 255);
        Color speechColor = Color.White;
        Color highlightColor = Color.Gold;
        Color unlightedColor = new Color(0, 128, 128, 255);

        Dictionary<string, Texture2D> moodDict = new Dictionary<string, Texture2D>();
        int currentHandler = 0;
        public int startDelay = 1;
        string lastCaption = null;
        int mood = 50;

        public Dialog(Game game, ContentManager content)
            : base(game)
        {
            this.content = content;
        }


        public int Mood
        {
            get { return mood; }
            set { mood = value; }
        }

        public string prevCaption
        {
            get { return lastCaption; }
            set { lastCaption = value; }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts/Palatino");

            moodTex = content.Load<Texture2D>("Sprites/Moodlets/angry");
            moodDict.Add("angry", moodTex);
            moodTex = content.Load<Texture2D>("Sprites/Moodlets/annoyed");
            moodDict.Add("annoyed", moodTex);
            moodTex = content.Load<Texture2D>("Sprites/Moodlets/happy");
            moodDict.Add("happy", moodTex);
            moodTex = content.Load<Texture2D>("Sprites/Moodlets/neutral");
            moodDict.Add("neutral", moodTex);
            moodTex = content.Load<Texture2D>("Sprites/Moodlets/sad");
            moodDict.Add("sad", moodTex);

            background = new Texture2D(
                GraphicsDevice,
                1,
                1, 
                1, 
                TextureUsage.None, 
                SurfaceFormat.Color);

            background.SetData<Color>(new Color[] { Color.White });
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Conversation != null && npc != null)
            {

                if (mood >= 45)
                    moodTex = moodDict["neutral"];
                if (mood >= 64)
                    moodTex = moodDict["happy"];
                if (mood >= 100)
                    moodTex = moodDict["sad"];
                if (mood <= 38)
                    moodTex = moodDict["annoyed"];
                if (mood <= 25)
                    moodTex = moodDict["angry"];

                if (InputHelper.IsNewPress(Keys.W))
                {
                    currentHandler--;
                    if (currentHandler < 0)
                        currentHandler = Conversation.Handlers.Count - 1;
                }
                if (InputHelper.IsNewPress(Keys.S))
                {
                    currentHandler = 
                        (currentHandler + 1) % Conversation.Handlers.Count;
                }
                if (InputHelper.IsNewPress(Keys.Space) && startDelay == 0)
                {
                    lastCaption = Conversation.Handlers[currentHandler].Caption;
                    Conversation.Handlers[currentHandler].Invoke(npc);
                    currentHandler = 0;
                }
                if (InputHelper.IsKeyUp(Keys.Space))
                {
                    startDelay = 0;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int lastCaptionHeight = 32;

            Rectangle dest = new Rectangle(
                GraphicsDevice.Viewport.Width / 2 - Area.Width / 2,
                GraphicsDevice.Viewport.Height - Area.Height,
                Area.Width,
                Area.Height);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(background, dest, new Color(0, 0, 0, 150));
            spriteBatch.End();

            if (Conversation == null)
                return;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (lastCaption != null)
            {
                spriteBatch.DrawString(
                    spriteFont,
                    lastCaption,
                    new Vector2(dest.X + 16, dest.Y),
                    previousTextColor);
                lastCaptionHeight = (int)spriteFont.MeasureString(lastCaption).Y;
            }

            int lineHeight = (int)spriteFont.MeasureString(" ").Y;
            string fullText = WrapText(Conversation.Text);

            spriteBatch.DrawString(
                spriteFont,
                fullText,
                new Vector2(dest.X + 16, dest.Y + lastCaptionHeight + lineHeight),
                speechColor);

            int fullTextHeight = (int)spriteFont.MeasureString(fullText).Y + (lineHeight * 2);
            int captionHeight = lineHeight;

            for (int i = 0; i < Conversation.Handlers.Count; i++)
            {
                string caption = WrapText(Conversation.Handlers[i].Caption);
                Color color = (i == currentHandler) ? highlightColor : unlightedColor;

                spriteBatch.DrawString(
                    spriteFont,
                    caption,
                    new Vector2(
                        dest.X + 16,
                        dest.Y + fullTextHeight + captionHeight + (i * lineHeight)),
                        color);
                
                captionHeight = (int)spriteFont.MeasureString(caption).Y;

            }

           /* spriteBatch.Draw(
                moodTex,
                new Rectangle(dest.X + 450,
                dest.Y + fullTextHeight + captionHeight + lineHeight, 128, 128),
                Color.White);
            Smiley & mood image
            */

            spriteBatch.End();
        }

        private string WrapText(string text)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;
            
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < Area.Width)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return sb.ToString();
        }
    }
}
