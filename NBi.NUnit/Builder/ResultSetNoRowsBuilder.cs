﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;

namespace NBi.NUnit.Builder
{
    class ResultSetNoRowsBuilder : AbstractResultSetBuilder
    {
        protected NoRowsXml ConstraintXml {get; set;}

        public ResultSetNoRowsBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is NoRowsXml))
                throw new ArgumentException("Constraint must be a 'RowCountXml'");

            ConstraintXml = (NoRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint()
        {           
            var expressions = new List<IColumnExpression>();
            if (ConstraintXml.Expression!=null)
                expressions.Add(ConstraintXml.Expression);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                        (
                            ConstraintXml.Aliases
                            , expressions
                            , ConstraintXml.Predicate
                        );

            var ctr = new NoRowsConstraint(filter);
            return ctr;
        }

    }
}