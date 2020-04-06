/*
 * Alex Kitt (featuring Josh Smallwood)
 * December 19, 2017 (original time of writing: September 17, 2017 - something something Austrilian Heritage Day)
 * CompSci 40S
 * Dann Dann Revolution
 * IDirector: This interface allows entities to be aware of and interact with each other.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dann_Dann_Revolution
{
    internal interface IDirector: IEntity
    {
        // The only thing that's actually supposed to be in this file!
        /// <summary> List of things that do stuff - amazing </summary>
        List<IEntity> EntityList { get; set; }

		/// <summary>
		/// FilterList: This method filters the entityList for entities that this director deals with
		/// </summary>
		void FilterList();
	}
}