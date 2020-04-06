/*
 * Dann Dann Revolution Team
 * File Created: January 8, 2018 (original time of writing: October 6, 2017
 * DynamicMenu: This class is the "super cool work-in-progress dynamic menu" I talked about in the Menu class! 
 * It's not nearly done yet, but in its current state it'll be super useful for the song selection menu.
 * The big innovation here is that it stores each menu option as its own object, making it quite dynamic indeed.
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
    internal class DynamicMenu : IEntity, ISprite
    {
        // Instance variables
        /// <summary> The standard speed at which menu items should whoosh, measured in pixels per second </summary>
        private readonly int OPTION_SPEED = 600;
        /// <summary> The Distance That The Menu Options Will "Woosh" As Alex Puts It</summary>
        public int wooshDistance { get; set; }
        /// <summary> You need an IQ of at least 56348y0y8042646408024 to comprehend this variable (it's the options this menu presents) </summary>
        private List<MenuOption> options;
        /// <summary>
        /// The size of the menu as a whole
        /// </summary>
        private Vector2 size;
        /// <summary>
        /// Size: More of an excuse to play around with custom accessors than a variable, this one lets other objects access the size of the menu
        /// </summary>
        public Vector2 Size {
            get { return size; }
            set
            {
                // Loop through all options, setting their individual sizes so the overall size is as intended
                foreach (MenuOption option in options)
                    option.Size = new Vector2(value.X, value.Y / options.Count);
            }
        }
        /// <summary> The index of the currently selected menu option </summary>
        public int Index { get; private set; }  // This cool getter-setter thing makes it so any class can see the variable, 
                                                // but only this class can modify it
        /// <summary> The sprite's position on screen </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Assigns colours and positions to a whole buncha menu options 
        /// </summary>
        /// <param name="fontPath"> The path to the font each option is using </param>
        /// <param name="position"> The coordinates of the top-left of the menu </param>
        /// <param name="size"> How much space on the screen the menu takes up </param>
        /// <param name="options"> The things the menu can, er, do </param>
        public DynamicMenu(string fontPath, Vector2 position, Vector2 size, List<MenuOption> options, int _wooshDistance = 8)
        {
            // Do the obvious stuff - shove a buncha parameters into instance variables
            Position = position;
            this.size = size;
            this.options = options;

            // Load the font (which entails summoning ContentManager, mind you) and assign it to all the options
            ContentManager contentManager = GameServices.Get<ContentManager>(); // I missed you, ContentManager
            SpriteFont font = contentManager.Load<SpriteFont>(fontPath);

            // Calculate the size and position of each option 
            // (the animation of them coming from nowhere will come later (from nowhere); for now the focus is on making it exist)
            int optionHeight = (int)size.Y / options.Count;
            for (int count = 0; count < options.Count; count++)
            {
                options[count].Size = new Vector2(size.X, optionHeight);
                // This line here is why I used a traditional for loop, if anyone asks
                options[count].Position = new Vector2(Position.X, Position.Y + optionHeight * count);
                // Everything's gonna be white by default
                options[count].BGColour = options[count].BGColour;
                options[count].Font = font;
                options[count].Speed = OPTION_SPEED;
            }
            wooshDistance = _wooshDistance;
        }

        /// <summary>
        /// Update: This method reacts to input and plays a buncha animations. Hopefully.
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of a snapshot of a snapshot of a snapshot of a snapshot of timing values. </param>
        public void Update(GameTime gameTime)
        {
            // Handle input (reused from the old menu, as is tradition)
            // If the down arrow key is pressed (and isn't already held down), increment the index, moving the cursor one option down
            if (InputManager.Pressed(Keys.Down))
            {
                Index++;
                // If we've gone past the end of the list, go back to the beginning
                if (Index > options.Count - 1)
                    Index = 0;
            }
            // Up arrow key: decrememnt index, moving the cursor up
            else if (InputManager.Pressed(Keys.Up))
            {
                Index--;
                // Same thing as last time; loop around to the end if we've gone past the beginning
                if (Index < 0)
                    Index = options.Count - 1;
            }
            // Enter key: Make selection. I'm not sure how this will work quite yet
            else if (InputManager.Pressed(Keys.Enter))
                // Run a method based on which option the user just chose
                options[Index].Option();

            /* Animate the menu! I've waited months for this.
             * Essentially, the currently selected option will whoosh outwards, 
             * and then upon deselection it'll whoosh inwards until it goes back to its normal position
             */
            foreach(MenuOption option in options)
            {
                // "If this option is the currently selected one,"
                if (options.IndexOf(option) == Index)
                {
                    // "and it's not already at its maximum distance away from its origin"
                    if (option.Size.X <= Size.X + option.Size.X / wooshDistance)
                    {
                        option.Size += new Vector2(Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds * option.Speed), 0);
                        option.Position += new Vector2(Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds * (option.Speed / 2)), 0);
                    }
                }
                else
                    if (option.Size.X > Size.X)
                    {
                        // Put it back!
                        option.Size += new Vector2(-Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds * option.Speed), 0);
                        option.Position += new Vector2(-Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds * (option.Speed / 2)), 0);
                    }

            }
        }

        /// <summary>
        /// Draw: This method displays the MENU on screen. Whowouldathunkit????
        /// </summary>
        /// <param name="spriteBatch"> mONOgAME OBJECT USED TO DISPLAY GRAPHICS </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Like a Scene drawing all its entities, draw all the menu options
            foreach (MenuOption option in options)
                option.Draw(spriteBatch);
        }
    }
}