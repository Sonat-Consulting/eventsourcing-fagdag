using Clippers.EventFlow.Projections.Core.Events;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class NumOfHaircutsCreatedView
    {
        public int Count { get; set; } = 0;
    }
    public class NumOfHaircutsCreatedProjection : Projection<NumOfHaircutsCreatedView>
    {
        public NumOfHaircutsCreatedProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
        }

        private void WhenHaircutCreated(HaircutCreated haircutCreated, NumOfHaircutsCreatedView view)
        {
            view.Count++;
        }
    }
}
