//Dann Dann Revolution Team
//File Created: 21-12-2017
//This class lets us parse data from a XML file and load it into the game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dann_Dann_Revolution
{
    class SongLoader
    {
        private string songName;         //Name Of Song (Public So It Can Be Indetified)
        private string artistName;      //Name Of Artist
        private string difficulty;      //Difficulty Setting
        private double duration;        //Duration Of Song
        private List<Note> notes;       //Notes Sorted In Order
        private Texture2D art;      // The album art associated with the song

        /// <summary>
        /// Loads Song Information Into This Class From A XML File
        /// </summary>
        /// <param name="xmlName">Name of song file to load</param>
        public SongLoader(string xmlName) {

            notes = new List<Dann_Dann_Revolution.Note>();

            /*
             * So this is working for now I think but I will keep this line around anyways just incase
             * string databasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\" + path);
            */

            string databasePath = Assembly.GetExecutingAssembly().Location;//Path Where File Is Being Executed

            for (int i = 0; i < 5; i++)//BaBaBa BRAP THAT BACK 5 Directories
            {
                databasePath = Path.GetDirectoryName(databasePath);
            }

            databasePath = Path.Combine(databasePath, @"Data\" + xmlName + ".xml");//Append XML File Path To Project Directory

            //Load File
            XElement database = XElement.Load(databasePath);

            //Parse The Info From The XML File Passed Into Class
            XElement titleElement = database.Element("Title");
            songName = titleElement.Value;
            XElement artistElement = database.Element("Artist");
            artistName = artistElement.Value;
            XElement difficultyElement = database.Element("Difficulty");
            difficulty = difficultyElement.Value;
            XElement durationElement = database.Element("Duration");
            duration = Double.Parse(durationElement.Value);

            // Get album art
            XElement artElement = database.Element("Art");
            ContentManager contentManager = GameServices.Get<ContentManager>();
            art = contentManager.Load<Texture2D>(artElement.Value);

            //Narrow Down Into Subsections Of XML File
            XElement notesElement = database.Element("Notes");

            //Loop For All Directions
            for (int y = 0; y < 4; y++)
            {
                //Its Long..... But It Works Better Then Anything Else I Could Think Of
                string direction = (y == 0) ? "UP" : (y == 1) ? "DOWN" : (y == 2) ? "LEFT" : "RIGHT";
                XElement keyUpElement = notesElement.Element("Key" + direction);

                /*
                * Time To Do The Really Odd Process Of Making A List Of Timestamps
                */

                string input = keyUpElement.Value.ToString();//Get Raw Data
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < input.Length; i++)//Insert '-' Every Time New Timestamp Starts (2 Decimal Places ATM -- 1.3(2) --) May Change As We Get More Exact
                {
                    try
                    {
                        if (input[i - 3] == '.')
                            sb.Append('-');
                    }
                    catch (Exception)
                    {
                        //Catch For The First Few Itirations
                        //This has to be a catch, you cant just start the loop at 3
                        //DO NOT REMOVE OR ANARCY WITH DECEND
                    }
                    sb.Append(input[i]);
                }
                string dashedString = sb.ToString();//Build StringBuilder Into String
                List<string> Times = dashedString.Split('-').ToList<string>();//Split By '-' And Store In Array

                //Check To See If Times Were Loaded Correctly
                if (!String.IsNullOrWhiteSpace(Times[0]))
                {
                    foreach (string i in Times)
                    {
                        notes.Add(new Note(float.Parse(i), direction, new Vector2(1250,907)));//Make Notes With The Parsed Times
                    }
                }
            }

            //Grade 11 Prepared Me To Do This......
            //LIST, I WILL SORT YOU!
            Sort(notes);
        }

        /*
         *                  OH MY WHAT'S THIS????
         *        IS THIS A GRADE 11 INSERT SORT?!?!?!?!?!?!?!
         * 
         *                       IT IS!!!
        */


        /// <summary>
        /// Sorts The List Of Notes Into Ascending Order
        /// </summary>
        /// <param name="list">List of notes</param>
        private void Sort(List<Note> list) {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    for (int j = i + 1; j > 0; j--)
                    {
                        if (list[j - 1].getTime() > list[j].getTime())
                        {
                            Note temp = list[j - 1];
                            list [j - 1] = list[j];
                            list [j] = temp;
                        }
                    }
                }
        }

        //A BUNCH OF GETTERS BABY
        public string getSongName() { return songName; }
        public string getArtistName() { return artistName; }
        public string getDifficulty() { return difficulty; }
        public double getDuration() { return duration; }
        public List<Note> getNotes() { return notes; }
        public Texture2D getArt() { return art; }
    }
}