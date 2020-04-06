/*
 * Dann Dann Revolution Team
 * File Created: December 18, 2017
 * DannDannRevolution: This class runs the game, really. 
 * This and many of the game's engine files are lifted from the game I started at Propel, Project Tribue (patent pending!).
 * Very quick summary of the engine's design: The game is split into Scenes, which work like functions in a procedural program. 
 * Each Scene contains entities (objects implementing specific interfaces), which are updated every frame of gameplay. 
 * There are two main types of entities: Sprites (stuff the player can see), and directors (which govern interactions between sprites). 
 */

// Pro-tip: If something new isn't working, first check to make sure you have all the required using statements!
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Dann_Dann_Revolution
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DannDannRevolution : Game
    {
        // Instance variables
        /// <summary> The width of the resolution the game is rendered at </summary>
        public static readonly int VIRTUAL_SCREEN_WIDTH = 1920;     // Ever seen that many keywords strung together before?
        /// <summary> The height of the resolution the game is rendered at </summary>
        public static readonly int VIRTUAL_SCREEN_HEIGHT = 1080;    // 'Tis the beauty of C#
        // Stuff that came with the Monogame framework (do not touch!)
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //Probs Not Good But YOLO ALEX WILL HELP LATER
        public static SpriteFont GameFont { get; private set; } // The game's main font
        public static string GameFontPath { get; private set; } // (in loaded AND unloaded form!)
        // Alex here - it's good!
        // I'm even gonna put more public static stuff here! Colours!
        public static readonly Color Green = new Color(76, 175, 80);
        public static readonly Color Yellow = new Color(255, 235, 59);
        public static readonly Color Red = new Color(244, 67, 54);
        public static readonly Color Blue = new Color(33, 150, 243);


        // This method doesn't actually do much constructing - that's Initialize()'s job.
        public DannDannRevolution()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";

            // Initialize a class that controls the game's resolution - thank David Amador
            Resolution.Init(ref graphics);
            // Change Virtual Resolution 
            Resolution.SetVirtualResolution(VIRTUAL_SCREEN_WIDTH, VIRTUAL_SCREEN_HEIGHT);
            // And Actual Resolution
            Resolution.SetResolution(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, true);
            /* The first two paraemeters are obviously the true screen resolution (OBVIOUSLY), 
             * but what you might not know is that the third parameter controls whether the game is in fullscreen! Wow!
             * You're watching The History Channel.
             */
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // Don't mind if I do!

            base.Initialize();

            // Add the game's Exit method to sceneManager's repitoire of stuff to do when the game ends
            SceneManager.OhSnapNoScenes += Exit;

            // Add the game's services to the static (NOT GLOBAL, STATIC) GameServices object
            GameServices.Add<GraphicsDevice>(GraphicsDevice);
            GameServices.Add<ContentManager>(Content);
            /* Content is the one we're looking for (though GraphicsDevice does have advanced potential use), 
             * as most of our content-loading will be done outside this class. 
             * Can I use this class to make any object global? I will never know, because I pledge to never abuse this power.
             */

            // Add a scene to the scene manager - the title screen scene!
            // It's time to get NESTED, yo
            SceneManager.AddNewScene(
                // The only actual parameter - the Scene objects, which holds a list of IEntity objects (which is where it gets messy!)
                new Scene
                (
                    // The ~Enitity List~
                    new List<IEntity>
                    {
                        // The contents of The ~Entity List~ - everything on this level of indenting is an object
                        new GenericSprite("Graphics/Dance club placeholder", new Vector2(0)),
                        new TitleScreenMenu(new Vector2(VIRTUAL_SCREEN_WIDTH / 2, VIRTUAL_SCREEN_HEIGHT / 2), this),
                        new GenericSprite("Graphics/Logo ver. 4", new Vector2(VIRTUAL_SCREEN_WIDTH / 2 - 500, 20)),
                        new GenericSprite("Graphics/menu_help", new Vector2(0))
                    }
                )
            );
            /* Note: Moving the cursor to a bracket will highlight it and the other bracket in the pair.
             * Mastering this ancient technique is crucial to understanding any AddNewScene call, 'cause believe me, 
             * there's gonna be far more than two sprites per scene later, and I won't be as nice with the indenting.
             */
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Joke's on the Monogame framework - content is loaded on a per-object basis, so this method is useless!
            // ...Except for in the case of the main font. That's loaded here.
            GameFontPath = "Fonts/Arial Black 64pt";
            GameFont = Content.Load<SpriteFont>(GameFontPath);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            // Nothing to see here!
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit on Esc key press
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Initialize the spritey thing and the resolutiony thing
            Resolution.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Resolution.getTransformationMatrix());
            // That last parameter is responsible for scaling the game up, yo!!!
            /* To disable anti-aliasing (useful for small sprites),
             * replace the second and third parameters with BlendState.AlphaBlend and SamplerState.PointClamp, respectively
             */

            // Call the one true Draw method
            SceneManager.Draw(spriteBatch);
            // De-initialize spritey thing
            spriteBatch.End();

            // Display the frame rate in the top-secret Output window
            Console.WriteLine(1 / (float)gameTime.ElapsedGameTime.TotalSeconds); // That's called a reciprocal, right?
            /* Fun fact (but not really): gameTime.ElapsedGameTime is the time since the last call to the Draw method, 
             * and TotalSecods is that value expressed in seconds.
             * In short, (float)gameTime.ElapsedGameTime.TotalSeconds is the quantity of seconds per frame.
             * All the great physicists know that you can invert a measurement by dividing 1 by it, so evaluating 1 / seconds per frame
             * yields the frames per second.
             */

            base.Draw(gameTime);
        }
    }
}
