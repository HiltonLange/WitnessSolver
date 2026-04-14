using System.Collections.Generic;

namespace WitnessSolver
{
    public class SolutionStore
    {
        private readonly List<int[]> _solutions = new List<int[]>();
        private readonly object _lock = new object();

        public int TotalFound { get; private set; }
        public int StoredCount { get { lock (_lock) { return _solutions.Count; } } }
        public int MaxStored { get; set; } = 10000;

        public void Add(int[] routeEdgeIndices)
        {
            TotalFound++;

            if (ShouldStore(TotalFound))
            {
                lock (_lock)
                {
                    if (_solutions.Count < MaxStored)
                        _solutions.Add(routeEdgeIndices);
                }
            }
        }

        public int[] Get(int index)
        {
            lock (_lock)
            {
                return index >= 0 && index < _solutions.Count ? _solutions[index] : null;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _solutions.Clear();
                TotalFound = 0;
            }
        }

        private static bool ShouldStore(int n)
        {
            if (n <= 1000) return true;
            if (n <= 10000) return n % 10 == 0;
            if (n <= 100000) return n % 100 == 0;
            return n % 1000 == 0;
        }
    }
}
