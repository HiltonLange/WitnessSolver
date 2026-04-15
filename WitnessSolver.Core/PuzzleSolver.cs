namespace WitnessSolver
{
    public class PuzzleSolver
    {
        private readonly SolverGraph _graph;

        public PuzzleSolver(SolverGraph graph)
        {
            this._graph = graph;
        }

        public int Solve()
        {
            this.Search();
            this._graph.SendUpdate(true);
            return this._graph.Solutions.TotalFound;
        }

        private void Search()
        {
            if (this._graph.CheckSolved())
                return;

            var edges = this._graph.PossibleOutEdges();
            foreach (var edge in edges)
            {
                if (this._graph.AddEdge(edge))
                    this.Search();

                this._graph.RemoveEdge(edge);
            }
        }
    }
}

