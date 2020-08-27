using System;
using System.Reflection;
using FSControl;

namespace PIDDisplay
{
    public static class PIDCrackerOpener
    {
        private static bool init = false;
        private static FieldInfo pField;
        private static FieldInfo iField;
        private static FieldInfo dField;
        private static FieldInfo outField;

        public static PIDSample GetSample(FBWAxisPID pid)
        {
            if (!init)
            {
                Init();
            }
            PIDSample pidSample = new PIDSample();
            pidSample.p = (float)pField.GetValue(pid);
            pidSample.i = (float)iField.GetValue(pid);
            pidSample.d = (float)dField.GetValue(pid);
            pidSample.total = (float)outField.GetValue(pid);
            return pidSample;
        }

        private static void Init()
        {
            init = true;
            pField = typeof(PID32).GetField("p", BindingFlags.NonPublic | BindingFlags.Instance);
            iField = typeof(PID32).GetField("i", BindingFlags.NonPublic | BindingFlags.Instance);
            dField = typeof(PID32).GetField("d", BindingFlags.NonPublic | BindingFlags.Instance);
            outField = typeof(FBWAxisPID).GetField("vOut", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
