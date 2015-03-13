# NTestsRunner
## About
NTestsRunner is a tool for running functional automated tests. It gives you flexibility to use many verification points in one test. Tests are run in consequence order in their containing class. Results are saved in jUnit XML and HTML.

## Usage
1. Create new console application in MS Visual Studio.
2. Add reference to NTestsRunner.dll.
3. Create test classes with test methods. Important is that test methods should have specific signature. It should accept List of Verifications as argument: <code>public void Initialise(List<Verification> verifications)</code>
4. In console application’s main method instantiate NTestsRunner, configure it and run the tests.

    	static void Main(string[] args)
    	{
    		NTestsRunner runner = new NTestsRunner();
    		runner.TestResultsDir = @"C:\temp";
    		runner.Execute();
    	}
    
## Reference
See following <a target="_blank" href="https://github.com/llatinov/SampleAppPlus">GitHub</a> project for reference how to use NTestsRunner. See NTestsRunner <a target="_blank" href="http://automationrhapsody.com/ntestsrunner/">home page</a> for more details.
