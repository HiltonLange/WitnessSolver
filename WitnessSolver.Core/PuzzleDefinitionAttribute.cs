using System;

namespace WitnessSolver
{
    public enum PuzzleCategory
    {
        Normal,
        Long,
        VeryLong,
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PuzzleDefinitionAttribute : Attribute
    {
        public PuzzleCategory Category { get; }

        public PuzzleDefinitionAttribute(PuzzleCategory category = PuzzleCategory.Normal)
        {
            this.Category = category;
        }
    }
}
