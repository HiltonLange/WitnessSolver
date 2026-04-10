using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WitnessSolver
{
    public class PuzzleEntry
    {
        public string Name { get; }
        public PuzzleCategory Category { get; }
        public long ExpectedSolutions { get; }
        public Func<Puzzle> Factory { get; }

        public PuzzleEntry(string name, PuzzleCategory category, long expectedSolutions, Func<Puzzle> factory)
        {
            this.Name = name;
            this.Category = category;
            this.ExpectedSolutions = expectedSolutions;
            this.Factory = factory;
        }

        public override string ToString() => this.Name;
    }

    public static class PuzzleLibrary
    {
        private static List<PuzzleEntry> _entries;
        private static readonly List<System.Reflection.Assembly> _assemblies = new List<System.Reflection.Assembly>();

        public static void RegisterAssembly(System.Reflection.Assembly assembly)
        {
            _assemblies.Add(assembly);
            _entries = null;
        }

        public static IReadOnlyList<PuzzleEntry> All => _entries ??= Discover();

        public static IEnumerable<PuzzleEntry> Below(PuzzleCategory category)
            => All.Where(e => e.Category < category);

        public static IEnumerable<PuzzleEntry> AtLeast(PuzzleCategory category)
            => All.Where(e => e.Category >= category);

        public static IEnumerable<PuzzleEntry> Between(PuzzleCategory min, PuzzleCategory max)
            => All.Where(e => e.Category >= min && e.Category <= max);

        public static IEnumerable<PuzzleEntry> AtMost(PuzzleCategory category)
            => All.Where(e => e.Category <= category);

        private static List<PuzzleEntry> Discover()
        {
            var entries = new List<PuzzleEntry>();
            var assemblies = _assemblies.Count > 0
                ? _assemblies
                : new List<System.Reflection.Assembly>(AppDomain.CurrentDomain.GetAssemblies());

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    {
                        var attr = method.GetCustomAttribute<PuzzleDefinitionAttribute>();
                        if (attr == null) continue;
                        if (method.ReturnType != typeof(Puzzle)) continue;
                        if (method.GetParameters().Length != 0) continue;

                        var factory = (Func<Puzzle>)Delegate.CreateDelegate(typeof(Func<Puzzle>), method);
                        var puzzle = factory();
                        entries.Add(new PuzzleEntry(method.Name, attr.Category, puzzle.ExpectedSolutions, factory));
                    }
                }
            }

            return entries
                .OrderBy(e => (int)e.Category)
                .ThenBy(e => e.ExpectedSolutions)
                .ToList();
        }
    }
}
