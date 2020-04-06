/*
 * Dann Dann Revolution team I think
 * January 23, 2017 - hey, didn't class end two minutes ago?
 * COMPUTAH SCIANCE
 * 
 * IntroScreen: This class is dedicated to David Kim. It waits a couple seconds, and then opens up a specified Scene! 
 * The culmination of my high-school computer science career starts and ends here.
 * 
 * Update: Since I'm unable to get the video player to play more than one video during the program's life, this class is useless 
 * (for its intended use case was to delay the appearance of the title screen so we could play a short intro video). 
 * I'm keeping it around for reference purposes, though - who knows when I'll have to make a Scene after a delay?
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Dann_Dann_Revolution
{
    class IntroScreen : IEntity
    {
        // Instants of variables
        /// <summary> How much time, in seconds, has passed since the object's creation </summary>
        private float time = 0;
        /// <summary> The Scene this cool dude will add to the Scene list after... </summary>
        private Scene scene;
        /// <summary> ...this many seconds. </summary>
        private int delay;

        /// <summary>
        /// IntroScreen: This method creates a Scene but doesn't actually add it to THE LIST until after a delay
        /// </summary>
        /// <param name="scene"> The Scene to be saved </param>
        /// <param name="delay"> How many seconds the player must wait </param>
        public IntroScreen(Scene scene, int delay)
        {
            // Store parameter values to conveniently named instance variables
            this.scene = scene;
            this.delay = delay;
        }

        /// <summary>
        /// Update: This method doesn't do much; it just waits a while and then STARTS
        /// </summary>
        /// <param name="gameTime"> Displays a snapshot of- sorry, I'm tearing up! </param>
        public void Update(GameTime gameTime)
        {
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (time > delay)
                SceneManager.AddNewScene(scene);    // Shortest AddNewScene call of all time?
        }
    }
}