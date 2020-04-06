//Dann Dann Revolution Team
//File Created: 21/12/2017
//This Class Conatains Data And Direction On A Note
//It Is Also Responsable For The Animation Of The Notes

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
    internal class Note : ISprite, IEntity
    {
        private float time;        //Time The Note Must Be Pressed
        private Keys direction;     //Direction The Arrow Will Face
        private Texture2D image;    //Key Image
        private bool scored;        //If The Note Has Been Pressed Yet
        public Vector2 Position { get; set; }//Starting position for the note

        //Animation Stuff
        private const int ROWS = 1;         //Rows in our sprite sheet
        private const int COLUMNS = 6;      //Columns in our sprite sheet
        private const int TOTALFRAMES = 6;  //total frames in our sprite sheet (the same as columns though??)
        private const float TIMER = 0.1f;   //Timer that is used to cycle through the frames of the animation
        private int currentFrame;           //Current frame that the animation is on
        private float elapsed;              //Amount of time the note has been active for
        private float timer;                //Timer that goes off when it reaches the constant "TIMER"

        private const double SPEED = 256.2857;   //(EndposVector - StartposVector) / TravelTime
                                                 //To Make Faster Change Travel Time And Recalculate Speed

        /// <summary>
        /// Constructor For The Note Class                                             
        /// </summary>                                                                 
        /// <param name="_time">time note has</param>                                  
        /// <param name="_direction">direction of note</param>                         
        /// <param name="vec">Starting postion for the notes</param>
        public Note(float _time, string _direction, Vector2 vec) {
            //Initalize Some Stuuuuuffff
            ContentManager contentManager = GameServices.Get<ContentManager>();
            currentFrame = 0;
            elapsed = 0.0f;
            timer = 0f;
            time = _time;
            scored = false;

            switch (_direction)//Lets Load Up Some Notes SHALL WE!?!?!?
            {
                case "LEFT":
                    direction = Keys.Left;                                                  //Set note direction
                    image = contentManager.Load<Texture2D>("Graphics/left_sprite_sheet");   //Set note picture
                    Position = vec += new Vector2(6, 0);                                    //Set starting place for note
                    break;
                case "UP":              
                    direction = Keys.Up;
                    image = contentManager.Load<Texture2D>("Graphics/up_sprite_sheet");
                    Position = vec += new Vector2(186, 0);                                  //Each Note Will Be Displaced From The First Note
                    break;
                case "RIGHT":
                    direction = Keys.Right;
                    image = contentManager.Load<Texture2D>("Graphics/right_sprite_sheet");
                    Position = vec += new Vector2(346, 0);
                    break;
                case "DOWN":
                    direction = Keys.Down;
                    image = contentManager.Load<Texture2D>("Graphics/down_sprite_sheet");
                    Position = vec += new Vector2(516, 0);
                    break;  
                default:
                    /*
                    * If You Get This Exeption.... YOU MESSED UP BAD HAHA. Future Mike Sucks xD.
                    */ 
                    throw new System.ArgumentException("Direction Is Not Valid", "_direction");
                    break;
            }
        }

        //Set Me And Get Me Baby...... Actualy In This Case Only Set Me
        public float getTime() { return time; }
        public Keys getDirection() { return direction; }
        public void Score() { scored = true; }
        //Oh wait.... There is a get me
        public bool getScored() { return scored; }


        /// <summary>
        /// System call that will update the class
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public void Update(GameTime gameTime)
        {
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;     //Add time since last update to the total time note has been active

            Position += new Vector2(0, -1 * Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds * SPEED)); //Make The Arrow Fly Upwards

            //Timer For Animation Change Speed
            timer += elapsed;
            if (timer > TIMER)//If timer is complete
            {
                currentFrame++;//Update current frame
                if (currentFrame == TOTALFRAMES)//reset current frame
                    currentFrame = 0;
                timer = 0;//reset timer
            }
        }

        /// <summary>
        /// System call that will draw things that I want it to becuase I am its creator and I does what I want (Most of the time)
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            int width = image.Width / COLUMNS;                      //Width each sprite in the spritesheet
            int height = image.Height / ROWS;                       //Height each sprite in the spritesheet
            int row = (int)((float)currentFrame / (float)COLUMNS);  //How far along the sprite sheet the animation is
            int column = currentFrame % COLUMNS;                    //Not actualy applicable for this spritesheet

            //Move frame along the spritesheet photo
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            //Draw the new image
            spriteBatch.Draw(image, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
