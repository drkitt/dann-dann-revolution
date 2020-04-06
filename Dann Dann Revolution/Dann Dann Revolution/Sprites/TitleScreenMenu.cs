//Dann Dann Revolution Team
//File Created: 1/5/2018
//The First Menu Where The User Can Either:
//  Play The Game
//  Quit The Game
//  Or Watch The Credits


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
    // Up next: The creation of the first-ever derived class!
    internal class TitleScreenMenu : Menu
    {
        Game game;//Instance Of The Game Class That The Program is Running Off of

        /// <summary>
        /// TitleScreenMenu: This method creates a menu hand-crafted for the title screen!
        /// </summary>
        /// <param name="position"> The position of the top-middle of the menu... yeah </param>
        public TitleScreenMenu(Vector2 position, Game _game) :
            // Genius Idea: Since there's only ever gonna be one title screen menu, why not hard-code rest of the parameters? Yeah?
            base("Graphics/Dan Arrow Right", "Fonts/Arial Black 64pt", new string[] { "Play", "Quit", "Credits" }, 

                position, Menu.TextAlignment.centre, Color.White)
        {
            /* Check this out:
             * the first set of brackets (before the colon) are for this class' constructor declaration, 
             * while the second set is a call to the base class' contructor.
             */
            game = _game;
        }

        /// <summary>
        /// GetDelegateArray: This method sorts its methods into an array of delegates
        /// so they can be easily called by the Update method of the base class
        /// </summary>
        /// <returns> Array of delegates representing the methods that are called when certain options are selected </returns>
        protected override Action[] GetDelegateArray()
        {
            // Yo yo, it's time to hardcode! It's okay because this is part of a specialized class that's derived from a reusable base class
            return new Action[] { Play, Quit, Credits};
            // That Was Easy!
        }

        /// <summary> Play: This method begins the game! </summary>
        protected void Play()
        {
         //   int y = 10; //Y postion for arrow placement
            SceneManager.AddNewScene(
                   // The only actual parameter - the Scene objects, which holds a list of IEntity objects (which is where it gets messy!)
                   new Scene
                   (
                       // The ~Enitity List~
                       new List<IEntity>
                       {
                        // The contents of The ~Entity List~ - everything on this level of indenting is an object
                        new GenericSprite("Graphics/Dance club placeholder", new Vector2(0)),
                        new GenericSprite("Graphics/Logo ver. 4", new Vector2(900, 20)),
                        new SongSelection(new List<string> {
                            /*Easy*/        "Older", "All Summer Long",
                            /*Medium*/      "I Lost On Jeopardy", "Rockstar", "Are You Gonna Be My Girl", "Ballroom Blitz", "Sugar",
                            /*Hard*/        "Do I Wanna Know", "We Built This City", "Last Saskatchewan Pirate", "Tear In My Heart",
                            /*Sweetnam*/   "Shooting Stars", "Handclap", "Bap-U"},
                            "Fonts/Arial Black 36pt", "Fonts/Arial Black 64pt", new Vector2(0, 300))
                       }
                   )
               );
        }
        
        /// <summary>
        /// Help: This method does nothing. We were going to include a help menu, but ended up scrapping it in favour of the overlay. 
        /// However, we couldn't just give up The Dann Bear, so this method is here to stay!
        ///         ^^^ I Couldnt Agree More -Mike (Father Of Said Bear)
        /// </summary>
        protected void Help() {

            /*
             * Now There Is Helpful Stuff Here..
             * But I Am Still Keeping The Dann Bear
             * 
          _.--.._ ..----.. _..--.
        ,'      `'        `'  _  `.
       :   ,';               `.`.  :
       |  : /                  \ ) |
       :  `:    __        __    :  ;
        `-.|   (o_)  __  (o_)   |-'
           :        ___         ;
      __    \      (:::)       /    __
    ,'  `.   `.     `-'      ,'   ,'  `.
   :      `-._.`.. `---' _..'._.-'      :
   :      ) /     \`---''/     \'    ,  ;
    `._ .  /       `.   /       \  -'_.'
       :-,'          `.'         `.-:
       `'-._;     Dann Bear    :_.-`'
           /                    \
         _:__                  __:_
       ,' _  `.              ,' _  `.
      / ,' `.  \            / ,' `.  \
     : :     :  :          : :     :  :
     | |     |  |::..____.:| |     |  |
     : :.    ;  ;          : :.    ;  ;
      \ `::.' ,'            \ `::.'  /
       `-...-'               `-....-'
        */
        }

        /// <summary> Quit: This method ends the current Scene. 
        /// When there are no Scenes to fall back on (such as when it's called from the title screen),
        /// it closes the game thanks to the SceneManager.OhSnapNoScenes event. </summary>
        protected void Quit()
        {
            game.Exit();//Exit Game By Calling The Base Game Class
        }

        protected void Credits() {
            SceneManager.AddNewScene(
                   // The only actual parameter - the Scene objects, which holds a list of IEntity objects (which is where it gets messy!)
                   new Scene
                   (
                       // The ~Enitity List~
                       new List<IEntity>
                       {
                           new Credits()
                       }
                   )
               );


        }
    }
}

