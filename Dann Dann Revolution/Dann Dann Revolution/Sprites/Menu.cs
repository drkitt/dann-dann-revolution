/*
 * Dann Dann Revolution Team
 * File Created: December 19, 2017 (original time of writing: July 10, 2017)
 * Menu: This class presents a list of options to the user and gives them a cute little cursor to select one with.
 * 
 * Update July 21, 2017 (still December 19, 2017, actually): We're shaking things up! 
 * To deal with the whole "different menus do different things" dillemma, 
 * I'm making each type of menu a seperate class, derived from an abstract Menu class!
 * Before, there was a really convoluted solution involving 
 * static methods in this class being put into delegates in the main class and then passed back here.
 * That worked, but it was really stupid. Now, slightly less stupidly, I'm having each type of menu be a derived class from the og menu,
 * and its unique functionality will be in the form of methods unique to that class.
 * There's still some delegate trickery going on, but it's much more pleasant. Each derived menu class needs a method that shoves its unique methods 
 * into a delegate array, and then the methods are ran in the same fashion as before!
 * 
 * Update December 19, 2017: It should be known that this menu is slightly outdated - 
 * there's a super cool work-in-progress dynamic menu I'm working on that allows for unique code in each menu but has no need for inheritance - 
 * but this menu will work fine for Dann Dann Revolution.
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
// For nostalgia's sake: using namespace std;

namespace Dann_Dann_Revolution
{
    internal abstract class Menu : IEntity, ISprite
    {
        // Instance variables - all these cool dudes are protected because all the derived classes just use the exact same variables!
        // Don't worry, this works is just like implementing an interface, and no data security is compromised... I think.

        /// <summary> Number representing which option the cursor is on </summary>
        protected int index = 0;
        /// <summary> Loop counter :) </summary>
        protected int count;

        /// <summary> Array containing all menu options </summary>
        protected string[] options;

        /// <summary> Array containing the position of each menu item </summary>
        protected Vector2[] option_positions;
        /// <summary> Depending on the alignment (more on that later), this is either the top-left, top-centre, or top-right of the menu </summary>
        public Vector2 Position { get; set; }

        /// <summary> Whatever font the menu ends up using. 
        /// Pro-tip: Use Comic Neue. It makes your lemonade stand look like a Fortune 500 company!</summary>
        protected SpriteFont font;
        /// <summary> The colour (that's spelt with a u, I'll have ya know) of the menu text </summary>
        protected Color textColour;
        /// <summary> You know, it's that little picture that appears next to the option you're about to select! </summary>
        protected Texture2D cursorImage;
        /// <summary> The size of the cursor... Yay for self-documenting code! </summary>
        protected Point cursorSize;
        /// <summary> The size and position of the cursor. Yes, it contains cursor_size! </summary>
        protected Rectangle cursorRect;

        /// <summary> Whether the text's origin represents the left, centre, or right (hence the names) of the first line of text </summary>
        public enum TextAlignment { left, centre, right }

        // You thought methods were tough? How about arrays? 
        /// <summary>
        /// Well, this is an array of methods (technically delegates, but whatevs) 
        /// that represent what actually happens when the player selects something
        /// </summary>
        // 'Action' means a delegate whose method doesn't take any parameters and doesn't return anything!
        protected Action[] option_methods;

        /// <summary>
        /// __init__ function - er, I mean, constructor
        /// </summary>
        /// <param name="cursorImagePath"> The path (relative to ContentManager's root directory)
        /// to that little picture that appears next to the option you're about to select</param>
        /// <param name="fontPath"> The path (relative to ContentManager's root directory) to whatever font the menu ends up using </param>
        /// <param name="options"> Array containing all menu options </param>
        /// <param name="position"> Depending on the alignment (more on that later), 
        /// this is either the top-left, top-centre, or top-right of the menu </param>
        /// <param name="alignment"> How the text is aligned relative to the origin. 
        /// This parameter is the only one not copied to an instance variable, as it's only used in the constructor! </param>
        /// <param name="textColour"> The colour (that's spelt with a u, I'll have ya know) of the menu text </param>
        public Menu(string cursorImagePath, string fontPath,
            string[] options, Vector2 position, TextAlignment alignment, Color textColour)
        {
            // Woooooooooooah look, it's the locals
            /// <summary> Turns out these fancy tags don't work with local variables (darn!), 
            /// so we're going with old-style variable dictionaries for those. I think they looked better anyways. </summary>

            int side;   // The length of one side of the square containing the cursor
            float alignmentMultiplier;  // Fancy mathy variable - it's explained in more detail when it's defined and used, 
                                        // but the x pos of the text is multiplied by this in order to align the text
            ContentManager contentManager = GameServices.Get<ContentManager>();

            // The best part (plus, it's easy!): Initialize instance variables based on parameters
            this.options = options;
            Position = position;
            this.textColour = textColour;

            // LOAD THE CONTENT, KRONK
            this.cursorImage = contentManager.Load<Texture2D>(cursorImagePath);
            this.font = contentManager.Load<SpriteFont>(fontPath);

            // Get the alignment multiplier based on the alignment
            switch (alignment)
            {
                case TextAlignment.left: alignmentMultiplier = 0; break;
                case TextAlignment.centre: alignmentMultiplier = 0.5f; break;
                case TextAlignment.right: alignmentMultiplier = 1; break;
                default: alignmentMultiplier = 0; break;  // This means the alignment is left if the other programmer somehow 
                                                          // doesn't pass the alignment parameter or passes an invalid enum value... 
                                                          // I don't know how the heck that would ever happen, but, like, gotta stay safe
            }
            // You may be thinking, why the heck am I storing these numbers? All will be revealed in the soon...

            // Another thing we'll do here for the sake of not having to do it every frame: set the position of each individual item!
            // This is where alignment_multiplier comes into play.
            // First, make the array containing the options' positions the same length as the one containing the options themselves...
            option_positions = new Vector2[options.Length];
            // Now for the fun part! Loop through the two arrays, filling up option_positions
            for (count = 0; count < options.Length; count++)
            {
                // Initialize this Vector2 object
                option_positions[count] = new Vector2();

                // For all these formulas, order of operations applies. Remember BEDMAS? UNDERSTAND BEDMAS? BE BEDMAS!?

                // X position: The X position of the menu itself, minus... a cool formula. I'll explain soon enough.
                option_positions[count].X = Position.X - font.MeasureString(options[count]).X * alignmentMultiplier;
                // The last term is the crazy part - basically, you move the text to the left (by subtracting the displacement from the position)
                // by a certain magnitude to align it a certain way - 
                // nothing to align it to the left (since the X position is the top-left of the text by default),
                // half the length of the text (in pixels) to centre it, or by the full length to align it to the right!

                // Y position: The Y position of the menu itself, 
                // plus the height of the text times the number of options that have already been displayed
                option_positions[count].Y = Position.Y + font.MeasureString(options[count]).Y * count;
            }

            // And another! The "snimplifications" just keep on coming!
            // Set the size of the cursor based on the text size
            // You know the height of the letter I? The cursor should be that tall, and that wide!
            side = (int)font.MeasureString("I").Y;
            cursorSize = new Point(side);

            // Call the method that sorts all callable methods into an array of D E L E G A T E S
            option_methods = GetDelegateArray();
        }

        /// <summary>
        /// Update: This method updates the menu - handling input and making sure the cursor is in the right place, basically
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            // A local variable! Wowza!
            Point cursor_pos = new Point();   // The position of the cursor

            // Handle input
            // If the down arrow key is pressed (and isn't already held down), increment the index, moving the cursor one option down
            if (InputManager.Pressed(Keys.Down))
            {
                index++;
                // If we've gone past the end of the list, go back to the beginning
                if (index > options.Length - 1)
                    index = 0;
            }
            // Up arrow key: decrememnt index, moving the cursor up
            else if (InputManager.Pressed(Keys.Up))
            {
                index--;
                // Same thing as last time; loop around to the end if we've gone past the beginning
                if (index < 0)
                    index = options.Length - 1;
            }
            // Enter key: Make selection. I'm not sure how this will work quite yet
            else if (InputManager.Pressed(Keys.Enter))
                // Run a method based on which option the user just chose
                option_methods[index]();

            // Set position of cursor - it's always slightly to the left of the text
            // Offset the cursor to the left by the width of the cursor, so nothing overlaps
            cursor_pos.X = (int)option_positions[index].X - cursorSize.X;
            // The Y position of the text and cursor are THE SAME
            cursor_pos.Y = (int)option_positions[index].Y;

            // Make a rectangle from the cursor's position (defined in the constructor) and the size (defined right above you!)
            cursorRect = new Rectangle(cursor_pos, cursorSize);
        }

        /// <summary>
        /// Draw: This method displays the menu on the screen. I still find it interesting how drawing and updating have to be separate in Monogame.
        /// </summary>
        /// <param name="spriteBatch"> MonoGame object used to display graphics </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw each menu option
            for (count = 0; count < options.Length; count++)
                spriteBatch.DrawString(font, options[count], option_positions[count], textColour);

            // Draw the cursor
            spriteBatch.Draw(cursorImage, cursorRect, Color.White);
        }

        /// <summary>
        /// GetDelegateArray: This method sorts its methods into an array of delegates 
        /// so they can be easily called by the Update method of the base class.
        /// </summary>
        /// <returns> Array of delegates, each corresponding to the effects of selecting a certain menu option </returns>
        protected abstract Action[] GetDelegateArray();
    }
}