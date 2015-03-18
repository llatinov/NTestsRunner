using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AutomationRhapsody.NTestsRunner
{
    public class NTestsRunnerSettings
    {
        private string testResultsDir;
        public string TestResultsDir
        {
            get
            {
                return testResultsDir;
            }
            set
            {
                testResultsDir = value;
                if (!testResultsDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    testResultsDir += Path.DirectorySeparatorChar;
                }
                testResultsDir += DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + Path.DirectorySeparatorChar;
            }
        }
        public int MaxTestCaseRuntimeMinutes { get; set; }
        public List<string> TestsToExecute { get; set; }
        public bool PreventScreenLock { get; set; }

        public NTestsRunnerSettings()
        {
            this.TestResultsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            this.MaxTestCaseRuntimeMinutes = 15;
            this.TestsToExecute = new List<string>();
            this.PreventScreenLock = false;
        }
    }
}
