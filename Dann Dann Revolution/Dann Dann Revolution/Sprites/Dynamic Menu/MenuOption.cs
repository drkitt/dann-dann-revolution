/*
 * Dann Dann Revolution Team
 * File Created: January 8, 2018 (original time of writing: October 6, 2017
 * MenuOption: This class represents a single option for the cool new dynamic menu
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dann_Dann_Revolution
{
    internal abstract class MenuOption : ISprite
    {
        // Instance vvvvvvvvvvariables
        /// <summary> If the size of the menu option is A and this variable is B, A/B is subtracted from A to put a gap between options </summary>
        protected readonly int GAP_DIVISOR = 20;
        /// <summary> Text is pushed this many pixels to the side and up </summary>
        protected readonly int PADDING = 10;
        /// <summary> A 1x1 white texture that'll be stretched a whole lot later 
        /// Also, it doesn't necessarily have to be white - it can be tinted using the Color parameter of the SpriteBatch.Draw method </summary>
        protected Texture2D pixel;
        /// <summary> The text the menu option displays </summary>
        protected string text;
        /// <summary> How fast the sprite goes when moving, measured in pixels per second. 
        /// Who knows, this might become part of the ISprite interface! </summary>
        public int Speed;
        /// <summary> The font used to display the text </summary>
        /// Possible TODO: Change the font to different sizes based on the size of the option! 
        /// For now, though, it's hardcoded into each individual menu option.
        public SpriteFont Font { get; set; }
        /// <summary> The sprite's position on screen </summary>
        public Vector2 Position { get; set; }
        /// <summary> The size of the menu option </summary>
        public Vector2 Size { get; set; }
        /// <summary> The colour of the menu option background </summary>
        public Color BGColour { get; set; }
        /// <summary> The colour of the menu option text</summary>
        public Color TextColour { get; set; }

        /// <summary>
        /// This constructor sets up the menu option!
        /// </summary>
        public MenuOption()
        {
            // Set the size to an arbitrary initial value, even though 5/7 times this value will be overwritten
            Size = new Vector2(216, 96);

            // Set up the ~TEXTURE~
            // You may think this requires summoning ContentManager, but we'll be fiddling with a much more forbidden object - GraphicsDevice!
            GraphicsDevice graphicsDevice = GameServices.Get<GraphicsDevice>();
            // Create the texture!
            pixel = new Texture2D(graphicsDevice, 1, 1);
            // Fill the texture with white (analogous to using the paint bucket tool in MSPaint, I think)
            Color[] colorData = { Color.White };
            pixel.SetData<Color>(colorData);
        }

        /// <summary>
        /// Draw: This method displays the menu option on screen
        /// </summary>
        /// <param name="spriteBatch"> 
        /// don't 👏 call 👏 yourself 👏 a 👏 SpriteBatch 👏 if 👏 you're 👏 not 👏 a 👏 MonoGame 👏 object 👏 used 👏 to 👏 display 👏 graphics
        /// </param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            /* Create a rectangle to make drawing the sprite easier
             * (The fact that Size.Y is cut off a little bit means that the menu won't quite be as tall as implied by DynamicMenu.Position, 
             * but we needn't worry about that) */
            Rectangle destRect = new Rectangle((int)Position.X, (int)(Position.Y + (Size.Y / GAP_DIVISOR) / 2), (int)Size.X, (int)(Size.Y - Size.Y / GAP_DIVISOR));
            // Draw the fancy texture at the desired size and colour
            spriteBatch.Draw(pixel, destRect, BGColour);
            // Texture will be drawn left justified inside the menu option rectangle
            Vector2 textPos = new Vector2(Position.X + PADDING, Position.Y - PADDING);   
            // Draw the text
            spriteBatch.DrawString(Font, text, textPos, TextColour);
        }

        /// <summary>
        /// Option: This method is called when the player selects this option.
        /// What does it do? Depends on the option! I can't tell you here, in the base class!
        /// </summary>
        public abstract void Option();
    }
}