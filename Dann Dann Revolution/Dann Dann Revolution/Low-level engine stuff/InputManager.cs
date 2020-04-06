/*
 * Alex Kitt
 * December 19, 2017 (Original time of writing: sometime around July 26th, 2017)
 * CompSci 40S
 * Dann Dann Revolution
 * InputManager: This class makes it slightly easier to find out if a button is being pressed!
 * 'Cause seeing if a key is pressed this frame and not last frame is SO HARD, 
 * this class has a convenient little method that just lets you pass a key in as a parameter, and then returns whether a valid press has been made.
 * Also, this opens the door to easily finding out if a key is being held down later on, which is a plus.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dann_Dann_Revolution
{
    internal static class InputManager
    {
        // Instance variables? Instance variables.
        /// <summary> The state of the keyboard this frame - whether each key is being held down </summary>
        private static KeyboardState currentState = Keyboard.GetState();    // The keyboard state obtained here is a dummy one - 
                                                                            // it has an initial value so Update's routine of 
                                                                            // assigning currentState's value to previousState 
                                                                            // will work the first time around
        /// <summary> The state of the keyboard last frame - used so methods like Pressed only return true once! </summary>
        private static KeyboardState previousState;

        /// <summary>
        /// Update: This method gets the state of the keyboard, and puts the previous frame's keyboard state into a separate variable
        /// </summary>
        public static void Update()
        {
            // The keyboard state from the last time we called this method is old news!
            previousState = currentState;
            // The keyboard state of this frame is, uh, new news.
            currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Pressed: This method queries the keyboard state of this frame and the last frame to figure out if a specified button has been pressed
        /// </summary>
        /// <param name="key"> The key the calling method is wondering about </param>
        /// <returns> Whether that key had just been pressed </returns>
        public static bool Pressed(Keys key)
        {
            // SIMPLE!
            // Yeah, this entire class only exists so I only have to pass one parameter 
            // (this object, or maybe even no parameters if I can make this part of the BaseGame reference)
            // to certain functions instead of two (the keyboard state duo)
            return (currentState.IsKeyDown(key) && previousState.IsKeyUp(key));
        }

        /// <summary>
        /// Held: This method queries the keyboard state of this frame to figure out if a specified button is currently being pressed / held down
        /// </summary>
        /// <param name="key"> The key the calling method is wondering about </param>
        /// <returns> Whether that key is being held </returns>
        public static bool Held(Keys key)
        {
            return currentState.IsKeyDown(key);
        }
    }
}
