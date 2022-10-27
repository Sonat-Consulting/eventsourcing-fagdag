using Clippers.EventFlow.Projections.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class NumOfHaircutsCancelledView
    {
        public int NumOfHaircutsCancelled { get; set; } = 0;
    }
    public class NumOfHaircutsCancelledProjection : Projection<NumOfHaircutsCancelledView>
    {
        public NumOfHaircutsCancelledProjection()
        {
            RegisterHandler<HaircutCancelled>(WhenHaircutCancelled);
        }
        private void WhenHaircutCancelled(HaircutCancelled haircutCancelled, NumOfHaircutsCancelledView view)
        {
            view.NumOfHaircutsCancelled++;
        }
    }
}
