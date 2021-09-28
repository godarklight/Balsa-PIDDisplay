using System;
using BalsaCore;
using UnityEngine;
using Autopilot;
using AutopilotCommon;
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
                AutopilotComponent ac = v.GetComponent<AutopilotComponent>();
                if (ac != null)
                {
                    PIDSample pidSample = null;
                    switch (pidWindow.GetMode())
                    {
                        case 0:
                            pidSample = new PIDSample(ac.pitchPid);
                            pidWindow.SetPID(ac.pitchPid);
                            break;
                        case 1:
                            pidSample = new PIDSample(ac.rollPid);
                            pidWindow.SetPID(ac.rollPid);
                            break;
                        case 2:
                            pidSample = new PIDSample(ac.speedPid);
                            pidWindow.SetPID(ac.speedPid);
                            break;
                        case 3:
                            pidSample = new PIDSample(ac.verticalSpeedPid);
                            pidWindow.SetPID(ac.verticalSpeedPid);
                            break;
                        case 4:
                            pidSample = new PIDSample(ac.altitudePid);
                            pidWindow.SetPID(ac.altitudePid);
                            break;
                        case 5:
                            pidSample = new PIDSample(ac.headingPid);
                            pidWindow.SetPID(ac.headingPid);
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