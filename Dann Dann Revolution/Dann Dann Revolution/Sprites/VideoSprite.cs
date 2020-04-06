/*
 * Dann Dann Revolution Team
 * File Created: January 22, 2017 - Last TizQuest Day!
 * 
 * VideoSprite: This may be the last class I ever write in this course! 
 * This one plays a video, which can easily done using Monogame's built-in classes, 
 * but it does so in a way compatible with Tribute Engine (patent pending)'s system of Scenes and entities.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Dann_Dann_Revolution
{
    internal class VideoSprite : IEntity, ISprite
    {
        /// <summary> Texture representing the current frame of the video </summary>
        private Texture2D videoTexture;
        /// <summary> The video the sprite is playing </summary>
        private Video video;
        /// <summary> Object that plays video - this entire class is basically a wrapper for this object! </summary>
        /// It's static because, uh, it doesn't work otherwise. 
        /// There's something to this class I don't quite understand, that breaks the game when you've made more than one instance of it. 
        private static VideoPlayer player = new VideoPlayer();
        /// <summary> Where the heck is this video? </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// VideoSprite: This constructor loads up a video
        /// </summary>
        /// <param name="videoPath"> Path to the video file </param>
        /// <param name="position"> The sprite's position-to-be </param>
        public VideoSprite(string videoPath, Vector2 position)
        {
            // LOAD THE VIDEO, KRONK (but first manage some content)
            ContentManager contentManager = GameServices.Get<ContentManager>();
            video = contentManager.Load<Video>(videoPath);
            // It's TIME to STOP
            player.Stop();

            // Initialize texture variable
            videoTexture = new Texture2D(GameServices.Get<GraphicsDevice>(), 1, 1);

            // Play the video
            // Just kidding! The video gets played in the Update method!
        }

        /// <summary>
        /// Update: This method starts the video, or maybe it updates what frame of the video is being displayed
        /// </summary>
        /// <param name="GameTime"> MonoGame object used to- wait, wrong description! Displays a snapshot of timing values. </param>
        public void Update(GameTime GameTime)
        {
            /* Start playing the video - this is done here just in case it takes a really long time for the game to run all 
             * the other entities's constructors, which would cause the player to miss some of the video if this part were in this class' constructor.
             * If the video is already playing, it updates the current frame!
             */
            if (player.State == MediaState.Stopped)
                player.Play(video);
            else
            {
                // AHA! JUST AS I WAS ABOUT TO GIVE UP ON INCLUDING THE VIDEO, A STRATEGICALLY-PLACED CALL TO TEXTURE2D.DISPOSE SAVES THE DAY!!!!
                videoTexture.Dispose(); // Seriously, try commenting this out. 
                /* 
                 * The game WILL crash, and it's not just any old crash - the memory leak will force you to restart your computer!
                 * I think that the game was having trouble overwriting the videoTexture variable again and again, and forcing it to 
                 * delete the texture first gave it a helping hand. 
                 */
                videoTexture = player.GetTexture();
                // Is this the last comment I'll ever write for this project?

                // It's not! Well, I'm gonna test this again with a longer song - fingers crossed...
            }
        }

        /// <summary>
        /// Draw: This method displays the current frame of the video on-screen
        /// </summary>
        /// <param name="spriteBatch"> MonoGame object used to display graphics </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            /* Create a rectangle - this makes the video fill the screen, but this part is written in such a way that you can (probably) 
             * un-hardcode it without much trouble later
             */
            Rectangle destinationRectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y),
                // (these are the parts you'll want to modify later)
                DannDannRevolution.VIRTUAL_SCREEN_WIDTH, DannDannRevolution.VIRTUAL_SCREEN_HEIGHT);

            // Draw the current texture - the quite neat way the VideoPlayer class works, we can just treat the current frame like any old image!
            spriteBatch.Draw(videoTexture, destinationRectangle, Color.White);
        }
    }
}


// Code that tried and failed to have content get loaded in this class, and thus have the background change based on difficulty
//public enum Backgrounds { Dann, Scott };

//private static Video dannVid, scottVid;

//static VideoSprite()
//{
//    ContentManager contentManager = GameServices.Get<ContentManager>();
//    dannVid = contentManager.Load<Video>("Video/Danncing - Room 217");
//    scottVid = contentManager.Load<Video>("Video/Danncing - The NerdDome");
//}
//public VideoSprite(Backgrounds bg)
//{
//    Position = Vector2.Zero;
//    if (bg == Backgrounds.Dann)
//        video = dannvid;
//    else
//        video = scottvid;
//    videotexture = new texture2d(gameservices.get<graphicsdevice>(), 1, 1);
//    player.stop();
//}

// Fun fact: You can use Ctrl+K+C to comment out an entire highlighted section, and Ctrl+K+U to uncomment it!