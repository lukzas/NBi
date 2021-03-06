﻿using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    class AndCombinationPredication : BaseCombinationPredication
    {
        public override string Description { get => "and"; }

        public AndCombinationPredication(IEnumerable<IPredication> predications)
            : base(predications)
        { }

        protected override bool ContinueCondition(bool state)
            => state;
        
        protected override bool StartState()
            => true;
    }
}
