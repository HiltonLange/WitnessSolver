using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WitnessSolver
{
    public static class PuzzleCatalog
    {
        private static readonly Lazy<IReadOnlyList<PuzzleEntry>> _entries = new Lazy<IReadOnlyList<PuzzleEntry>>(Discover);

        public static IReadOnlyList<PuzzleEntry> All => _entries.Value;

        public static IEnumerable<PuzzleEntry> Below(PuzzleCategory category)
            => All.Where(e => e.Category < category);

        public static IEnumerable<PuzzleEntry> AtLeast(PuzzleCategory category)
            => All.Where(e => e.Category >= category);

        public static IEnumerable<PuzzleEntry> Between(PuzzleCategory min, PuzzleCategory max)
            => All.Where(e => e.Category >= min && e.Category <= max);

        public static IEnumerable<PuzzleEntry> AtMost(PuzzleCategory category)
            => All.Where(e => e.Category <= category);

        private static IReadOnlyList<PuzzleEntry> Discover()
        {
            var entries = new List<PuzzleEntry>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    var attr = method.GetCustomAttribute<PuzzleDefinitionAttribute>();
                    if (attr == null) continue;
                    if (method.ReturnType != typeof(Puzzle)) continue;
                    if (method.GetParameters().Length != 0) continue;

                    var factory = (Func<Puzzle>)Delegate.CreateDelegate(typeof(Func<Puzzle>), method);
                    entries.Add(new PuzzleEntry(
                        method.Name,
                        attr.Category,
                        attr.ExpectedSolutions,
                        attr.HasExpectedSolutions,
                        factory));
                }
            }

            return entries
                .OrderBy(e => (int)e.Category)
                .ThenBy(e => e.ExpectedSolutions)
                .ToList()
                .AsReadOnly();
        }
    }
}
