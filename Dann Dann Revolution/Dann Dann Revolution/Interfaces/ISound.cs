/*
 * Joshua Smallwood (a grad helping with the final project? Is this legal? Anyways, all the code here and most of the commenting is Josh doing.)
 * December 19, 2017 (original time of writing: December 12, 2017 -- The day I didn't do as well as the calc exam as I thought I would and this is how I'm coping.)
 * CompSci 40S
 * Dann Dann Revolution
 * ISound: This interface ensures that everything that may have sound involved (be it music, sound effects, ambiance, etc.)
 * has a list of sounds and a method for initializing what file locations the sounds are located in.
 * 
 * This is my first time using visual studio in so long it feels so good i feel so freeeee
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Dann_Dann_Revolution
{
	internal interface ISound
	{ 
		// Properties
		/// <summary> List of sounds, contains info about whether each sound is intended to loop or not. </summary>
		List<SoundEffect> SoundList { get; set; }

		/// <summary>
		/// InitializeSounds: This method should always be called in the constructor. It initializes the file location and whether the audio file in question should be looped.
		/// </summary>
		void InitializeSounds();
	}
}
