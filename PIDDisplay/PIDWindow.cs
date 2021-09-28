using System;
using System.Collections.Generic;
using UnityEngine;

namespace PIDDisplay
{
    public class PIDWindow
    {
        private bool init = false;
        private Rect windowRect;
        private Texture2D drawTexture;
        private int currentX = 0;
        private int currentY = 0;
        private Queue<PIDSample> newSamples = new Queue<PIDSample>();
        private int displayMode = 0;
        private Dictionary<int, string> modeStrings = new Dictionary<int, string>();
        public const int NUMBER_OF_SAMPLES = 640;
        private string pString;
        private string iString;
        private string dString;
        private Autopilot.PID watchPID = null;


        private void Init()
        {
            init = true;
            drawTexture = new Texture2D(NUMBER_OF_SAMPLES, 480);
            windowRect = new Rect(100, 100, drawTexture.width + 50, drawTexture.height + 50);

            for (int x = 0; x < drawTexture.width; x++)
            {
                for (int y = 0; y < drawTexture.height; y++)
                {
                    drawTexture.SetPixel(x, y, Color.black);
                }
                drawTexture.SetPixel(x, drawTexture.height / 2, Color.gray);
            }
            drawTexture.Apply();
            modeStrings[0] = "Pitch";
            modeStrings[1] = "Roll";
            modeStrings[2] = "Speed";
            modeStrings[3] = "Vertical Speed";
            modeStrings[4] = "Altitude";
            modeStrings[5] = "Heading";
        }

        public void Draw()
        {
            if (!init)
            {
                Init();
            }
            windowRect = PreventOffscreenWindow(GUILayout.Window(2348589, windowRect, DrawContent, "PIDDisplay"));
        }

        public void Update()
        {
            lock (newSamples)
            {
                while (newSamples.Count > 0)
                {
                    PIDSample pidSample = newSamples.Dequeue();
                    int nextX = (currentX + 1) % drawTexture.width;
                    for (int y = 0; y < drawTexture.height; y++)
                    {
                        drawTexture.SetPixel(currentX, y, Color.black);
                    }
                    for (int y = 0; y < drawTexture.height; y++)
                    {
                        drawTexture.SetPixel(nextX, y, Color.red);
                    }
                    int offset = drawTexture.height / 2;
                    int pPix = Clamp((int)(pidSample.p * offset + offset));
                    int iPix = Clamp((int)(pidSample.i * offset + offset));
                    int dPix = Clamp((int)(pidSample.d * offset + offset));
                    int tPix = Clamp((int)(pidSample.total * offset + offset));
                    drawTexture.SetPixel(currentX, pPix, Color.red);
                    drawTexture.SetPixel(currentX, iPix, Color.green);
                    drawTexture.SetPixel(currentX, dPix, Color.blue);
                    drawTexture.SetPixel(currentX, tPix, Color.white);
                    drawTexture.SetPixel(currentX, offset, Color.gray);
                    drawTexture.Apply();
                    currentX = nextX;
                    currentY = (currentY + 1) % (drawTexture.height);
                }
            }
        }

        public int Clamp(int input)
        {
            int retVal = input;
            if (retVal < 0)
            {
                retVal = 0;
            }
            if (retVal >= drawTexture.height)
            {
                retVal = drawTexture.height - 1;
            }
            return retVal;
        }

        public void ReportSample(PIDSample pidSample)
        {
            lock (newSamples)
            {
                newSamples.Enqueue(pidSample);
            }
        }

        private void DrawContent(int windowID)
        {
            GUILayout.BeginVertical();
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            if (GUILayout.Button(modeStrings[displayMode]))
            {
                watchPID = null;
                displayMode = (displayMode + 1) % 3;
            }
            GUILayout.Box(drawTexture);
            GUILayout.BeginHorizontal();
            GUILayout.Label("P", GUILayout.MaxWidth(30));
            pString = GUILayout.TextField(pString);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("I", GUILayout.MaxWidth(30));
            iString = GUILayout.TextField(iString);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("D", GUILayout.MaxWidth(30));
            dString = GUILayout.TextField(dString);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Save"))
            {
                SavePIDs();
            }
            GUILayout.EndVertical();
        }

        public int GetMode()
        {
            return displayMode;
        }

        public void SetPID(Autopilot.PID pid)
        {
            if (watchPID == null)
            {
                watchPID = pid;
                pString = pid.kP.ToString();
                iString = pid.kI.ToString();
                dString = pid.kD.ToString();
            }
        }

        public void SavePIDs()
        {
            if (float.TryParse(pString, out float pFloat))
            {
                watchPID.kP = pFloat;
            }
            if (float.TryParse(iString, out float iFloat))
            {
                watchPID.kI = iFloat;
            }
            if (float.TryParse(dString, out float dFloat))
            {
                watchPID.kD = dFloat;
            }
            pString = watchPID.kP.ToString();
            iString = watchPID.kI.ToString();
            dString = watchPID.kD.ToString();
        }

        public Rect PreventOffscreenWindow(Rect input)
        {
            int xMax = Screen.width - (int)input.width;
            int yMax = Screen.height - (int)input.height;
            if (input.x < 0)
            {
                input.x = 0;
            }
            if (input.y < 0)
            {
                input.y = 0;
            }
            if (input.x > xMax)
            {
                input.x = xMax;
            }
            if (input.y > yMax)
            {
                input.y = yMax;
            }
            return input;
        }
    }
}
