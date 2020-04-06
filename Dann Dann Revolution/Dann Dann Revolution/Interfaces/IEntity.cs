/*
 * Alex Kitt
 * December 19, 2017 (original time of writing: July 30, 2017)
 * CompSci 40S
 * Dann Dann Revolution
 * Entity: This interface literally just makes sure that implementing objects have an Update method.
 * Why does this need its own file?
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
    internal interface IEntity
    {
        /// <summary> Does the 'thinking' for the sprite... that's my best explanation for now. </summary>
        /// <param name="gameTime"> Object that keeps track of the time elapsed in real life </param>
        void Update(GameTime GameTime);
    }
}