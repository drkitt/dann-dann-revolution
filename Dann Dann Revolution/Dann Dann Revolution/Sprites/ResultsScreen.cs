/*
 * Dann Dann Revolution Team
 * File Created: 1/17/2018
 * ResultsScreen: This class displays how the player did (by means of a percentage and a ranking) and gives them the ability to go back.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Dann_Dann_Revolution
{
    internal class ResultsScreen: IEntity, ISprite
    {
        /// <summary> All the possible backgrounds for this Screen (they're all variations of a cool picture of Mr. Dann) </summary>
        private static readonly List<Texture2D> backgrounds;
        /// <summary> All possible rankings for the player. They may look a tad familiar, if you've played the game... </summary>
        private static readonly List<Texture2D> rankings;
        /// <summary> All possible rewards for the player to get. Not redeemable for cash value! </summary>
        private static readonly List<Texture2D> rewards;
        /// <summary> The ranking image is scaled up THIS MANY times (in both directions)! </summary>
        private static readonly int SCALE =3;
        /// <summary> The player's mark for the song, sorta. It's the score divided by the max possible score, times 100. </summary>
        private int percentage;
        /// <summary> The image used for the background (we found a cool picture online for this) </summary>
        private Texture2D backgroundImage;
        /// <summary> The image used for the player's ranking </summary>
        private Texture2D rankingImage;
        /// <summary> The image used for the player's "reward" for playing so well </summary>
        private Texture2D rewardImage;
        /// <summary> Variable keeping track of the time elapsed, so as to create dramatic tension </summary>
        private float totalSeconds;
        /// <summary> Where the heck is this sprite located? Probably (0, 0). </summary>
        public Vector2 Position { get; set; }

        private int score;          //Score The The User Achived
        private int possiblePoints; //Score That Is Possible
        private int streak;         //Highest Streak The User Attained
        private int[] notesHit;     //Different Types Of Notes Hit
        
        /// <summary>
        /// You ever see that painting of a pipe that says, "This is not a pipe"? 
        /// Everyone, back when that painting was made, was all like, "That's a pipe!" And the artist was like, "Then use it!"
        /// Boom. Everyone was mystified, their perceptions of art shattered. How does this pertain to CompSci? Well, this... 
        /// This is not a constructor.
        /// </summary>
        static ResultsScreen()
        {
            // Summon ContentManager and load images
            ContentManager contentManager = GameServices.Get<ContentManager>();
            backgrounds = new List<Texture2D>
            {
                contentManager.Load<Texture2D>("Graphics/Results screen/result_bad"),
                contentManager.Load<Texture2D>("Graphics/Results screen/result_ok"),
                contentManager.Load<Texture2D>("Graphics/Results screen/result_good"),
                contentManager.Load<Texture2D>("Graphics/Results screen/result_perfect")
            };
            rankings = new List<Texture2D>
            {
                contentManager.Load<Texture2D>("Graphics/bad"),
                contentManager.Load<Texture2D>("Graphics/ok"),
                contentManager.Load<Texture2D>("Graphics/good"),
                contentManager.Load<Texture2D>("Graphics/perfect"),
            };
            rewards = new List<Texture2D>
            {
                // This is to be expanded later, as more rewards are made
                contentManager.Load<Texture2D>("Graphics/Results screen/Rewards/dann_can"),
                contentManager.Load<Texture2D>("Graphics/Results screen/Rewards/dannfish_pants"),
                contentManager.Load<Texture2D>("Graphics/Results screen/Rewards/dann_cookie"),
                contentManager.Load<Texture2D>("Graphics/Results screen/Rewards/oscar_dann_2")
            };
        }   // Still think that's a constructor? TRY CONSTRUCTING WITH IT


        /// <summary>
        /// ResultsScreen: This CONSTRUCTINATOR determines the player's performance and via the cool parameters to create an instance of this class
        /// </summary>
        /// <param name="_score"> The score the player got during the song </param>
        /// <param name="_possiblePoints"> The maximum possible score for the song - what they would get if they got Perfect on every note! </param>
        /// <param name="_streak">Highest Streak The User Attainedhe User Attained</param>
        /// <param name="_notesHit">Different Types Of Notes Hit</param>
        public ResultsScreen(int _score, int _possiblePoints, int _streak, int[] _notesHit)
        {
            score = _score;
            possiblePoints = _possiblePoints;
            streak = _streak;
            notesHit = _notesHit;

            // Calculate the good stuff
            float ratio = Convert.ToSingle(score) / possiblePoints; // We gotta get the score ratio first, for readability purposes
            percentage = Convert.ToInt32(ratio * 100);  // While it's cool to know if you scored 95.543754745341%, in most other cases 95% will do.
            // Set images based on said good stuff
            int imageIndex; // The index of the picture in those texture arrays that will actually be displayed
            // Starting at 0%, every increase in 25% nets the player cool new images
            imageIndex = percentage / 25;
            // Validate the image index - it's gotta be within the bounds of the image lists
            if (imageIndex < 0)
                imageIndex = 0;
            if (imageIndex > backgrounds.Count - 1) // backgrounds is used aribitrarily, as all image arrays are to be the same length
                imageIndex = backgrounds.Count - 1;
            // Set the images
            backgroundImage = backgrounds[imageIndex];
            rankingImage = rankings[imageIndex];
            rewardImage = rewards[imageIndex];

            // Set time elapsed to 0
            totalSeconds = 0f;
        }

        /// <summary>
        /// Update: This method checks out that time, yo, so the right text/images can be displayed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Update the time (talk about rude - this class checks its watch several times every second!)
            totalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (InputManager.Pressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                SceneManager.EndScene();
            }
        }

        /// <summary>
        /// Draw: This method displays the good stuff
        /// I've, uh, never worked with timing before. 
        /// Everything I've programmed in this format has occurred immediately after input, but never with any delays.
        /// Let's see if this conditional chain works...
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            /* Here's the plan: Since this classs will always add stuff to the screen, never remove, each display element will be tied to a time.
             * If the specified time for a given element has passed, it will be displayed!
             */

            // Variable dictionary I guess
            string text;    // Text being displayed at any given time
            Vector2 textPos = Position + new Vector2(30, 200);   // The POSITION of TEXT
            SpriteFont font = DannDannRevolution.GameFont;  // Because I don't wanna type "DannDannRevolution.GameFont" over and over
                                                            // (even with Visual Studio's cool autocomplete)

            /* The two-second mark is where it really gets good. 
             * Unfortunately, the code ran at two seconds can't be placed at the two-second mark in the code, 
             * as the new background must be drawn before the text that goes on top of it (please understand)
             */
            if (totalSeconds >= 2)
            {
                // Display appropriate background, ranking, and reward - in that order
                spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, DannDannRevolution.VIRTUAL_SCREEN_WIDTH, DannDannRevolution.VIRTUAL_SCREEN_HEIGHT), Color.White);
                spriteBatch.Draw(rankingImage, new Rectangle(30, 475, rankingImage.Width * SCALE, rankingImage.Height * SCALE), Color.White);
                spriteBatch.Draw(rewardImage, new Rectangle(rankingImage.Width * SCALE, 400, rewardImage.Width * (SCALE - 2), rewardImage.Height * (SCALE - 2)), null, Color.White, Convert.ToSingle(Math.PI / 6), Vector2.Zero, SpriteEffects.None, 1);
                //spriteBatch.Draw(rewardImage, new Vector2(800, 400), Color.White);    // Normal, boring
            }

            // The first condition isn't necessary, but it's there for convenience!
            if (totalSeconds >= 0)
            {
                text = "Congratulations!";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(0, font.MeasureString(text).Y);
            }

            // After a given amount of seconds (at the time of writing, one), display an intriguing message
            if (totalSeconds >= 1)
            {
                text = "Your Ranking is";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(font.MeasureString(text).X, 0);
            }
            if (totalSeconds >= 1.25)
            {
                text = ".";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(font.MeasureString(text).X, 0);
            }
            if (totalSeconds >= 1.5)
            {
                text = ".";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += new Vector2(font.MeasureString(text).X, 0);
            }
            if (totalSeconds >= 1.75)
            {
                text = ".";
                spriteBatch.DrawString(font, text, textPos, Color.White);
                textPos += font.MeasureString(text);
            }

            if (totalSeconds >= 3)
            {
                spriteBatch.DrawString(font, "Statistics", new Vector2(1280, 40), Color.Black);
            }

            if (totalSeconds >= 4)
            {
                spriteBatch.DrawString(font, "Score:", new Vector2(1280, 140), Color.White);
                text = score.ToString();
                spriteBatch.DrawString(font, text, new Vector2(1280, 240), Color.Blue);
                spriteBatch.DrawString(font, " / " + possiblePoints.ToString(), new Vector2(1280, 240) + new Vector2(font.MeasureString(text).X, 0), Color.White);
            }

            if (totalSeconds >= 5)
            {
                text = "Streak: ";
                spriteBatch.DrawString(font, text, new Vector2(1280, 340), Color.White);
                spriteBatch.DrawString(font, streak.ToString(), new Vector2(1280, 340) + new Vector2(font.MeasureString(text).X + 10,0), Color.Blue);
            }
            if (totalSeconds >= 6)
            {
                spriteBatch.DrawString(font, "Breakdown", new Vector2(1280, 440), Color.Black);
            }
            if (totalSeconds >= 6.5)
            {
                text = "Perfect: ";
                spriteBatch.DrawString(font, text, new Vector2(1280, 540), Color.White);
                spriteBatch.DrawString(font, notesHit[0].ToString(), new Vector2(1280, 540) + new Vector2(font.MeasureString(text).X + 10, 0), Color.Blue);
            }
            if (totalSeconds >= 7)
            {
                text = "Good: ";
                spriteBatch.DrawString(font, text, new Vector2(1280, 640), Color.White);
                spriteBatch.DrawString(font, notesHit[1].ToString(), new Vector2(1280, 640) + new Vector2(font.MeasureString(text).X + 10, 0), Color.Blue);
            }
            if (totalSeconds >= 7.5)
            {
                text = "Ok: ";
                spriteBatch.DrawString(font, text, new Vector2(1280, 740), Color.White);
                spriteBatch.DrawString(font, notesHit[2].ToString(), new Vector2(1280, 740) + new Vector2(font.MeasureString(text).X + 10, 0), Color.Blue);
            }
            if (totalSeconds >= 8)
            {
                text = "Bad: ";
                spriteBatch.DrawString(font, text, new Vector2(1280, 840), Color.White);
                spriteBatch.DrawString(font, notesHit[3].ToString(), new Vector2(1280, 840) + new Vector2(font.MeasureString(text).X + 10, 0), Color.Blue);
            }
            if (totalSeconds >= 8.5)
            {
                text = "Missed: ";
                spriteBatch.DrawString(font, text, new Vector2(1280, 940), Color.White);
                spriteBatch.DrawString(font, notesHit[4].ToString(), new Vector2(1280, 940) + new Vector2(font.MeasureString(text).X + 10, 0), Color.Blue);
            }
            if (totalSeconds >= 10)
            {
                spriteBatch.DrawString(font, "Press Enter To Continue...", new Vector2(10, 950), Color.White);
            }
            //Wow, That Was Alot Of Animating, But It Sure Does Look Nice
        }
    }
}