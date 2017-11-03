﻿using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Loading;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework;

namespace NBi.NUnit.Query
{
    public class RowCountFilterConstraint : RowCountConstraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IResultSetFilter filter = ResultSetFilter.None;
        protected ResultSet filterResultSet;
        protected Func<ResultSet, ResultSet> filterFunction;

        public RowCountFilterConstraint(NUnitCtr.Constraint childConstraint, IResultSetFilter filter)
            : base(childConstraint)
        {
            this.filter = filter;
            filterFunction = filter.Apply;
        }

        public IResultSetFilter Filter
        {
            get
            {
                return filter;
            }
        }

        protected override IDataRowsMessageFormatter BuildFailure()
        {
            var factory = new DataRowsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, EngineStyle.ByIndex);
            msg.BuildFilter(actualResultSet.Rows.Cast<DataRow>(), filterResultSet.Rows.Cast<DataRow>());
            return msg;
        }

        /// <summary>
        /// Handle an IDbCommand and compare its row-count to a another value
        /// </summary>
        /// <param name="actual">An OleDbCommand, SqlCommand or AdomdCommand</param>
        /// <returns>true, if the row-count of query execution validates the child constraint</returns>
        public override bool Matches(object actual)
        {
            if (actual is IResultSetService)
                return Matches(((IResultSetService)actual).Execute());
            else if (actual is ResultSet)
            {
                actualResultSet = (ResultSet)actual;
                filterResultSet = filterFunction(actualResultSet);
                return Matches(filterResultSet.Rows.Count);
            }
            else if (actual is int)
                return doMatch(((int)actual));
            else
                throw new ArgumentException($"The type '{actual.GetType().Name}' is not supported by the constraint '{this.GetType().Name}'. Use a IResultSetService or a ResultSet.", nameof(actual));
        }


        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WritePredicate($"count of rows validating the predicate '{filter.Describe()}' is");
            child.WriteDescriptionTo(writer);
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(Failure.RenderMessage());
            else
            {
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine();
                WriteFilterMessageTo(writer);
                writer.WriteLine(Failure.RenderAnalysis());
            }
        }

        public virtual void WriteFilterMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WriteLine("Filtered version of the result-set:");
        }

    }
}