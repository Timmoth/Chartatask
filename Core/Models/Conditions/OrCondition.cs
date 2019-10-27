﻿using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Interactors;

namespace ChartATask.Core.Models.Conditions
{
    public class OrCondition : ICondition
    {
        private readonly List<ICondition> _conditions;

        public OrCondition(List<ICondition> conditions)
        {
            _conditions = conditions;
        }

        public bool Check(ISystemEvaluator evaluator)
        {
            return _conditions.Any(condition => condition.Check(evaluator));
        }

        public override string ToString()
        {
            return $@"OR {{{string.Join(" || ", _conditions)}}}";
        }
    }
}