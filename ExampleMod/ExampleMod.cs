using System;
using BalsaCore;
using UnityEngine;
namespace ExampleNamespace
{

    public class ExampleMod : MonoBehaviour
    {
        //Called when game object is started
        public void Start()
        {
        }

        //Called once per frame
        public void Update()
        {
            Log("Update");
        }

        //Called once per frame after Update
        public void LateUpdate()
        {
        }

        //Called once per physics frame (50hz)
        public void FixedUpdate()
        {
            Log("FixedUpdate");
        }

        //Called to draw the old GUI. Obsolete.
        public void OnGUI()
        {
        }

        //Called when the mod object is enabled
        public void OnEnable()
        {
        }

        //Called when the mod object is disabled.
        public void OnDisable()
        {
        }

        //It's nice to identify in the log where things came from
        public static void Log(string text)
        {
            Debug.Log($"{Time.realtimeSinceStartup} [ExampleMod] {text}");
        }
    }
}