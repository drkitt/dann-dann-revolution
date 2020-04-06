/*
 * Dann Dann Revolution Team
 * June 16th, 2028
 * CompSci 75G
 * Dane DNA Resolution
 * 
 * InputEffects: Okay, THIS is my final class (probably). 
 * I dedicate this one to the guy who was in Team Alpha in the 2015-16 year and moved to Spain midway through.
 * This class displays a little glowy effect around the on-screen arrows when the user presses the arrow key, adding a bit of pizazz 
 * to the main game screen.
 * (a common complaint among playtesters is that they didn't like how the notes just disappeared upon the user's button-hitting)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Dann_Dann_Revolution: This namespace- wait! This namespace is the entire project!
/// </summary>
namespace Dann_Dann_Revolution
{
    internal class InputEffects : IEntity, ISprite
    {
        // If anyone asks, this is a data structure. Not at all am I neglecting the need to put classes in separate files
        // And before you ask, I can't make this a struct. It messes with the foreach loops!
        private class ButtonImage
        {
            public bool Visible { get; set; }       // Whether the image can be seen
            public Keys Key { get; set; }           // The key pressed to make this image visible
            public float Timer { get; set; }        // How long it's been on screen for
            public Vector2 Position { get; set; }   // Where it is (there it is!)
            public Texture2D Image { get; set; }    // The image being displayed
        }

        /// <summary> How many pixels down the screen the images should appear </summary>
        private readonly int Y_POS = 4;
        /// <summary> How long the effect should stay on screen (it's approximately right in between the timing for Perfect and Good!) </summary>
        private readonly float TIMER = 0.16f;
        /// <summary> The keys this class is dealing with. Stored here for "convenience" purposes </summary>
        private static readonly List<Keys> KEYS;
        /// <summary> The position of each image that represents a button </summary>
        private static readonly List<int> KEY_POSITIONS;
        /// <summary> All images for the input effects </summary>
        private static readonly List<Texture2D> IMAGES;
        /// <summary> Array of images for the buttons </summary>
        private List<ButtonImage> buttonImages = new List<ButtonImage>();
        /// <summary> Wherem'st've is this object?????? </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// InputEffects: This constructor loads up the content for the class
        /// </summary>
        public InputEffects()
        {
            // Initialize images
            for (int i = 0; i < KEYS.Count(); i++)
            {
                buttonImages.Add(new ButtonImage());
                buttonImages[i].Visible = false;
                buttonImages[i].Key = KEYS[i];
                buttonImages[i].Timer = 0.0f;
                buttonImages[i].Position = new Vector2(KEY_POSITIONS[i], Y_POS);
                buttonImages[i].Image = IMAGES[i];
            }

            // Set position to that of the first image, just in case something's wondering about it
            Position = buttonImages[0].Position;
            // That's it...
        }
            // ... For this version of the constructor! MUAHAHAHA!
        static InputEffects()
        {
            // Initialize readonly variables 
            // (note that these Lists have their elements arranged in the order they will appear on screen - left, up, right, down)
            KEYS = new List<Keys> { Keys.Left, Keys.Up, Keys.Right, Keys.Down };
            KEY_POSITIONS = new List<int> { 1240, 1420, 1580, 1750 };
            ContentManager contentManager = GameServices.Get<ContentManager>(); // Yay for content manager
            IMAGES = new List<Texture2D>
            {
                contentManager.Load<Texture2D>("Graphics/dann_perfect_2_left"),
                contentManager.Load<Texture2D>("Graphics/dann_perfect_2_up"),
                contentManager.Load<Texture2D>("Graphics/dann_perfect_2_right"),
                contentManager.Load<Texture2D>("Graphics/dann_perfect_2_down"),
            };
        }

        /// <summary>
        /// Update: This method checks for input and plans ahead accordingly
        /// </summary>
        /// <param name="gameTime"> estimates 1/√x, the reciprocal (or multiplicative inverse) of the square root of a 32-bit floating-point number x in IEEE 754 floating-point format.
        /// Just kidding! It keeps track of time and stuff </param>
        public void Update(GameTime gameTime)
        {
            // For each button image, update its state based on input and time
            foreach (ButtonImage image in buttonImages) // haha eys
            {
                if (InputManager.Pressed(image.Key))
                    image.Visible = true;
                if (image.Visible)
                    if (image.Timer > TIMER)
                        image.Visible = false;
                    else
                        image.Timer += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
                else
                    image.Timer = 0.0f;
            }
        }

        /// <summary>
        /// Draw: This method displays effects in a cool way
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ButtonImage image in buttonImages)
                if (image.Visible)
                    spriteBatch.Draw(image.Image, image.Position, Color.White);
        }
    }
}