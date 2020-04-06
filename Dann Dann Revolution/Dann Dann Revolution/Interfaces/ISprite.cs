/*
 * Alex Kitt
 * December 19, 2017 (original time of writing: July 28, 2017 - EMOJI MOVIE 😂😂😂😂😂👌💩💩💩💯💯💯💯💯💯💯)
 * CompSci 40S
 * Dann Dann Revolution
 * Sprite: This interface BILLY MAYS GUARANTEES that every sprite (as in, any object that implements this interface) 
 * will have a method for drawing itself and a position on the screen
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
    internal interface ISprite
    {
        // Properties that all sprites gotta have
        /// <summary> The coordinates of point where the drawing of the sprite starts - 
        /// generally the top-left of the sprites bounding rectangle, but there can be exceptions for more irregularly-shaped sprites. </summary>
        Vector2 Position { get; set;}
        
        /// <summary> Displays the sprite on-screen </summary>
        /// <param name="spriteBatch"> MonoGame object used to display graphics </param>
        void Draw(SpriteBatch spriteBatch);
    }
}