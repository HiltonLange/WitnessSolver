using System;

namespace WitnessSolver
{
    public enum PuzzleCategory
    {
        Trivial  = 10,  // 0-1ms
        Short    = 20,  // 2-50ms
        Medium   = 30,  // 51-5000ms
        Long     = 40,  // 5s-100s
        VeryLong = 50,  // 100s+
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PuzzleDefinitionAttribute : Attribute
    {
        public PuzzleCategory Category { get; }

        public PuzzleDefinitionAttribute(PuzzleCategory category = PuzzleCategory.Medium)
        {
            this.Category = category;
        }
    }
}
