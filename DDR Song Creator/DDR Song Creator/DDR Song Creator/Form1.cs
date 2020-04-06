//Mike Hodges
//Project Dann Dann Revolution - 12/19/2017 
//This Program Will Allow Us To Live Read In Notes For Our Songs

//Atm the timer is accurate to a hunderth of a second, can be changed easily if nessassary
//All Files Must Be In WAV Format

//
//                                     !!!This was just to make our lives easier and was only to be used by me so please do not mark it!!!!
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Diagnostics;

namespace DDR_Song_Creator
{
    public partial class Form1 : Form
    {
        private SoundPlayer player;     //Media Player
        private StreamWriter upStream;   //File Stream
        private StreamWriter downStream;   //File Stream
        private StreamWriter leftStream;   //File Stream
        private StreamWriter rightStream;   //File Stream
        private Stopwatch stopWatch;
        private string fileName;
        private string songName;

        public Form1()//Construtor
        {
            InitializeComponent();
            //Initialize Form Components
            label1.Text = "File Loaded:";
            label2.Text = "None";
            stopWatch = new Stopwatch();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";       //Path To The File
            string file = "";       //File Name
            openFileDialog1.Filter = "Wave Files (*.wav)|*.wav";    //Set File Dialog Filter
            DialogResult result = openFileDialog1.ShowDialog();     //Show File Dialog
            if (result == DialogResult.OK)                          //If File Inputed
            {
                path = openFileDialog1.FileName;//Get Path
                file = openFileDialog1.SafeFileName;
                file = file.Replace(".wav", "");//Get Filename
            }

            songName = file;
            label2.Text = path;//Update Lable
            player = new SoundPlayer(path);//Load Sound Player
            fileName = file + " " + DateTime.Now.ToString("h.mm.ss");//Make File Name
            upStream = new StreamWriter(fileName + "UP.txt");//Load File Stream
            downStream = new StreamWriter(fileName + "DOWN.txt");//Load File Stream
            leftStream = new StreamWriter(fileName + "LEFT.txt");//Load File Stream
            rightStream = new StreamWriter(fileName + "RIGHT.txt");//Load File Stream
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopWatch.Start();
            player.Play();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //Record Corisponding KeyStroke
            if (e.KeyCode == Keys.Left)
            {
                leftStream.WriteLine(stopWatch.ElapsedMilliseconds);
            }
            if (e.KeyCode == Keys.Right)
            {
                rightStream.WriteLine(stopWatch.ElapsedMilliseconds);
            }
            if (e.KeyCode == Keys.Up)
            {
                upStream.WriteLine(stopWatch.ElapsedMilliseconds);
            }
            if (e.KeyCode == Keys.Down)
            {
                downStream.WriteLine(stopWatch.ElapsedMilliseconds);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Stop Processe
            upStream.Close();
            downStream.Close();
            leftStream.Close();
            rightStream.Close();

            stopWatch.Stop();
            player.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(songName + ".xml");
            sw.WriteLine("<Notes>\n<KeyUP>");
            Write_Times("UP", sw);
            sw.WriteLine("</KeyUP>\n<KeyDOWN>");

            Write_Times("DOWN", sw);
            sw.WriteLine("</KeyDOWN>\n<KeyLEFT>");

            Write_Times("LEFT", sw);
            sw.WriteLine("</KeyLEFT>\n<KeyRIGHT>");

            Write_Times("RIGHT", sw);
            sw.WriteLine("</KeyRIGHT>\n</Notes>");
            sw.Close();

            button5_Click(sender,e);
        }

        private void Write_Times(string Direction, StreamWriter sw)
        { 
            string line;
            float temp;
            StreamReader sr = new StreamReader(fileName + Direction +".txt");
            while ((line = sr.ReadLine()) != null)
            {
                temp = float.Parse(line);
                sw.WriteLine("<Time>" + Math.Round(temp / 1000, 2) + "</Time>");
            }
            sr.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            File.Delete(fileName + "UP.txt");
            File.Delete(fileName + "DOWN.txt");
            File.Delete(fileName + "LEFT.txt");
            File.Delete(fileName + "RIGHT.txt");
        }
    }
}
