using System;

namespace AutomationRhapsody.NTestsRunner.Types
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethod : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class TestClass : Attribute { }
}
