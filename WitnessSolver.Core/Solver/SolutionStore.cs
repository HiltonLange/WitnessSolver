using System.Collections.Generic;

namespace WitnessSolver
{
    public class SolutionStore
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

        public void Clear()
        {
            lock (_lock)
            {
                _solutions.Clear();
                TotalFound = 0;
            }
        }
    }
}
