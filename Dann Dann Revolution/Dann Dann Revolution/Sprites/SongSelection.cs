//Dann Dann Revolution Team
//File Created: 1/5/2018
//Menu To Have The User Select A Song
/*
 * Well... it's basically a menu. More specifically, it's a sprite that contains a menu, but for all intents and purposes it's a fancy menu.
 * Had we more time, I would've made the song info window a spearate object from the menu, and connect the two with a director, but, well, 
 * we have a month. Perfection is a myth anyway!
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Dann_Dann_Revolution
{
    internal class SongSelection : IEntity, ISprite
    {
        // Instance variables and the like
        /// <summary> The sprite's position on screen </summary>
        public Vector2 Position { get; set; }
        /// <summary> The size of the album art </summary>
        private static readonly Vector2 ART_SIZE = new Vector2(600, 600);    // (imagine the following in bold and italics) COMPLETE GUESSWORK
        /// <summary> How far away the album art is from the sprite's position </summary>
        private static readonly int ART_DISPLACEMENT = Convert.ToInt32(DannDannRevolution.VIRTUAL_SCREEN_WIDTH - ART_SIZE.X - 50);
        /// <summary> The font used for the menu and info window </summary>
        private SpriteFont font;
        /// <summary> The font used for the title of the song </summary>
        private SpriteFont bigFont;
        /// <summary> Stores info for all the songs. All of them! </summary>
        private List<SongLoader> songs;
        /// <summary> Dynamic menu that displays all possible songs and allows user to pick one </summary>
        private DynamicMenu songPicker;
        /// <summary> If The Song Has Changed Or Not In The Update Method</summary>
        private bool songChange;

        // Fields and fields of song info (of the currently selected song)
        /// <summary> Name of the currently selected song  </summary>
        private string songName;
        /// <summary> Artist of the currently selected song  </summary>
        private string artist;
        /// <summary> Duration of the currently selected song </summary>
        private string duration;
        /// <summary> Difficulty of the currently selected song </summary>
        private string difficulty;
        /// <summary> Album art of the currently selected song </summary>
        private Texture2D albumArt;
        /// <summary>Pictoral Representation Of The Difficulty Of The Selected Song</summary>
        private Texture2D difficultyArt;


        /// <summary>
        /// Creates the menu and other cool stuff
        /// </summary>
        /// <param name="trackList"> List of songs this object canselect </param>
        /// <param name="fontPath"> The path to the font the menu is using </param>
        /// <param name="bigFontPath"> The path to the font used to display the title of the song </param>
        /// <param name="position"> The coordinates of the top-left of the sprite </param>
        public SongSelection(List<string> trackList, string fontPath, string bigFontPath, Vector2 position)
        {
            // Make an list (but don't check it twice)
            List<MenuOption> songPickerOptions = new List<MenuOption>();

            // Load the songs, Kronk!
            songs = new List<SongLoader>();
            for (int i = 0; i < trackList.Count; i++)
            {
                songs.Add(new SongLoader(trackList[i]));
                songPickerOptions.Add(new SongSelectionButton(songs[i].getSongName(), songs[i].getDifficulty()));
            }

            //Add a exit button
            songPickerOptions.Add(new ExitButton());
            // Attempt to create the menu
            songPicker = new DynamicMenu(fontPath, new Vector2(0, 0), new Vector2(825, 800), songPickerOptions);
            //Make A Smaller "Woosh"
            songPicker.wooshDistance = 24;
            // Also, keep the font around
            ContentManager contentManager = GameServices.Get<ContentManager>();
            font = contentManager.Load<SpriteFont>(fontPath);  // Thank you content manager
            bigFont = contentManager.Load<SpriteFont>(bigFontPath);

            // Initialize the song-data instance variables (this needs to be here because the first Draw call comes before the first Update call)
            UpdateSongData();

            // Set position (almost forgot!)
            Position = position;
        }

        /// <summary>
        /// Update: This method updates the info shown based on the index of the menu
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of timing values </param>
        public void Update(GameTime gameTime)
        {
            // I almost forgot! Update the menu.
            songPicker.Update(gameTime);

            // Update song data
            UpdateSongData();
        }

        /// <summary>
        /// Draw: This method displays the menu and relevant info
        /// </summary>
        /// <param name="spriteBatch"> MonoGame object used for GRAPHICS and things </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // A local variable! Untold of!
            const int TEXT_SPACING = -66;   // How many pixels are between each line of text
            // And another one!
            Vector2 textPos = new Vector2(Position.X + ART_DISPLACEMENT, Position.Y + ART_SIZE.Y + TEXT_SPACING); // Initial position of song info

            // Draw the menu
            songPicker.Draw(spriteBatch);   // Around this point, the call stack will be as follows: Program() -> game.Run() -> game.Draw() -> SceneManager.Draw() -> foreach (ISprite sprite in spriteList) { sprite.Draw(); } -> songPicker.Draw() -> foreach (MenuOption option in options) { option.Draw() }

            // Draw the album art
            spriteBatch.Draw(albumArt, new Rectangle(Convert.ToInt32(Position.X + ART_DISPLACEMENT), Convert.ToInt32(Position.Y), Convert.ToInt32(ART_SIZE.X), Convert.ToInt32(ART_SIZE.Y)), Color.White);

            //Draw Difficulty
            spriteBatch.Draw(difficultyArt, new Rectangle(Convert.ToInt32(ART_DISPLACEMENT + ART_SIZE.X * 5 / 6 - 30), Convert.ToInt32(Position.Y - 20), Convert.ToInt32(150), Convert.ToInt32(150)), Color.White);

            // Display info
            spriteBatch.DrawString(bigFont, songName, Position + new Vector2(ART_DISPLACEMENT + ART_SIZE.X - bigFont.MeasureString(songName).X, ART_SIZE.Y - 30), Color.White);
            // It's repetitive enough to be, well, repetitive, but each method call is too distinct from its cohorts to make it a loop :(
            // (though I suppose I COULD make a List<string> of all the labels and data, but... I'd rather get all my assignments finished)
            spriteBatch.DrawString(font, artist, textPos + new Vector2(-font.MeasureString(artist).X, 0), Color.White);

            // Uncomment the following to display duration and difficulty
            //textPos += new Vector2(0, TEXT_SPACING * 1.25f);
            //spriteBatch.DrawString(font, duration, textPos + new Vector2(-font.MeasureString(duration).X, 0), Color.White);
            //textPos += new Vector2(0, TEXT_SPACING);
            //spriteBatch.DrawString(font, difficulty, textPos + new Vector2(-font.MeasureString(difficulty).X, 0), Color.White);
        }

        /// <summary>
        /// UpdateSongData: This method assigns values to instance variables based on the currently selected song
        /// </summary>
        private void UpdateSongData()
        {
            try        //The Try/Catch Is For The Back Button
            {
                // Get ready to show appropriate/tasteful info on screen
                if (songName == songs[songPicker.Index].getSongName()) {
                    //Its Same Song So No Need To Re Update
                    return;
                }
                else
                {
                    songName = songs[songPicker.Index].getSongName();
                    songChange = true;
                }

                artist = songs[songPicker.Index].getArtistName();
                int duration_seconds = Convert.ToInt32(songs[songPicker.Index].getDuration());  // The duration of the song, in seconds

                // But wait! There's more!
                // We also gotta convert the duration in seconds to minutes
                int duration_minutes = duration_seconds / 60;   // The duration of the song, in minutes
                int duration_minutes_remainder = duration_seconds % 60; // The remaining seconds after the truncation-tastic conversion to minutes
                string leadingZero = "";    // A delightful little string that makes the formatting look proper
                // Display leading zero if there's a single-digit number of seconds
                if (duration_minutes_remainder < 10)
                    leadingZero = "0";
                duration = "Duration: " + Convert.ToString(duration_minutes) + ":" + leadingZero + Convert.ToString(duration_seconds % 60);
            
                // We now return you to your regularly scheduled programming
                difficulty = songs[songPicker.Index].getDifficulty();
                albumArt = songs[songPicker.Index].getArt();
                ContentManager contentManager = GameServices.Get<ContentManager>();
                difficultyArt = contentManager.Load<Texture2D>("Graphics/Difficulties/" + difficulty);
                Song song = contentManager.Load<Song>("Music/" + songName);
                //Preview Song
                MediaPlayer.Volume = 0.15f;
                MediaPlayer.Play(song);
                songChange = false;
            }
            catch (Exception)
            {
                MediaPlayer.Stop();
                //If Alex Wants To Change This He Can But I Am Content With How It Looks / Works
            }
        }
    }
}
