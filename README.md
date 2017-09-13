# NTestsRunner
## About
NTestsRunner is a tool for running functional automated tests. It gives you flexibility to use many verification points in one test. Tests are run in consequence order in their containing class. Results are saved in jUnit XML and HTML.

## Configurations
- **TestResultsDir** – where test results will be saved. If none is provided then current directory will be used. Inside TestResultsDir folder with time stamp is automatically created and actual results are saved inside this folder. Each run new folder is created according to time of run.
- **MaxTestCaseRuntimeMinutes** – maximum allowed time per test method. If execution of given method takes more than given minutes then current test is aborted and marked as failed. Execution continues with next test. Default value is 15 minutes.
- **TestsToExecute** – list of tests to be executed. By default all test classes with [TestClass] attribute are taken. In this configuration test classes can be configured as string values. Only classes given here are executed if they exits.
- **PreventScreenLock** – boolean property to enable or disable functionality that will prevent system from going to sleep or account to lock. Default value is


## Usage
1. Create new console application in MS Visual Studio.
2. Add reference to NTestsRunner.dll.
3. Create test classes with test methods. Important is that test methods should have specific signature. It should accept List of Verifications as argument:

	`public void TestMethod(List<Verification> verifications)`

4. In console application’s main method instantiate NTestsRunner, configure it and run the tests.

    	static void Main(string[] args)
    	{
			NTestsRunnerSettings settings = new NTestsRunnerSettings();
            settings.TestResultsDir = @"C:\temp";
    		
			NTestsRunner runner = new NTestsRunner(settings);
            runner.Execute();
    	}
    
## Reference
See following <a target="_blank" href="https://github.com/llatinov/NTestsRunner">GitHub</a> project for reference how to use NTestsRunner. See NTestsRunner <a target="_blank" href="http://automationrhapsody.com/ntestsrunner/">home page</a> for more details.
