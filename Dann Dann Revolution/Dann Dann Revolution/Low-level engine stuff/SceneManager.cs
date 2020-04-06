/*
 * Alex Kitt
 * July 28, 2017
 * SceneManager: This class is, like, the real deal, yo
 * It manages scenes, as the name may imply. What's a scene, you ask? Why, nothing but a collection of sprites
 * (Plus some other logic, but we'll get to that later)!
 * At its core, the scene manager is a list of scenes. There will be a scene for every scenario in the game!
 * The last scene in the list gets updated every frame. This entails calling the update/draw methods of all the sprite it contains.
 * There will also be methods for scenes to spawn other scenes, moving execution to the newly-spawned scene, 
 * or to remove themselves from the list, moving execution to the previous scene.
 * In this way, the scene manager will emulate the last-in-first-out structure of the good ol' C++ console apps!
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
    // All these comments until the actual class declaration are just rambles. 
    // I'd like to keep them because they show the thought process behind certain decisions, but you don't gotta read 'em.

    // This is not a static class because I wanted to make it so only the main class can control sceneList, 
    // but we'll have to see if that makes it even makes it possible to, uh, manage scenes.

    // Here's an argument for making SceneManager public/global: it allows for behaviour similar to the stack, as calling the AddNewScene and EndScene
    // methods would be just like putting method calls and return statements inside methods, which is what all the rational-thinking people do.
    // Yeah, proooobably making this static and just (somehow) putting additional protection on Update and Draw, 
    // but first I want to see if I can find a way to keep this controlled solely by the main class.

    // Yep, we're making this static. 
    // Like, it's theoretically totally possible to have a system in which the sprites call events 
    // that notify their Scene to notify SceneManager to make the main class add and remove the scenes, but ya know what?
    // There comes a point when added security makes for extra work at best, and spaghettification at worst.
    // The only downsides to making SceneManager static internal is the intrinsic shame in using sorta-globals, 
    // and the fact that any class can now call the Update and Draw methods.
    // The former can be dealt with by means of some solid denial, while the latter... just don't call those methods, okay?

    /// <summary>
    /// The single most important part of the game engine, and it's just a glorified list.
    /// </summary>
    static internal class SceneManager
    {
        // For instance, here are the variables.
        /// <summary>
        /// The core of this class - a list of scenes, for which the last scene on the list has all of its sprites updated and drawn every frame
        /// </summary>
        private static List<Scene> sceneList = new List<Scene>();

        /// <summary> Look! It's my first event! 
        /// This makes it so the main game class knows when sceneList is empty and can thus do something about it </summary>
        public static event Action OhSnapNoScenes;
        /// <summary> Method called when sceneList is empty </summary>
        public static void OnNoScenes()
        {
            OhSnapNoScenes?.Invoke();
        }

        /// <summary>
        /// Update: This method updates the scene at the top of the stack - the end of the list, really.
        /// </summary>
        /// <param name="gameTime"> Object that keeps track of the time elapsed in real life </param>
        public static void Update(GameTime gameTime)
        {
            sceneList[sceneList.Count - 1].Update(gameTime);

            // There was gonna be some code here that handled the case of there being no scenes to start with, but you know what?
            // If we get to that point, the game deserves to crash.
        }

        /// <summary>
        /// Draw: This method draws the sprites of the scene at the top of the stack - the end of the list, really!
        /// </summary>
        /// <param name="spriteBatch"> Mysterious MonoGame object that you're not allowed to touch. Has the powers of displaying graphics! </param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            sceneList[sceneList.Count - 1].Draw(spriteBatch);
        }

        /// Methods to control sceneList
        /// Yeah, I know I could've made the scene list public to remove the need for this and RemoveScene, 
        /// but these methods restrict the control that any ol' scene can exert on the master list, which is a good thing.
        /// Scenes can only overwrite or remove themselves - all previous scenes are safe from tampering!
        /// <summary>
        /// AddNewScene: This method adds a new scene to the end of the list. 
        /// </summary>
        public static void AddNewScene(Scene newScene)
        {
            sceneList.Add(newScene);
        }

        /// <summary>
        /// EndScene: This method removes the last item of sceneList, 
        /// but for the sake of the stack metaphor let's say it ends the current scene and moves to the previous one. ;)
        /// </summary>
        public static void EndScene()
        {
            sceneList.RemoveAt(sceneList.Count - 1);

            // Raise the roof/event if the list is now empty
            if (!sceneList.Any())
                OnNoScenes();
        }
    }
}