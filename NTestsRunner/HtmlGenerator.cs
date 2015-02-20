using AutomationRhapsody.NTestsRunner.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AutomationRhapsody.NTestsRunner
{
    class HtmlGenerator
    {
        public static void SaveResultsHtml(List<TestPlanResult> testPlans, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title>Test Results</title>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.AppendLine("<script>");
            sb.AppendLine("function displayProperties (element) {");
            sb.AppendLine("\tvar win = window.open('','JUnitSystemProperties','scrollbars=1,resizable=1');");
            sb.AppendLine("\tvar doc = win.document.open();");
            sb.AppendLine("\tdoc.write('<html><head><title>Details</title>');");
            sb.AppendLine("\tdoc.write('</head><body style=\"background:#eeeee0;\">');");
            sb.AppendLine("\tdoc.write('<pre style=\"font:normal 68% verdana,arial,helvetica; color:#000000;\">' + element.parentNode.parentNode.childNodes[5].innerHTML + '</pre>');");
            sb.AppendLine("\tdoc.write('<div align=\"left\"><a href=\"javascript:window.close();\">Close</a></div>');");
            sb.AppendLine("\tdoc.write('</body></html>');");
            sb.AppendLine("\tdoc.close();");
            sb.AppendLine("\twin.focus();");
            sb.AppendLine("}");
            sb.AppendLine("</script>");
            sb.AppendLine("<style type=\"text/css\">");
            sb.AppendLine("body { font:normal 68% verdana,arial,helvetica; color:#000000; }");
            sb.AppendLine("table.details { border: 0px; width: 95%; }");
            sb.AppendLine("table.details tr th { font-weight: bold; text-align:left; background:#a6caf0; padding: 5px; }");
            sb.AppendLine("table.details tr td { background:#ddddd0; padding: 5px; }");
            sb.AppendLine("table.details tr:nth-of-type(odd) td { background:#eeeee0; padding: 5px; }");
            sb.AppendLine(".pass { color:green; }");
            sb.AppendLine(".fail, .fail a { font-weight:bold; color:red; }");
            sb.AppendLine(".width80 { width: 80%; }");
            sb.AppendLine(".width10 { width: 10%; }");
            sb.AppendLine(".width5 { width: 5%; }");
            sb.AppendLine(".nowrap { white-space:nowrap; }");
            sb.AppendLine("p { line-height:1.5em; margin-top:0.5em; margin-bottom:1.0em; }");
            sb.AppendLine("h1 { margin: 0px 0px 5px; font: 165% verdana,arial,helvetica; }");
            sb.AppendLine("h2 { margin-top: 1em; margin-bottom: 0.5em; font: bold 125% verdana,arial,helvetica; }");
            sb.AppendLine("h3 { margin-bottom: 0.5em; font: bold 115% verdana,arial,helvetica; }");
            sb.AppendLine("h4 { margin-bottom: 0.5em; font: bold 100% verdana,arial,helvetica; }");
            sb.AppendLine("h5 { margin-bottom: 0.5em; font: bold 100% verdana,arial,helvetica; }");
            sb.AppendLine("h6 { margin-bottom: 0.5em; font: bold 100% verdana,arial,helvetica; }");
            sb.AppendLine("hr { height: 1px; width: 95%; margin-left: 0px; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<a id=\"top\"></a>");
            sb.AppendLine("<h1>Test Results</h1>");
            sb.AppendLine("<hr />");
            sb.Append(GenerateHtmlMainSummary(testPlans));
            sb.Append(GenerateHtmlTestPlanSummary(testPlans));
            sb.Append(GenerateHtmlTestCaseSummary(testPlans));
            sb.Append(GenerateHtmlVerificationsSummary(testPlans));
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(sb.ToString());
            }
        }

        private static string GenerateHtmlMainSummary(List<TestPlanResult> testPlans)
        {
            int tcPassed = 0; int tcFailed = 0;
            int verPassed = 0; int verFailed = 0;
            TimeSpan timeTotal = TimeSpan.Zero;
            foreach (TestPlanResult testPlan in testPlans)
            {
                tcFailed += testPlan.TestCasesFailed;
                tcPassed += testPlan.TestCasesPassed;
                verPassed += testPlan.VerificationsPassed;
                verFailed += testPlan.VerificationsFailed;
                timeTotal = timeTotal.Add(testPlan.Time);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h2>Summary</h2>");
            sb.AppendLine("<table class=\"details\">");
            sb.AppendLine("\t<tr>");
            sb.AppendLine("\t\t<th class=\"width10\">Tests</th>");
            sb.AppendLine("\t\t<th class=\"pass width10\">Pass</th>");
            sb.AppendLine("\t\t<th class=\"fail width10\">Fail</th>");
            sb.AppendLine("\t\t<th class=\"width10\">Success rate</th>");
            sb.AppendLine("\t\t<th class=\"width10\">Verifications</th>");
            sb.AppendLine("\t\t<th class=\"pass width10\">VerificationPassed</th>");
            sb.AppendLine("\t\t<th class=\"fail width10\">VerificationFailed</th>");
            sb.AppendLine("\t\t<th class=\"width10\">Verifications success rate</th>");
            sb.AppendLine("\t\t<th class=\"width10\">Time</th>");
            sb.AppendLine("\t</tr>");
            sb.AppendLine("\t<tr>");
            sb.AppendLine("\t\t<td>" + (tcPassed + tcFailed) + "</td>");
            sb.AppendLine("\t\t<td class=\"pass\">" + tcPassed + "</td>");
            sb.AppendLine("\t\t<td" + GenerateHtmlFailedClass(tcFailed) + ">" + tcFailed + "</td>");
            sb.AppendLine("\t\t<td>" + GenerateHtmlPercentage(tcPassed, tcPassed + tcFailed) + "</td>");
            sb.AppendLine("\t\t<td>" + (verPassed + verFailed) + "</td>");
            sb.AppendLine("\t\t<td class=\"pass\">" + verPassed + "</td>");
            sb.AppendLine("\t\t<td" + GenerateHtmlFailedClass(verFailed) + ">" + verFailed + "</td>");
            sb.AppendLine("\t\t<td>" + GenerateHtmlPercentage(verPassed, verPassed + verFailed) + "</td>");
            sb.AppendLine("\t\t<td>" + GenerateHtmlTime(timeTotal) + "</td>");
            sb.AppendLine("\t</tr>");
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        private static string GenerateHtmlTestPlanSummary(List<TestPlanResult> testPlans)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h2>Test Plans Summary</h2>");
            sb.AppendLine("<table class=\"details\">");
            sb.AppendLine("\t<tr>");
            sb.AppendLine("\t\t<th>Name</th>");
            sb.AppendLine("\t\t<th class=\"width5\">Tests</th>");
            sb.AppendLine("\t\t<th class=\"pass width5\">Pass</th>");
            sb.AppendLine("\t\t<th class=\"fail width5\">Fail</th>");
            sb.AppendLine("\t\t<th class=\"width5\">Success rate</th>");
            sb.AppendLine("\t\t<th class=\"width5\">Verifications</th>");
            sb.AppendLine("\t\t<th class=\"pass width5\">Ver.Passed</th>");
            sb.AppendLine("\t\t<th class=\"fail width5\">Ver.Failed</th>");
            sb.AppendLine("\t\t<th class=\"width5\">Verifications success rate</th>");
            sb.AppendLine("\t\t<th class=\"width5\">Time</th>");
            sb.AppendLine("\t</tr>");
            foreach (TestPlanResult testPlan in testPlans)
            {
                sb.AppendLine("\t<tr>");
                sb.AppendLine("\t\t<td><a href=\"#" + testPlan.Name + "\"" + GenerateHtmlFailedClass(testPlan.TestCasesFailed) + ">" + testPlan.Name + "</a></td>");
                sb.AppendLine("\t\t<td>" + (testPlan.TestCasesPassed + testPlan.TestCasesFailed) + "</td>");
                sb.AppendLine("\t\t<td class=\"pass\">" + testPlan.TestCasesPassed + "</td>");
                sb.AppendLine("\t\t<td" + GenerateHtmlFailedClass(testPlan.TestCasesFailed) + ">" + testPlan.TestCasesFailed + "</td>");
                sb.AppendLine("\t\t<td>" + GenerateHtmlPercentage(testPlan.TestCasesPassed, testPlan.TestCasesPassed + testPlan.TestCasesFailed) + "</td>");
                sb.AppendLine("\t\t<td>" + (testPlan.VerificationsPassed + testPlan.VerificationsFailed) + "</td>");
                sb.AppendLine("\t\t<td class=\"pass\">" + testPlan.VerificationsPassed + "</td>");
                sb.AppendLine("\t\t<td" + GenerateHtmlFailedClass(testPlan.VerificationsFailed) + ">" + testPlan.VerificationsFailed + "</td>");
                sb.AppendLine("\t\t<td>" + GenerateHtmlPercentage(testPlan.VerificationsPassed, testPlan.VerificationsPassed + testPlan.VerificationsFailed) + "</td>");
                sb.AppendLine("\t\t<td class=\"nowrap\">" + GenerateHtmlTime(testPlan.Time) + "</td>");
                sb.AppendLine("\t</tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        private static string GenerateHtmlTestCaseSummary(List<TestPlanResult> testPlans)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TestPlanResult testPlan in testPlans)
            {
                sb.AppendLine("<hr />");
                sb.AppendLine("<a id=\"" + testPlan.Name + "\"></a>");
                sb.AppendLine("<h3>Test Plan '" + testPlan.Name + "'</h3>");
                sb.AppendLine("<table class=\"details\">");
                sb.AppendLine("\t<tr>");
                sb.AppendLine("\t\t<th>Name</th>");
                sb.AppendLine("\t\t<th class=\"width5\">Verifications</th>");
                sb.AppendLine("\t\t<th class=\"pass width5\">Ver.Passed</th>");
                sb.AppendLine("\t\t<th class=\"fail width5\">Ver.Failed</th>");
                sb.AppendLine("\t\t<th class=\"width5\">Verifications success rate</th>");
                sb.AppendLine("\t\t<th class=\"width5\">Time</th>");
                sb.AppendLine("\t</tr>");
                // Generate records only if there are Verifications
                foreach (TestCaseResult testCase in testPlan.TestCases.Where(x => x.Verifications.Any()))
                {
                    sb.AppendLine("\t<tr>");
                    sb.AppendLine("\t\t<td><a href=\"#" + testPlan.Name + "_" + testCase.Name + "\"" + GenerateHtmlFailedClass(testCase.VerificationsFailed) + ">" + testCase.Name + "</a></td>");
                    sb.AppendLine("\t\t<td>" + (testCase.VerificationsPassed + testCase.VerificationsFailed) + "</td>");
                    sb.AppendLine("\t\t<td class=\"pass\">" + testCase.VerificationsPassed + "</td>");
                    sb.AppendLine("\t\t<td" + GenerateHtmlFailedClass(testCase.VerificationsFailed) + ">" + testCase.VerificationsFailed + "</td>");
                    sb.AppendLine("\t\t<td>" + GenerateHtmlPercentage(testCase.VerificationsPassed, testCase.VerificationsPassed + testCase.VerificationsFailed) + "</td>");
                    sb.AppendLine("\t\t<td class=\"nowrap\">" + GenerateHtmlTime(testCase.Time) + "</td>");
                    sb.AppendLine("\t</tr>");
                }
                sb.AppendLine("</table>");
                sb.AppendLine("<a href=\"#top\">Back to top</a>");
            }
            return sb.ToString();
        }

        private static string GenerateHtmlVerificationsSummary(List<TestPlanResult> testPlans)
        {
            StringBuilder sb = new StringBuilder();
            string startString = "Desktop captured at: ";
            string endString = ".png";
            foreach (TestPlanResult testPlan in testPlans)
            {
                // Generate records only if there are Verifications
                foreach (TestCaseResult testCase in testPlan.TestCases.Where(x => x.Verifications.Any()))
                {
                    sb.AppendLine("<hr />");
                    sb.AppendLine("<a id=\"" + testPlan.Name + "_" + testCase.Name + "\"></a>");
                    sb.AppendLine("<h3>Test Case '" + testCase.Name + "'</h3>");
                    sb.AppendLine("<table class=\"details\">");
                    sb.AppendLine("\t<tr>");
                    sb.AppendLine("\t\t<th>Result</th>");
                    sb.AppendLine("\t\t<th>Time</th>");
                    sb.AppendLine("\t\t<th class=\"width80\">Details</th>");
                    sb.AppendLine("\t\t<th>Image</th>");
                    sb.AppendLine("\t</tr>");
                    foreach (Verification verification in testCase.Verifications)
                    {
                        sb.AppendLine("\t<tr class=\"" + (verification is VerificationFailed ? "fail" : "pass") + "\">");
                        sb.AppendLine("\t\t<td>" + (verification is VerificationFailed ? "Failed" : "Passed") + "</td>");
                        sb.AppendLine("\t\t<td>" + verification.ExecutedAt.TimeStamp() + "</td>");
                        string details = verification.Result;
                        string image = string.Empty;
                        if (details.Contains(startString) && details.Contains(endString))
                        {
                            int start = details.IndexOf(startString) + startString.Length;
                            int end = details.IndexOf(endString, start) + endString.Length;
                            string imageString = details.Substring(start, end - start);
                            image = "<a target=\"_blank\" href=\"" + HttpUtility.UrlPathEncode(imageString) + "\" style=\"float: left\"><img alt=\"" + testCase.Name + " Image\" width=\"150\" src=\"" + HttpUtility.UrlPathEncode(imageString) + "\" /></a>";
                            details = details.Replace(startString, "").Replace(imageString, "");
                        }
                        sb.AppendLine("\t\t<td>" + details + "</td>");
                        sb.AppendLine("\t\t<td>" + image + (verification is VerificationFailed ? "<br /><a href=\"javascript:void(0);\" onClick=\"displayProperties(this);\">More details</a>" : "") + "</td>");
                        sb.AppendLine("\t</tr>");
                    }
                    sb.AppendLine("</table>");
                    sb.AppendLine("<a href=\"#" + testPlan.Name + "\">Back to Test Plan</a>");
                }
            }
            return sb.ToString();
        }

        private static string GenerateHtmlPercentage(int value, int total)
        {
            if (total == 0)
            {
                return "0%";
            }
            else
            {
                return Math.Round((decimal)((value * 100) / (total)), 0) + "%";
            }
        }

        private static string GenerateHtmlTime(TimeSpan time)
        {
            return string.Format("{0:hh\\:mm\\:ss} ({1} seconds)", time, (int)time.TotalSeconds);
        }

        private static string GenerateHtmlFailedClass(int value)
        {
            return value > 0 ? " class=\"fail\"" : string.Empty;
        }
    }
}
