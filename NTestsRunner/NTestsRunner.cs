﻿using AutomationRhapsody.NTestsRunner.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutomationRhapsody.NTestsRunner
{
    public class NTestsRunner
    {
        private NTestsRunnerSettings settings;

        public NTestsRunner(NTestsRunnerSettings settings)
        {
            this.settings = settings;
        }

        public void Execute()
        {
            if (this.settings.PreventScreenLock)
            {
                PreventLock.DisableSleep();
            }

            Assembly assembly = Assembly.GetCallingAssembly();
            List<TestPlanResult> testPlans = new List<TestPlanResult>();
            List<Type> types = GetTestsToExecute(assembly);
            foreach (Type type in types)
            {
                object[] attributes = type.GetCustomAttributes(false);
                foreach (object attr in attributes)
                {
                    if (attr is TestClass)
                    {
                        try
                        {
                            TestPlanResult result = ExecuteTestPlan(type);
                            testPlans.Add(result);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error during test execution: " + e.Message + e.StackTrace);
                        }
                    }
                    // Save results after each test plan
                    SaveResults(testPlans);
                }
            }

            SaveResults(testPlans);

            if (this.settings.PreventScreenLock)
            {
                PreventLock.EnableSleep();
            }
        }

        #region Private methods
        private List<Type> GetTestsToExecute(Assembly assembly)
        {
            List<Type> typesToExecute = new List<Type>();
            // If there are manually provided tests run them
            Type[] types = assembly.GetTypes();
            if (this.settings.TestsToExecute != null && this.settings.TestsToExecute.Any())
            {
                foreach (string testToExecute in this.settings.TestsToExecute)
                {
                    Type type = types.Where(x => x.Name == testToExecute).FirstOrDefault();
                    if (type != null)
                    {
                        typesToExecute.Add(type);
                    }
                    else
                    {
                        Console.WriteLine("Test class '" + testToExecute + "' cannot be instantiated. Make sure this class exists.");
                    }
                }
            }
            // Take all types that have TestClass attribute
            else
            {
                foreach (Type type in types)
                {
                    foreach (object attribute in type.GetCustomAttributes(false))
                    {
                        if (attribute is TestClass)
                        {
                            typesToExecute.Add(type);
                            continue;
                        }
                    }
                }
            }
            return typesToExecute;
        }

        private TestPlanResult ExecuteTestPlan(Type type)
        {
            object disposable = Activator.CreateInstance(type);
            TestPlanResult testPlan = new TestPlanResult();
            MethodInfo[] methods = disposable.GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                object[] attributes = method.GetCustomAttributes(false);
                foreach (object attr in attributes)
                {
                    if (attr is TestMethod)
                    {
                        TestCaseResult testCase = new TestCaseResult();
                        long start = DateTime.Now.Ticks;
                        try
                        {
                            Task task = new TaskFactory().StartNew(() => method.Invoke(disposable, new object[] { testCase.Verifications }));
                            task.Wait(TimeSpan.FromMinutes(this.settings.MaxTestCaseRuntimeMinutes));
                        }
                        catch (Exception ex)
                        {
                            ex = ex.InnerException.InnerException;
                            string failure = method.DeclaringType.Name + ": " + method.Name + " throws: " + ex.Message + ". " + Environment.NewLine + ex.StackTrace;
                            Console.WriteLine(failure);
                        }
                        long end = DateTime.Now.Ticks;
                        TimeSpan executionTime = new TimeSpan(end - start);
                        if ((int)executionTime.TotalMinutes >= this.settings.MaxTestCaseRuntimeMinutes)
                        {
                            testCase.Verifications.Add(new VerificationFailed("TestCase stopped as it has exceeded maximum allowed runtime of {0} minutes. To increase time change MaxTestCaseRuntimeMinutes settings value.", this.settings.MaxTestCaseRuntimeMinutes));
                        }
                        testCase.Name = method.Name;
                        testCase.Time = executionTime;
                        testPlan.TestCases.Add(testCase);
                    }
                }
            }
            testPlan.Name = disposable.GetType().Name;
            testPlan.Count();
            return testPlan;
        }

        private void SaveResultsXml(List<TestPlanResult> testPlans, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<testsuites>");
            foreach (TestPlanResult testPlan in testPlans)
            {
                sb.AppendLine("\t<testsuite name=\"" + testPlan.Name + "\" time=\"" + testPlan.Time + "\">");
                // Generate TestCase record only if there are Verifications
                foreach (TestCaseResult testCase in testPlan.TestCases.Where(x => x.Verifications.Any()))
                {
                    sb.AppendLine("\t\t<testcase name=\"" + testCase.Name + "\" time=\"" + testCase.Time + "\" classname=\"" + testPlan.Name + "\">");
                    foreach (Verification verification in testCase.Verifications.Where(x => x is VerificationFailed))
                    {
                        sb.AppendLine("\t\t\t<failure message=\"" + verification.Result + "\" />");
                    }
                    sb.AppendLine("\t\t</testcase>");
                }
                sb.AppendLine("\t</testsuite>");
            }
            sb.AppendLine("</testsuites>");

            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(sb.ToString());
            }
        }

        private void SaveResults(List<TestPlanResult> testPlans)
        {
            if (!Directory.Exists(this.settings.TestResultsDir))
            {
                Directory.CreateDirectory(this.settings.TestResultsDir);
            }

            SaveResultsXml(testPlans, this.settings.TestResultsDir + "Results.xml");
            HtmlGenerator.SaveResultsHtml(testPlans, this.settings.TestResultsDir + "Results.html");
        }
        #endregion
    }
}
