using System.Collections.Generic;

namespace WitnessSolver
{
    public interface ISolutionStore
    {
        int TotalFound { get; }
        int StoredCount { get; }
        void Add(int[] routeEdgeIndices);
        int[] Get(int index);
    }

    public class SolutionStore : ISolutionStore
    {
        private readonly List<int[]> _solutions = new List<int[]>();
        private readonly object _lock = new object();

        public int TotalFound { get; private set; }
        public int StoredCount { get { lock (_lock) { return _solutions.Count; } } }
        public int MaxStored { get; set; } = 20_000_000;

        public void Add(int[] routeEdgeIndices)
        {
            TotalFound++;

            lock (_lock)
            {
                if (_solutions.Count < MaxStored)
                    _solutions.Add(routeEdgeIndices);
            }
        }

        public int[] Get(int index)
        {
            lock (_lock)
            {
                return index >= 0 && index < _solutions.Count ? _solutions[index] : null;
            }
        }
    }

    public class NullSolutionStore : ISolutionStore
    {
        public int TotalFound { get; private set; }
        public int StoredCount => 0;
        public void Add(int[] routeEdgeIndices) => TotalFound++;
        public int[] Get(int index) => null;
    }
}
