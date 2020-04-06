/*
 * Alex Kitt
 * December 19, 2017 (original time of writing: July 28, 2017)
 * Project Tribute
 * Scene: This class manages a list of entities, updating them all every frame
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
    internal class Scene
    {
        // Instance variable! Wow!
        /// <summary> List of things in the Scene that have to be updated. This includes sprites, timers, bees, you name it!
        /// If it needs to be updated and is at least reasonably independent, you'll find it here. </summary>
        private List<IEntity> entityList;
        /// <summary> List of things in the Scene that have to be updated AND drawn... WOAH!
        /// Obtained by hand-picking all the ISprite objects from entityList </summary>
        private List<ISprite> spriteList;

        /// <summary>
        /// The boring ol' constructor - takes a list of entities (things that are in the Scene) and, uh, adds them to its own list.
        /// </summary>
        /// <param name="entityList"> List of things in the Scene that have to be updated while this Scene is active.
        /// Make sure to pass them in the order that they are updated (and thus drawn - 
        /// if you think about sprites like photoshop layers, the sprites that make up the simile's bottom layers should be passed here first) </param>
        public Scene(List<IEntity> entityList)
        {
            this.entityList = entityList;
            // That's it

            // As per usual with constructors, it turns out that merely assigning parameters to instance variables is not enough.
            // What a tragedy.
            // Time to make another list!


			// Give a reference to the scene's entityList to all the entities that need it
			List<IDirector> directorList;
            directorList = entityList.OfType<IDirector>().ToList();
			foreach (IDirector director in directorList)
			{
				director.EntityList = entityList;   // The entity you seek is death. 
				director.FilterList();
			}

            // Create the sprite list
            /* This line used to be above the director thing, but watch out! 
             * Directors have the power to add entities (SUCH AS sprites!) to the scene's entityList, 
             * so it's a good idea not to do any sprite-filtering until later after the directors have had their fun.
             */
            spriteList = entityList.OfType<ISprite>().ToList();
		}

        /// <summary>
        /// Update: This method calls the Update methods of all the entities it contains, yes... but it does so much more! 
        /// It update the input manager too!
        /// </summary>
        /// <param name="gameTime"> Object that keeps track of the time elapsed in real life </param>
        public void Update(GameTime gameTime)
        {
            /* Update the input manager - it's in here, rather than in a Scene as one of the entities, 
             * because pretty much every Scene will need input. Better to put the most basic of entities here than to pass it to 
             * every single Scene's constructor!
             * And for the rare Scene that doesn't take input, whoopee! A couple nanoseconds wasted.
            */
            InputManager.Update();

            // Update ALL the entities!
            foreach (IEntity entity in entityList)
                entity.Update(gameTime);
        }

        /// <summary>
        /// Draw: Fun fact - this method does nothing but run the methods of the same name of all the sprites it contains! 
        /// </summary>
        /// <param name="spriteBatch"> MonoGame object used to display graphics </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw ALL the sprites!
            foreach (ISprite sprite in spriteList)
                sprite.Draw(spriteBatch);
        }
    }
}
