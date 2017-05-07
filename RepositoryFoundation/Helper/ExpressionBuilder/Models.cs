using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelperFoundation.ExpressionBuilder
{
    public class WhereClauseFilter
    {
        public string PropertyName { get; set; }
        public Operations Operation { get; set; }
        public object Value { get; set; }
    }

    public class WhereClauseGroupFilter
    {
        public string Group { get; set; }
        public List<WhereClauseFilter> Filters { get; set; }
        public WhereFilterTypes Operation { get; set; }
    }

    

    public enum Operations
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }

    public enum WhereFilterTypes
    {
        AndAlso,
        OrElse
    }
}
