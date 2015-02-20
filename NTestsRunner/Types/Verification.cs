using System;
using System.Linq;

namespace AutomationRhapsody.NTestsRunner.Types
{
    public abstract class Verification
    {
        public string Result { get; private set; }
        public DateTime ExecutedAt { get; private set; }

        public Verification(params object[] args)
        {
            if (args != null && args.Length > 1)
            {
                Result = String.Format((string)args[0], args.Skip(1).ToArray());
            }
            else if (args != null && args.Length == 1)
            {
                Result = args[0].ToString();
            }
            else
            {
                Result = string.Empty;
            }
            ExecutedAt = DateTime.Now;
        }
    }
}
