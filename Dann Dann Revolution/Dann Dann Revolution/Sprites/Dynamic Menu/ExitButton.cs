//Dann Dann Revolution Team
//File created: January 16th 2018
//This Button Is Used To Exit The Song Selection Upon Being Called

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Dann_Dann_Revolution
{
    class ExitButton : MenuOption
    {
        // The CONSTRUCTOR! This takes the song and rolls with it, yo
        public ExitButton()
        {
            text = "Back To Menu";
            TextColour = Color.Black;
            BGColour = Color.White;
        }

        // Option: This method creates a new gameplay Scene featuring this object's song
        public override void Option()
        {
            MediaPlayer.Stop();
            SceneManager.EndScene();
        }
    }
}
