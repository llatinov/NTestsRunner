using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutomationRhapsody.NTestsRunner
{
    internal class PreventLock
    {
        private static Timer preventSleepTimer = null;
        private static int pingTimeSeconds = 30;

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_CONTINUOUS = 0x80000000
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE flags);

        public static void DisableSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            preventSleepTimer = new Timer(new TimerCallback(PingDeviceToKeepAwake), null, 0, pingTimeSeconds * 1000);
        }

        public static void EnableSleep()
        {
            if (preventSleepTimer != null)
            {
                preventSleepTimer.Dispose();
            }
            preventSleepTimer = null;
        }

        private static void PingDeviceToKeepAwake(object extra)
        {
            try
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
                IntPtr Handle = FindWindow("SysListView32", "FolderView");

                if (Handle == IntPtr.Zero)
                {
                    SetForegroundWindow(Handle);
                }
            }
            catch { }
        }
    }
}
