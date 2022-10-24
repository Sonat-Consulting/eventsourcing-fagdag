using Clippers.Core.Haircut.Events;

namespace Clippers.Projections.Projections
{
    public class NumOfHaircutsCreated
    {
        public int Count { get; set; } = 0;
    }
    public class NumOfHaircutsCreatedProjection : Projection<NumOfHaircutsCreated>
    {
        public NumOfHaircutsCreatedProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
        }

        private void WhenHaircutCreated(HaircutCreated haircutCreated, NumOfHaircutsCreated view)
        {
            view.Count++;
        }
    }
}
