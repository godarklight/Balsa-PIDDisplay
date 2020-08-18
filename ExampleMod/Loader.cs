using System;
using BalsaCore;
using UnityEngine;

namespace ExampleNamespace
{
    [BalsaAddon]
    public class Loader
    {
        private static GameObject go;
        private static MonoBehaviour mod;

        //Game start
        [BalsaAddonInit]
        public static void BalsaInit()
        {
            GameEvents.Game.OnSceneryFinishedLoading.AddListener(MenuLoad);
            //Move to menu load if you want to load later.
            if (go == null)
            {
                go = new GameObject();
                mod = go.AddComponent<ExampleMod>();
                ExampleMod.Log("Initialized!");
            }
        }

        //Main menu load
        public static void MenuLoad()
        {
        }

        //Game exit
        [BalsaAddonFinalize]
        public static void BalsaFinalize()
        {
        }
    }
}
