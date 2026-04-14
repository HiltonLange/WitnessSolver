using System.Threading;

namespace WitnessSolver
{
    public class SolverSnapshot
    {
        public readonly int[] RouteEdgeIndices;
        public readonly long StepCount;
        public readonly int RoutesFound;
        public readonly int SolutionsFound;
        public readonly bool IsComplete;

        public SolverSnapshot(int[] routeEdgeIndices, long stepCount, int routesFound, int solutionsFound, bool isComplete)
        {
            this.RouteEdgeIndices = routeEdgeIndices;
            this.StepCount = stepCount;
            this.RoutesFound = routesFound;
            this.SolutionsFound = solutionsFound;
            this.IsComplete = isComplete;
        }

        // Lock-free shared snapshot — solver writes, UI reads
        private static SolverSnapshot _current;

        public static SolverSnapshot Current => Volatile.Read(ref _current);

        public static void Publish(SolverSnapshot snapshot)
        {
            Volatile.Write(ref _current, snapshot);
        }
    }
}
