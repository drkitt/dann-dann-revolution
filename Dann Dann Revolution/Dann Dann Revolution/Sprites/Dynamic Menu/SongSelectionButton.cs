/*
 * Dann Dann Revolution Team
 * File created January 11, 2018
 * SongSelectionButton: This class represents a singluar button in the menu in the song selector. What a nest!
 * It has a song assigned to it, and will start the game using said song when pressed.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Dann_Dann_Revolution
{
    class SongSelectionButton : MenuOption
    {
        /// <summary>
        /// A Button For The Song Selection Menu
        /// </summary>
        /// <param name="songName"> The song's name </param>
        /// <param name="difficulty"> The song's difficulty, yo - this is a really hard method to explain </param>
        public SongSelectionButton(string songName, string difficulty)
        {
            // The most amazing parameter-to-instance-variable conversion of all time
            text = songName;
            TextColour = Color.Black;
            switch (difficulty)
            {
                case "Easy": BGColour = DannDannRevolution.Green; break;
                case "Medium": BGColour = DannDannRevolution.Yellow; break;
                case "Hard": BGColour = DannDannRevolution.Red; break;
                case "Sweetnam": BGColour = DannDannRevolution.Blue; break;
                default:
                    throw new Exception("Difficulty Not Valid");
                    break;
            }
        }

        // Option: This method creates a new gameplay Scene featuring this object's song
        public override void Option()
        {
            int y = 10; //Y Cordinate Of The Notes
            SceneManager.AddNewScene(
                new Scene(new List<IEntity>
                {
                    new VideoSprite("Video/Danncing - Room 217", Vector2.Zero),
                    // Uncomment to have the background picture instead of the video
                    //new GenericSprite("Graphics/Dance club placeholder", new Vector2(0)),
                        new GenericSprite("Graphics/Logo ver. 4", new Vector2(10, 20)),
                        //Add The Blank Arrows At The Top
                        new GenericSprite("Graphics/blank_arrow_left", new Vector2(1250 ,y)),
                        new GenericSprite("Graphics/blank_arrow_up", new Vector2(1430,y)),
                        new GenericSprite("Graphics/blank_arrow_right", new Vector2(1590,y)),
                        new GenericSprite("Graphics/blank_arrow_down", new Vector2(1760,y)),
                        new InputEffects(),
                        new GameScreen(text)
                })
                );
        }
    }
}
