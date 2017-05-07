﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RepositoryFoundation.Helper.ExpressionBuilder
{
    public static class QueryBuilder
    {
        static QueryBuilder()
        {

        }

        #region Compiling methods
        /// <summary>
        /// Get the final compiled lambda expression in a specified format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Compile<T>(Expression expression)
        {
            return Expression.Lambda<T>(expression).Compile();
        }

        /// <summary>
        /// Get the generated expression lambda in a specified format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<T> GetExpressionLambda<T>(Expression expression)
        {
            return Expression.Lambda<T>(expression);
        }

        public static Expression GetExpressionLambda(Expression expression)
        {
            return Expression.Lambda(expression);
        }
        #endregion

        #region Equality Check Expressions
        /// <summary>
        /// Add an equality check with a primitive value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression Equal<T>(this MemberExpression memberExpression, T value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.Equal(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add an equality check with a nullable primitive value.
        /// <para>If the value is null, primitive type's default value is considered</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression Equal<T>(this MemberExpression memberExpression, T? value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.Equal(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add an equality check with null value.
        /// </summary>
        public static BinaryExpression Equal(this MemberExpression memberExpression)
        {
            var constantExpression = GetConstant();
            return Expression.Equal(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add an equality check for a class member with a primitive value
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression Equal<TParameter, TValue>(string propertyName, TValue value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.Equal(value);
        }

        /// <summary>
        /// Add an equality check for a class member with a nullable primitive value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression Equal<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.Equal(value);
        }

        /// <summary>
        /// Add an equality check for a class member with null value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static BinaryExpression Equal<TParameter>(string propertyName)
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.Equal();
        }

        /// <summary>
        /// Add an inequality check with a primitive value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression NotEqual<T>(this MemberExpression memberExpression, T value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.NotEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add an inequality check with a nullable primitive value.
        /// <para>If the value is null, primitive type's default value is considered</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression NotEqual<T>(this MemberExpression memberExpression, T? value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.NotEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add an inequality check with null value.
        /// </summary>
        public static BinaryExpression NotEqual(this MemberExpression memberExpression)
        {
            var constantExpression = GetConstant();
            return Expression.NotEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add an inequality check for a class member with a primitive value
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression NotEqual<TParameter, TValue>(string propertyName, TValue value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.NotEqual(value);
        }

        /// <summary>
        /// Add an inequality check for a class member with a nullable primitive value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression NotEqual<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.NotEqual(value);
        }

        /// <summary>
        /// Add an inequality check for a class member with null value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static BinaryExpression NotEqual<TParameter>(string propertyName)
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.NotEqual();
        }

        /// <summary>
        /// Add a greater than condition check with a primitive value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression GreaterThan<T>(this MemberExpression memberExpression, T value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.GreaterThan(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a greater than condition check with a nullable primitive value.
        /// <para>If the value is null, primitive type's default value is considered</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression GreaterThan<T>(this MemberExpression memberExpression, T? value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.GreaterThan(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a greater than condition check for a class member with a primitive value
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression GreaterThan<TParameter, TValue>(string propertyName, TValue value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.GreaterThan(value);
        }

        /// <summary>
        /// Add a greater than condition check for a class member with a nullable primitive value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression GreaterThan<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.GreaterThan(value);
        }

        /// <summary>
        /// Add a greater than or equal to condition check with a primitive value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression GreaterThanOrEqual<T>(this MemberExpression memberExpression, T value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a greater than or equal to condition check with a nullable primitive value.
        /// <para>If the value is null, primitive type's default value is considered</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression GreaterThanOrEqual<T>(this MemberExpression memberExpression, T? value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a greater than or equal to condition check for a class member with a primitive value
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression GreaterThanOrEqual<TParameter, TValue>(string propertyName, TValue value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.GreaterThanOrEqual(value);
        }

        /// <summary>
        /// Add a greater than or equal to condition check for a class member with a nullable primitive value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression GreaterThanOrEqual<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.GreaterThanOrEqual(value);
        }

        /// <summary>
        /// Add a less than condition check with a primitive value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression LessThan<T>(this MemberExpression memberExpression, T value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.LessThan(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a less than condition check with a nullable primitive value.
        /// <para>If the value is null, primitive type's default value is considered</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression LessThan<T>(this MemberExpression memberExpression, T? value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.LessThan(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a less than condition check for a class member with a primitive value
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression LessThan<TParameter, TValue>(string propertyName, TValue value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.LessThan(value);
        }

        /// <summary>
        /// Add a less than condition check for a class member with a nullable primitive value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression LessThan<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.LessThan(value);
        }

        /// <summary>
        /// Add a less than or equal to condition check with a primitive value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression LessThanOrEqual<T>(this MemberExpression memberExpression, T value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.LessThanOrEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a less than or equal to condition check with a nullable primitive value.
        /// <para>If the value is null, primitive type's default value is considered</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static BinaryExpression LessThanOrEqual<T>(this MemberExpression memberExpression, T? value) where T : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return Expression.LessThanOrEqual(memberExpression, constantExpression);
        }

        /// <summary>
        /// Add a less than or equal to condition check for a class member with a primitive value
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression LessThanOrEqual<TParameter, TValue>(string propertyName, TValue value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return LessThanOrEqual(memberExpression, value);
        }

        /// <summary>
        /// Add a less than or equal to condition check for a class member with a nullable primitive value.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BinaryExpression LessThanOrEqual<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return LessThanOrEqual(memberExpression, value);
        }
        #endregion

        #region Object Expressions
        public static ParameterExpression CreateObject<T>()
        {
            return Expression.Parameter(typeof(T), "tParam");
        }

        public static MemberExpression GetProperty(string propertyName, ParameterExpression parameterExpression)
        {
            return Expression.Property(parameterExpression, propertyName);
        }

        public static MemberExpression GetProperty<T>(string propertyName)
        {
            return GetProperty(propertyName, CreateObject<T>());
        }

        public static ConstantExpression GetConstant<T>(T value)
        {
            return Expression.Constant(value);
        }

        public static ConstantExpression GetConstant<T>(T? value) where T : struct
        {
            if (value.HasValue)
                return GetConstant(value);
            else
                return GetConstant(default(T));
        }

        public static ConstantExpression GetConstant()
        {
            return Expression.Constant(null, typeof(object));
        }

        public static UnaryExpression GetConstant<TValue>(TValue value, MemberExpression memberExpression)
        {
            return Expression.Convert(GetConstant(value), memberExpression.Type);
        }

        public static UnaryExpression GetConstant<TValue>(TValue? value, MemberExpression memberExpression) where TValue : struct
        {
            return Expression.Convert(GetConstant(value), memberExpression.Type);
        }

        public static UnaryExpression ConvertConstant<TValue>(TValue value, Type type)
        {
            return Expression.Convert(GetConstant(value), type);
        }

        public static UnaryExpression ConvertConstant<TValue>(TValue? value, Type type) where TValue : struct
        {
            return Expression.Convert(GetConstant(value), type);
        }
        #endregion

        #region Connecting Expressions

        public static BinaryExpression AndAlso(this Expression leftExpression, Expression rightExpression)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression), "Null not allowed");
            return Expression.AndAlso(leftExpression, rightExpression);
        }

        public static BinaryExpression AndAlso(params Expression[] expressions)
        {
            if (expressions == null || expressions.Length <= 0)
                return null;
            Expression combiningExpression = null;
            foreach (var expression in expressions)
            {
                if (combiningExpression == null)
                {
                    combiningExpression = expression;
                }
                else
                {
                    combiningExpression = combiningExpression.AndAlso(expression);
                }
            }
            return combiningExpression as BinaryExpression;
        }

        public static BinaryExpression AndAlso(this Expression expression, params Expression[] expressions)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "Null not allowed");
            var combinedExpressions = AndAlso(expressions);
            return expression.AndAlso(combinedExpressions);
        }

        public static BinaryExpression OrElse(this Expression leftExpression, Expression rightExpression)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression), "Null not allowed");
            return Expression.OrElse(leftExpression, rightExpression);
        }

        public static BinaryExpression OrElse(params Expression[] expressions)
        {
            if (expressions == null || expressions.Length <= 0)
                return null;
            Expression combiningExpression = null;
            foreach (var expression in expressions)
            {
                if (combiningExpression == null)
                {
                    combiningExpression = expression;
                }
                else
                {
                    combiningExpression = combiningExpression.OrElse(expression);
                }
            }
            return combiningExpression as BinaryExpression;
        }

        public static BinaryExpression OrElse(this Expression leftExpression, params Expression[] rightExpressions)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            var combinedExpressions = OrElse(rightExpressions);
            return leftExpression.OrElse(combinedExpressions);
        }

        public static BlockExpression CreateBlock(this Expression leftExpression, Expression rightExpression)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression), "Null not allowed");
            return Expression.Block(leftExpression, rightExpression);
        }

        public static BlockExpression CreateBlock(params Expression[] expressions)
        {

            if (expressions == null || expressions.Length <= 0)
                return null;
            return Expression.Block(expressions);
        }

        public static BlockExpression CreateBlock(this Expression leftExpression, params Expression[] rightExpressions)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            if (rightExpressions == null)
                return CreateBlock(leftExpression);
            var expressionList = rightExpressions.ToList();
            expressionList.Add(leftExpression);
            return CreateBlock(expressionList.ToArray());
        }
        #endregion

        #region Calculative Expressions
        public static BinaryExpression Add(this Expression leftExpression, Expression rightExpression)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression), "Null not allowed");
            return Expression.Add(leftExpression, rightExpression);
        }
        public static BinaryExpression Add(params Expression[] expressions)
        {
            if (expressions == null || expressions.Length <= 0)
                return null;
            Expression addingExpression = null;
            foreach (var expression in expressions)
            {
                if (addingExpression == null)
                {
                    addingExpression = expression;
                }
                else
                {
                    addingExpression = addingExpression.Add(expression);
                }
            }
            return addingExpression as BinaryExpression;
        }
        public static BinaryExpression Add(this Expression leftExpression, params Expression[] rightExpressions)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            var addedExpressions = Add(rightExpressions);
            return leftExpression.Add(addedExpressions);
        }
        public static BinaryExpression Subtract(this Expression leftExpression, Expression rightExpression)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression), "Null not allowed");
            return Expression.Subtract(leftExpression, rightExpression);
        }
        public static BinaryExpression Subtract(params Expression[] expressions)
        {
            if (expressions == null || expressions.Length <= 0)
                return null;
            Expression subtractingExpression = null;
            foreach (var expression in expressions)
            {
                if (subtractingExpression == null)
                {
                    subtractingExpression = expression;
                }
                else
                {
                    subtractingExpression = subtractingExpression.Subtract(expression);
                }
            }
            return subtractingExpression as BinaryExpression;
        }
        public static BinaryExpression Subtract(this Expression leftExpression, params Expression[] rightExpressions)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression), "Null not allowed");
            var addedExpressions = Subtract(rightExpressions);
            return leftExpression.Subtract(addedExpressions);
        }
        #endregion

        #region Method Calling Expressions
        public static MethodCallExpression Contains<TParameter, TValue>(string propertyName, TValue value)
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.Contains(value);
        }

        public static MethodCallExpression Contains<TParameter, TValue>(string propertyName, TValue? value) where TValue : struct
        {
            var memberExpression = GetProperty<TParameter>(propertyName);
            return memberExpression.Contains(value);
        }

        public static MethodCallExpression Contains(this MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            var containsMethod = memberExpression.Type.GetMethod("Contains");
            return Expression.Call(memberExpression, containsMethod, constantExpression);
        }

        public static MethodCallExpression Contains(this MemberExpression memberExpression, UnaryExpression constantExpression)
        {
            var containsMethod = memberExpression.Type.GetMethod("Contains");
            return Expression.Call(memberExpression, containsMethod, constantExpression);
        }

        public static MethodCallExpression Contains<TValue>(this MemberExpression memberExpression, TValue value)
        {
            var constantExpression = GetConstant(value, memberExpression);
            return memberExpression.Contains(constantExpression);
        }

        public static MethodCallExpression Contains<TValue>(this MemberExpression memberExpression, TValue? value) where TValue: struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return memberExpression.Contains(constantExpression);
        }

        public static MethodCallExpression StartsWith(this MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            var startsWithMethod = memberExpression.Type.GetMethod("StartsWith");
            return Expression.Call(memberExpression, startsWithMethod, constantExpression);
        }

        public static MethodCallExpression StartsWith(this MemberExpression memberExpression, UnaryExpression constantExpression)
        {
            var startsWithMethod = memberExpression.Type.GetMethod("StartsWith");
            return Expression.Call(memberExpression, startsWithMethod, constantExpression);
        }

        public static MethodCallExpression StartsWith<TValue>(this MemberExpression memberExpression, TValue value)
        {
            var constantExpression = GetConstant(value, memberExpression);
            return memberExpression.StartsWith(constantExpression);
        }

        public static MethodCallExpression StartsWith<TValue>(this MemberExpression memberExpression, TValue? value) where TValue : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return memberExpression.StartsWith(constantExpression);
        }

        public static MethodCallExpression EndsWith(this MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            var endsWithMethod = memberExpression.Type.GetMethod("EndsWith");
            return Expression.Call(memberExpression, endsWithMethod, constantExpression);
        }

        public static MethodCallExpression EndsWith(this MemberExpression memberExpression, UnaryExpression constantExpression)
        {
            var endsWithMethod = memberExpression.Type.GetMethod("EndsWith");
            return Expression.Call(memberExpression, endsWithMethod, constantExpression);
        }

        public static MethodCallExpression EndsWith<TValue>(this MemberExpression memberExpression, TValue value)
        {
            var constantExpression = GetConstant(value, memberExpression);
            return memberExpression.EndsWith(constantExpression);
        }

        public static MethodCallExpression EndsWith<TValue>(this MemberExpression memberExpression, TValue? value) where TValue : struct
        {
            var constantExpression = GetConstant(value, memberExpression);
            return memberExpression.EndsWith(constantExpression);
        }
        #endregion

        #region Get Lambdas For Linq

        /// <summary>
        /// Get conditional lambda expreesion
        /// <para>Used to get lambda for Linq methods like, but not limited to </para>
        /// <para>Where, First, FirstOrDefault, Count, All, Any, Last, LastOrDefault, Single, SingleOrDefault, SkipWhile or TakeWhile</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, bool> GetConditionalLambda<T>(Expression expression)
        {
            return Compile<Func<T, bool>>(expression);
        }

        /// <summary>
        /// Get a selective lambda expression
        /// </summary>
        /// <para>Used to get lambda for Linq methods like, but not limited to </para>
        /// <para>Select, Max, Min and GroupBy</para>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResultOrKey"></typeparam>
        /// <returns></returns>
        public static Func<TSource, TResultOrKey> GetSelectiveLambda<TSource, TResultOrKey>(Expression expression)
        {
            return Compile<Func<TSource, TResultOrKey>>(expression);
        }
        #endregion
    }
}
