using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelperFoundation.ExpressionBuilder
{
    public static class Builder
    {

        private static readonly MethodInfo containsMethod;
        private static readonly MethodInfo startsWithMethod;
        private static readonly MethodInfo endsWithMethod;

        static Builder()
        {
            containsMethod = typeof(string).GetMethod("Contains");
            startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        }
        public static Expression<Func<TItem, object>> GetSelectOrGroupByExpression<TItem>(this string[] propertyNames)
        {
            var properties = propertyNames.Select(name => typeof(TItem).GetProperty(name)).ToArray();
            var propertyTypes = properties.Select(p => p.PropertyType).ToArray();
            var tupleTypeDefinition = typeof(Tuple).Assembly.GetType("System.Tuple`" + properties.Length);
            var tupleType = tupleTypeDefinition.MakeGenericType(propertyTypes);
            var constructor = tupleType.GetConstructor(propertyTypes);
            var param = Expression.Parameter(typeof(TItem), "item");
            var body = Expression.New(constructor, properties.Select(p => Expression.Property(param, p)));
            var expr = Expression.Lambda<Func<TItem, object>>(body, param);
            return expr;
        }

        public static Expression<Func<TItem, bool>> GetWhereClauseExpression<TItem>(this List<WhereClauseFilter> filters, WhereFilterTypes filterType)
        {
            var parameterExpression = Expression.Parameter(typeof(TItem), "tParam");

            Expression expr = null;

            filters.ForEach(filter =>
            {
                if (expr == null)
                {
                    expr = parameterExpression.GetWhereFilterExpression(filter);
                }
                else
                {
                    switch (filterType)
                    {
                        case WhereFilterTypes.AndAlso:
                            expr = Expression.AndAlso(expr, parameterExpression.GetWhereFilterExpression(filter));
                            break;
                        case WhereFilterTypes.OrElse:
                            expr = Expression.OrElse(expr, parameterExpression.GetWhereFilterExpression(filter));
                            break;
                    }
                }
            });

            return Expression.Lambda<Func<TItem, bool>>(expr, parameterExpression);
        }

        public static Expression<Func<TItem, bool>> GetWhereClauseExpression<TItem>(this List<WhereClauseGroupFilter> filters, WhereFilterTypes filterType)
        {
            var parameterExpression = Expression.Parameter(typeof(TItem), "tParam");

            Expression expr = null;

            filters.ForEach(filter =>
            {
                if (expr == null)
                {
                    expr = parameterExpression.GetWhereFilterGroupExpression(filter);
                }
                else
                {
                    switch (filterType)
                    {
                        case WhereFilterTypes.AndAlso:
                            expr = Expression.AndAlso(expr, parameterExpression.GetWhereFilterGroupExpression(filter));
                            break;
                        case WhereFilterTypes.OrElse:
                            expr = Expression.OrElse(expr, parameterExpression.GetWhereFilterGroupExpression(filter));
                            break;
                    }
                }
            });

            return Expression.Lambda<Func<TItem, bool>>(expr, parameterExpression);
        }

        public static Expression<Func<TItem, bool>> GetWhereClauseExpression<TItem>(this List<WhereClauseGroupFilter> filterGroupList, List<WhereClauseFilter> filters, WhereFilterTypes filterGroupCombineType, WhereFilterTypes filterCombineType, WhereFilterTypes filterType)
        {
            Expression groupFilterExpression = null;
            if (filterGroupList.Any())
            {
                groupFilterExpression = filterGroupList.GetWhereClauseExpression<TItem>(filterGroupCombineType);
            }
            Expression filterExpression = null;
            if (filters.Any())
            {
                filterExpression = filters.GetWhereClauseExpression<TItem>(filterGroupCombineType);
            }
            Expression exp = null;
            if (groupFilterExpression != null && filterExpression != null)
            {
                switch (filterType)
                {
                    case WhereFilterTypes.AndAlso:
                        exp = Expression.AndAlso(groupFilterExpression, filterExpression);
                        break;
                    case WhereFilterTypes.OrElse:
                        exp = Expression.OrElse(groupFilterExpression, filterExpression);
                        break;
                }
            }
            else if (groupFilterExpression == null && filterExpression != null)
            {
                exp = filterExpression;
            }
            else if (groupFilterExpression != null)
            {
                exp = groupFilterExpression;
            }
            return (Expression<Func<TItem, bool>>)exp;
        }

        private static Expression GetWhereFilterExpression(this ParameterExpression param, WhereClauseFilter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            var genericArguments = member.Type.GetGenericArguments();
            Expression constant;
            if (genericArguments.Length > 0)
            {
                ConstantExpression tempConstant = Expression.Constant(Convert.ChangeType(filter.Value, genericArguments[0]));
                constant = Expression.Convert(tempConstant, member.Type);
            }
            else
                constant = Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case Operations.Equals:
                    return Expression.Equal(member, constant);

                case Operations.NotEquals:
                    return Expression.NotEqual(member, constant);

                case Operations.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Operations.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Operations.LessThan:
                    return Expression.LessThan(member, constant);

                case Operations.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Operations.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Operations.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Operations.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static Expression GetWhereFilterGroupExpression(this ParameterExpression param, WhereClauseGroupFilter filterGroup)
        {
            var filters = filterGroup.Filters;
            var whereExpression = new List<Expression>();

            filters.ForEach(filter =>
            {
                whereExpression.Add(param.GetWhereFilterExpression(filter));
            });
            var totalExpressions = whereExpression.Count;
            Expression exp = null;
            for (var index = 0; index < totalExpressions; index++)
            {
                if (exp == null)
                {
                    exp = whereExpression[index];
                    continue;
                }
                switch (filterGroup.Operation)
                {
                    case WhereFilterTypes.AndAlso:
                        exp = Expression.AndAlso(exp, whereExpression[index]);
                        break;
                    case WhereFilterTypes.OrElse:
                        exp = Expression.OrElse(exp, whereExpression[index]);
                        break;
                }
            }

            return exp;
        }
    }
}
