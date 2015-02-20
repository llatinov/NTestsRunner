using System.Collections.Generic;
using System.Linq;
using System;

namespace AutomationRhapsody.NTestsRunner.Types
{
    public class TestPlanResult
    {
        public List<TestCaseResult> TestCases = new List<TestCaseResult>();
        public string Name { get; set; }
        public int TestCasesPassed { get; private set; }
        public int TestCasesFailed { get; private set; }
        public int VerificationsPassed { get; private set; }
        public int VerificationsFailed { get; private set; }
        public TimeSpan Time { get; private set; }

        public void Count()
        {
            foreach (TestCaseResult testCase in TestCases)
            {
                testCase.VerificationsPassed = testCase.Verifications.Where(x => x is VerificationPassed).Count();
                this.VerificationsPassed += testCase.VerificationsPassed;

                testCase.VerificationsFailed = testCase.Verifications.Where(x => x is VerificationFailed).Count();
                this.VerificationsFailed += testCase.VerificationsFailed;

                Time = Time.Add(testCase.Time);

                if (testCase.VerificationsFailed > 0)
                {
                    this.TestCasesFailed++;
                }
                else
                {
                    this.TestCasesPassed++;
                }
            }
        }
    }
}
