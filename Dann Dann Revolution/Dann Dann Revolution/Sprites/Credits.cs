//Dann Dann Revolution Team
//File Created: 1/22/2018
//Credit Screen With Beautiful Animations

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dann_Dann_Revolution
{
    class Credits : ISprite, IEntity
    {
        public Vector2 Position { get; set; }   //Like Always, More Or Less Useless
        private float totalSeconds;             //Total Seconds Transpired
        private Texture2D DreamTeam;            //Picture Of The Dream Team
        private Texture2D DannChan;             //Company Logo
        private Texture2D DannArrow;            //Dann Arrow
        private SpriteFont smallerFont;         //36pt Version Of The Game Font


        public Credits() {
            ContentManager contentmanger= GameServices.Get<ContentManager>();

            //LOAD MY ASSETS
            DannChan = contentmanger.Load<Texture2D>("Graphics/Credits/Dann Chan Productions");
            DreamTeam = contentmanger.Load<Texture2D>("Graphics/Credits/Dream Team");
            smallerFont = contentmanger.Load<SpriteFont>("Fonts/Arial Black 36pt");
            DannArrow = contentmanger.Load<Texture2D>("Graphics/Dan Arrow Down");

            //Play Some Jams
            Song song = contentmanger.Load<Song>("Music/All Star");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 1f;
            totalSeconds = 0f;
        }

        //Oh my. Have I not done enough of these system call comments yet? It's getting hard to come up with things to say in them
        /// <summary>
        /// Updates The Class
        /// </summary>
        /// <param name="GameTime">Time Object</param>
        public void Update(GameTime GameTime)
        {
            // Update the time (talk about rude - this class checks its watch several times every second!)
            totalSeconds += (float)GameTime.ElapsedGameTime.TotalSeconds;
            if (InputManager.Pressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                MediaPlayer.Stop();
                SceneManager.EndScene();
            }
        }

        /// <summary>
        /// Draws The Class Elements
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch Object</param>
        public void Draw(SpriteBatch spriteBatch)
        {

            // Variable dictionary I guess
            string text;    // Text being displayed at any given time
            Vector2 textPos = new Vector2(0, 0);   // The POSITION of TEXT
            SpriteFont font = DannDannRevolution.GameFont;  // Because I don't wanna type "DannDannRevolution.GameFont" over and over
                                                            // (even with Visual Studio's cool autocomplete)

            //Now we make beautiful animations
            if (totalSeconds >= 0 && totalSeconds < 8)
            {
                text = "Dann Dann Revolution";
                textPos = new Vector2(1920 / 2 - font.MeasureString(text).X / 2, 20);
                spriteBatch.DrawString(font, text, textPos, Color.White);
            }

            if (totalSeconds >= 2 && totalSeconds < 8)
            {
                text = "By Dann Chan Productions";
                textPos = new Vector2(1920 / 2 - font.MeasureString(text).X / 2, 120);
                spriteBatch.DrawString(font, text, textPos, Color.White);
                spriteBatch.Draw(DannChan, new Vector2(1920 / 2 - DannChan.Width / 2, 230), Color.White);
            }

            if (totalSeconds >= 4 && totalSeconds < 8)
            {
                text = "4 People, ";
                textPos = new Vector2(100, 900);
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(font.MeasureString(text).X + 5, 0);
            }

            if (totalSeconds >= 5 && totalSeconds < 8)
            {
                text = "15 Songs, ";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(font.MeasureString(text).X + 5, 0);
            }

            if (totalSeconds >= 6 && totalSeconds < 8)
            {
                text = "One Inspiration.";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(font.MeasureString(text).X + 5, 0);
            }

            if (totalSeconds >= 8 && totalSeconds < 10)
            {
                text = "Let's Meet The Team";
                textPos += new Vector2(1920 / 2 - font.MeasureString(text).X / 2, 1080 / 2 - font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, textPos, Color.White);
            }

            if (totalSeconds >= 10)
            {
                spriteBatch.Draw(DreamTeam, new Rectangle(0, 0, 1920 / 2, 1080 / 2), Color.White);
            }

            if (totalSeconds >= 11)
            {
                text = "Alex \"DANNImator\" Kitt // Monogame Master";
                textPos += new Vector2(10, 560);
                spriteBatch.DrawString(smallerFont, text, textPos, Color.White);
                textPos += new Vector2(0, 100);
            }

            if (totalSeconds >= 11 && totalSeconds < 13)
            {
                spriteBatch.Draw(DannArrow, new Rectangle(275, 0, DannArrow.Width - 40, DannArrow.Height - 40), Color.White);
            }

            if (totalSeconds >= 13)
            {
                text = "Noah \"What Can I Do\" Sweetnam // Project Manager";
                spriteBatch.DrawString(smallerFont, text, textPos, Color.White);
                textPos += new Vector2(0, 100);
            }

            if (totalSeconds >= 13 && totalSeconds < 15)
            {
                spriteBatch.Draw(DannArrow, new Rectangle(360, 0, DannArrow.Width - 40, DannArrow.Height - 40), Color.White);
            }

            if (totalSeconds >= 15)
            {
                text = "Mike \"What's Sleep?\" Hodges // Lead Programmer";
                spriteBatch.DrawString(smallerFont, text, textPos, Color.White);
                textPos += new Vector2(0, 100);
            }

            if (totalSeconds >= 15 && totalSeconds < 17)
            {
                spriteBatch.Draw(DannArrow, new Rectangle(445, -4, DannArrow.Width - 40, DannArrow.Height - 40), Color.White);
            }

            if (totalSeconds >= 17)
            {
                text = "Vaughn \"Chief Financial Officer\" Gregory // Lead Designer";
                spriteBatch.DrawString(smallerFont, text, textPos, Color.White);
                textPos += new Vector2(0, 100);
            }

            if (totalSeconds >= 17 && totalSeconds < 19.70)
            {
                spriteBatch.Draw(DannArrow, new Rectangle(550, -7, DannArrow.Width - 40, DannArrow.Height - 40), Color.White);
            }

            if (totalSeconds >= 19.70)
            {
                text = "--Dann Chan Productions-- ";
                spriteBatch.DrawString(font, text, textPos, Color.White);
            }

            if (totalSeconds >= 25)
            {
                text = "Press Enter To Exit..";
                spriteBatch.DrawString(smallerFont, text, new Vector2(1920 - smallerFont.MeasureString(text).X, 1080 - smallerFont.MeasureString(text).Y), Color.White);
            }

            //Oh yes, that is indeed beautiful
        }
    }
}
