//Dann Dann Revolution Team
//File Created: 21/12/2017
//This Class Scores Notes That Are Pressed Against Thier Time Stamps
//  It Is Also Responceable For Drawing The Scoring Animations

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Dann_Dann_Revolution
{
    class ScoringSystem : IEntity, ISprite
    {
        private const double ACCEPTABLE = 0.40;//THE AMOUNT OF TIME THE ARROW IS OVERTOP OF THE KEY
        private const double BAD = 0.30;
        private const double OK = 0.25;
        private const double GOOD = 0.20;
        private const double PERFECT = 0.12;

        private int points;                     //Points For This Song
        private List<Note> notes;               //Notes For Scoring System To Score Off Of
        public Vector2 Position { get; set; }   //Postion..... Useless
        private GenericSprite rating;           //Rating Image

        //Stuff For Ratings
        private int streak;                     //Current Streak
        private int last;                       //Last Type Of Rating Scored
        private int longestStreak;              //Longest Streak The User Achived
        private int[] notesHit;                 //1-Perfect, 2-Good, 3-Ok, 4-Bad, 5-Missed

        //Stuff For The Timer Below
        private const float TIMER = 2.0f;       //How Long The Timer Will Go (Seconds)
        private float timer;                    //Tracking Time
        private float elapsed;                  //Tracking Time Since Last Update Method Was Called
        private bool timerActive;               //If The Image Should Be Moving Or Not / If The Timer Should Be Active

        /// <summary>
        /// Constructor For Notes Class
        /// </summary>
        /// <param name="_notes">Notes For Scoring System To Score Off Of</param>
        public ScoringSystem(List<Note> _notes) {
            points = 0;
            notes = _notes;
            timer = 0.0f;
            elapsed = 0.0f;
            timerActive = false;
            streak = 0;
            last = 0;
            longestStreak = 0;
            notesHit = new int[5];

            rating = new GenericSprite("Graphics/bad", new Vector2(1, -600));       //Render Rating Out Of Screen xD
        }

        /// <summary>
        /// Every Time A Key Is Pressed
        /// </summary>
        /// <param name="keyPressed">Which direction key was pressed</param>
        public void KeyPress(Keys keyPressed) {
            foreach (Note i in notes)
            {                                                                                           
                if (!i.getScored() && keyPressed == i.getDirection()//Check if note meets requirements to be scored
                    && GameScreen.totalSeconds - i.getTime() <= ACCEPTABLE && GameScreen.totalSeconds - i.getTime() >= -1 * ACCEPTABLE)
                {
                    float bracket = GameScreen.totalSeconds - i.getTime();
                    bracket = (bracket < 0) ? bracket * -1 : bracket;//Absloute Value
                    Score(bracket);//Score the note
                    i.Score();//Mark the note as scored
                    break;
                }
            }
        }

        /// <summary>
        /// Scores notes based on accuracy
        /// </summary>
        /// <param name="time">How close the key was to the marked time</param>
        private void Score(float time) {
            timerActive = true;     //Set animtion timer to true
            if (time <= PERFECT)
            {
                Streak(1);
                points += 50;                                                         //Score appropriote points
                rating = new GenericSprite("Graphics/perfect", new Vector2(700, 300));//Load the correct image into rating
                notesHit[0]++;
            }
            else if (time <= GOOD)
            {
                Streak(1);
                points += 25;
                rating = new GenericSprite("Graphics/good", new Vector2(700, 300));
                notesHit[1]++;
            }
            else if (time <= BAD)
            {
                Streak(4);
                points -= 25;
                rating = new GenericSprite("Graphics/bad", new Vector2(700, 300));
                notesHit[3]++;
            }
            else
            {
                notesHit[2]++;
            }
        }

        /// <summary>
        /// System call that updates the scoring system
        /// </summary>
        /// <param name="GameTime">GameTime object</param>
        public void Update(GameTime GameTime)
        {
            //Check for arrow key presses
            if (InputManager.Pressed(Keys.Up)) {
                KeyPress(Keys.Up);
            }
            if (InputManager.Pressed(Keys.Down))
            {
                KeyPress(Keys.Down);
            }
            if (InputManager.Pressed(Keys.Left))
            {
                KeyPress(Keys.Left);
            }
            if (InputManager.Pressed(Keys.Right))
            {
                KeyPress(Keys.Right);
            }

            foreach (Note i in notes)//Check if any notes have been missed
            {
                if (!i.getScored() && GameScreen.totalSeconds - i.getTime() >= -1 * ACCEPTABLE + .7)
                {
                    Streak(5);
                    points += -50;
                    i.Score();
                    rating = new GenericSprite("Graphics/missed", new Vector2(700, 300));
                    notesHit[4]++;
                }
            }

            if (timerActive)//The timer for animating the rating
            {
                elapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;     //Add time since last update to the total time note has been active
                timer += elapsed;
                //Make the rating slide downwards
                rating.Position += new Vector2(0, 1 * Convert.ToSingle(GameTime.ElapsedGameTime.TotalSeconds * 50));
                if (timer > TIMER)//If timer is complete
                {
                    timer = 0;//reset timer
                    timerActive = false;
                }
            }
        }
        /// <summary>
        /// Calcualtes The Users Streak
        /// </summary>
        /// <param name="type">type of rating they got on that press</param>
        private void Streak(int type) {
            if (last == 0)//Start of game check
            {
                last = type;
                streak++;
            }
            else if (last == type && last != 5)
                streak++;
            else {
                streak = 1;
                last = type;
            }

            //Update Max Streak If Applicable
            if (longestStreak < streak)
            {
                longestStreak = streak;
            }
        }

        /// <summary>
        /// System Call To Draw The Stuff Because Drawers Gonna Draw Drawings
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            rating.Draw(spriteBatch);
            if (last != 5)
            {
                spriteBatch.DrawString(DannDannRevolution.GameFont, "x" + streak, new Vector2(rating.Position.X + rating.destinationRectangle.Width - 30, rating.Position.Y - 12), Color.Black);//Psst - add the following parameters to rotate the streak caption! Convert.ToSingle(-Math.PI / 12), Vector2.Zero, 1.0f, SpriteEffects.None, 1);
            }
        }

        public int getPoints() { return points; }//Get The Total Points 
        public int getLongestStreak() { return longestStreak; }//Get Longest Streak
        public int[] getNotesHit() { return notesHit; }//Get Notes Hit
    }
}
