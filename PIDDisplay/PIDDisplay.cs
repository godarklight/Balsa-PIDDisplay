using System;
using BalsaCore;
using UnityEngine;
namespace PIDDisplay
{

    public class PIDDisplayMod : MonoBehaviour
    {
        PIDWindow pidWindow;

        //Called when game object is started
        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        //Called once per frame
        public void Update()
        {
            if (GameLogic.CurrentScene == GameScenes.FLIGHT && GameLogic.LocalPlayerVehicle != null)
            {
                Vehicle v = GameLogic.LocalPlayerVehicle;
                if (v.Autotrim != null && v.Autotrim.pitch != null && v.Autotrim.roll != null && v.Autotrim.yaw != null && v.Autotrim.autoTrimEnabled)
                {
                    PIDSample pidSample = null;
                    switch (pidWindow.GetMode())
                    {
                        case 0:
                            pidSample = PIDCrackerOpener.GetSample(v.Autotrim.pitch);
                            pidWindow.SetPID(v.Autotrim.pitch);
                            break;
                        case 1:
                            pidSample = PIDCrackerOpener.GetSample(v.Autotrim.roll);
                            pidWindow.SetPID(v.Autotrim.roll);
                            break;
                        case 2:
                            pidSample = PIDCrackerOpener.GetSample(v.Autotrim.yaw);
                            pidWindow.SetPID(v.Autotrim.yaw);
                            break;
                    }
                    pidWindow.ReportSample(pidSample);
                }
            }
            if (pidWindow == null && GameLogic.CurrentScene == GameScenes.FLIGHT)
            {
                pidWindow = new PIDWindow();
            }
            if (pidWindow != null)
            {
                pidWindow.Update();
            }
        }

        //Called once per frame after Update
        public void LateUpdate()
        {
        }

        //Called once per physics frame (50hz)
        public void FixedUpdate()
        {
        }

        //Called to draw the old GUI. Obsolete.
        public void OnGUI()
        {
            if (pidWindow != null)
            {
                pidWindow.Draw();
            }
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
            Debug.Log($"{Time.realtimeSinceStartup} [PIDDisplay] {text}");
        }
    }
}