/*
 * ✋ WARNING ✋
 * This class does NOT belong to Alex Kitt, it was not made on August 8, 2017, and it was not made with Project Tribute in mind in any way!
 * (And no, it wasn't made on December 19, 2017 either, and it definitely wasn't made for CompSci 40S)
 * Rather, this is from the blog of Roy T.!
 * http://roy-t.nl/2010/08/25/xna-accessing-contentmanager-and-graphicsdevice-anywhere-anytime-the-gameservicecontainer.html
 * And then John Pollick cleaned it up by expiditing the creation of the container variable! Your name is going in the credits, John.
 * GameServices: This class allows us to put certain 'services' (objects?) in a 'container' (list?). 
 * Since this class is static, these services can then be accessed from anywhere!
 * No, this isn't the same as global variables! This class is mostly stateless, so it's more like using a header
 */

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
    public static class GameServices
    {
        private static GameServiceContainer container = new GameServiceContainer();

        /// <summary>
        /// Get: This method lets the caller access any service that's been added to the container
        /// </summary>
        /// <typeparam name="T">The type of service the caller wants to access</typeparam>
        /// <returns>A service of type T (yay for generics!)</returns>
        public static T Get<T>()
        {
            return (T)container.GetService(typeof(T));
        }

        /// <summary>
        /// Add: This method adds a service to the container
        /// </summary>
        /// <typeparam name="T">The type of service being added</typeparam>
        /// <param name="service">Is it just me, or does the word 'service' look really weird now? Huh. Anyway, this is the service being added</param>
        public static void Add<T>(T service)
        {
            container.AddService(typeof(T), service);
        }

        /// <summary>
        /// Add: This method removes a service from the container
        /// </summary>
        /// <typeparam name="T">The type of service the caller wants to remove</typeparam>
        public static void Remove<T>()
        {
            container.RemoveService(typeof(T));
        }
    }
}