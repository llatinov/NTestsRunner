using System;
using System.Collections.Generic;

namespace AutomationRhapsody.NTestsRunner.Types
{
    public class TestCaseResult
    {
        public List<Verification> Verifications = new List<Verification>();
        public string Name { get; set; }
        public int VerificationsFailed { get; set; }
        public int VerificationsPassed { get; set; }
        public TimeSpan Time { get; set; }
    }
}
