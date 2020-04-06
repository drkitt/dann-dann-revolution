//Dann Dann Revolution Team
//File Created: 1/8/2018
//This is the screen that the user can actualy play the game on

/*
 * Arrow Postions:
 * Left:  1250 
 * Up:    1430
 * Right: 1590
 * Down:  1760
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
    class GameScreen : ISprite, IEntity
    {
        public static float totalSeconds;   //Total Seconds Since Song Has Started
        public Vector2 Position{ get; set;} //Postion Of Game Screen... More Or Less Usless
        private SongLoader songloader;      //SongLoader Class To Load Selected Song
        private ScoringSystem scoringsystem;//Systen That Will Score KeyPresses
        private List<Note> ActiveArrows;    //All Arrows That Need To Be Displayed
        private Song song;                  //Contains Audio File Of Song
        private bool songPlaying;           //If A Song Is Playing Or Not

        public GameScreen(string XMLFILE) {
            ContentManager contentmanager = GameServices.Get<ContentManager>();
            song = contentmanager.Load<Song>("Music/" + XMLFILE);
            songloader = new SongLoader(XMLFILE);
            scoringsystem = new ScoringSystem(songloader.getNotes());
            ActiveArrows = new List<Note>();
            totalSeconds = 0f;
            songPlaying = false;
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play(song);
        }

        /// <summary>
        /// System call that will update all visuals
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw Score
            spriteBatch.DrawString(DannDannRevolution.GameFont, "Score: " + scoringsystem.getPoints().ToString(), new Vector2(10, 970), Color.White);
            //Draw Arrows
            foreach (Note i in ActiveArrows)
            {
                i.Draw(spriteBatch);
            }
            // Show the totalSeconds variable on screen, if you're in the mood for debugging
            //spriteBatch.DrawString(DannDannRevolution.GameFont, "Time: " + totalSeconds.ToString(), new Vector2(100, 700), Color.White);
            scoringsystem.Draw(spriteBatch);
        }

        /// <summary>
        /// System call that will update the class
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public void Update(GameTime GameTime)
        {
            totalSeconds += (float)GameTime.ElapsedGameTime.TotalSeconds;//Add seconds since last update call to the total seconds since the song started

            if (!songPlaying)
            {
                if (MediaPlayer.PlayPosition.Milliseconds == 0 )
                    totalSeconds = 0f;
                else
                    songPlaying = true;
            }
            
            foreach (Note i in songloader.getNotes())
            {
                if (totalSeconds > i.getTime() - 3.5 )//The Value There Is How Much Before Their Time Notes Should Start Flowing
                {
                    if (!ActiveArrows.Contains(i))//If the note is not already active
                    {
                        ActiveArrows.Add(i);//Make note active
                    } 
                }
            }

            //Update Each Visable Note                                         
            for (int i = ActiveArrows.Count - 1; i >= 0; i--)                  
            {                                                                  
                //Removed if scored                                                                                                    
                if (ActiveArrows[i].getScored())
                {
                    ActiveArrows.RemoveAt(i);
                }
                else//Update The Notes
                {
                    ActiveArrows[i].Update(GameTime);
                }
            }
            scoringsystem.Update(GameTime);//Update Scoring System

            if (totalSeconds > songloader.getDuration())//Quit scene if song duration is finnished
            {
                MediaPlayer.Stop();
				
                // Ending the last two Scenes before moving on to the results Scene makes it so the Scene right behind the results Scene is the title screen Scene. hooh!
                SceneManager.EndScene();
                SceneManager.EndScene();
                SceneManager.AddNewScene(new Scene(new List<IEntity> {
                    //new GenericSprite("Graphics/Results screen/In the news", new Rectangle(0, 0, DannDannRevolution.VIRTUAL_SCREEN_WIDTH, DannDannRevolution.VIRTUAL_SCREEN_HEIGHT)),
                    new ResultsScreen(scoringsystem.getPoints(), songloader.getNotes().Count * 50, scoringsystem.getLongestStreak(), scoringsystem.getNotesHit()),
                }));
            }
        }
    }
}
