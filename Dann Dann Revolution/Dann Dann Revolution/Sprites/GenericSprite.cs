/*
 * Dann Dann Revolution Team
 * File Created: December 19, 2017 (original time of writing: August 8, 2017)
 * GenericSprite: This class is reserved only for the most generic of sprites. 
 * These sprites literally just display their image every frame, not animating nor receiving input nor anything!
 * 
 * Mike Hodges Here!
 * Well actualy Alex, I have now updated the code so that if you want to you can easily make these animated.
 * Guess it's not so 'Generic' anymore xD
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Dann_Dann_Revolution
{
    internal class GenericSprite : IEntity, ISprite
    {
        // Instance variables
        /// <summary> The image the sprite displays </summary>
        private Texture2D image { get; set; }
        /// <summary>
        /// Back end variable for the postion inharited by ISprite
        /// </summary>
        private Vector2 backEndPosition;
        /// <summary>
        /// Loads postion and updates destination rectangle if postion is changed
        /// </summary>
        public Vector2 Position
        {
            get { return backEndPosition; }

            set
            {
                backEndPosition = value;
                destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
            }
        }
        /// <summary> The sprite's coordinates as well as its size </summary>
        public Rectangle destinationRectangle = new Rectangle();


        /// <summary>
        /// GenericSprite: Psst... this isn't the actual constructor. You see, this class uses multiple constructors that share some code. 
        /// This specific constructor contains the code that they both share, and can't actually be called outside of this class
        /// </summary>
        /// <param name="imagePath"> The path to the image the sprite displays, relative to ContentManager's root directory </param>
        private GenericSprite(string imagePath)
        {
            // Summon contentManager from the unholy depths of GameServices
            ContentManager contentManager = GameServices.Get<ContentManager>(); // It has returned!
            // Load the CONTENT
            image = contentManager.Load<Texture2D>(imagePath);
        }

        /// <summary>
        /// GenericSprite: This method creates an object that displays an image on-screen, and really nothing else
        /// </summary>
        /// <param name="imagePath"> The path to the image the sprite displays, relative to ContentManager's root directory </param>
        /// <param name="position"> The sprite's coordinates in TWO-DIMENSIONAL SPACE </param>
        public GenericSprite(string imagePath, Vector2 position) : this(imagePath)
        {
            /* 
             * Notice the ": this(imagePath)" at the end of the method's signature? That's a call for help!
             * I mean, it's a call to the private constructor up above!
             */
            // Load one more parameter... the easy way
            Position = position;
            // Give the Rectangle the lame-o value of Position and the actual image size
            destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
        }

        /// <summary>
        /// What's this? An Overload? Yep! This constructor does the exact same thing as the other one, 
        /// except instead of passing a set of coordinates you pass in the sprite's coordinates AND its size. 
        /// Truly revolutionary - this lets you resize images without messing with MSPaint or other industry-standard programs
        /// </summary>
        /// <param name="imagePath"> The path to the image the sprite displays, relative to ContentManager's root directory </param>
        /// <param name="destinationRectangle"> The sprite's location and size, in that order </param>
        public GenericSprite(string imagePath, Rectangle destinationRectangle) : this(imagePath)
        {
            // Load one more parameter... the easy way
            this.destinationRectangle = destinationRectangle;
            // Set Position value based on that of destinationRectangle
            Position = new Vector2(destinationRectangle.Left, destinationRectangle.Top);
        }

        /// <summary>
        /// Update: This method does nothing! Muahahaha!
        /// </summary>
        /// <param name="gameTime"> 🕴 </param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draw: This method draws the sprite's image on the screen
        /// </summary>
        /// <param name="spriteBatch"> MonoGame object used to display graphics </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, destinationRectangle, Color.White);
        }
    }
}