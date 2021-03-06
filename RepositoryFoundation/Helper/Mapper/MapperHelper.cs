﻿using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryFoundation.Helper.ExpressionBuilder;
using System.Linq.Expressions;
using System.Collections;
using AutoMapper.QueryableExtensions;

namespace RepositoryFoundation.Helper.Mapper
{
    public static class MapperHelper
    {  
        /// <summary>
        /// Copies all public properties from one class to another.
        /// </summary>
        /// <typeparam name="T">The Type of object to copy</typeparam>
        /// <param name="source">The source object to copy</param>
        /// <returns></returns>
        public static T CreateDeepCopy<T>(this T source) where T : class
        {
            if ((source == null)) return null;
            
            var propertyInfos = source.GetType().GetProperties();
            if (!propertyInfos.Any()) return null;

            var expressions = new List<Expression>();
            var newObjectExpression = source.GetType().Construct();
            var newObjParamExpression = QueryBuilder.CreateObject<T>("newObjParam");
            expressions.Add(newObjParamExpression.Assign(newObjectExpression));
            var paramExpression = QueryBuilder.CreateObject<T>();
            foreach (var propInfo in propertyInfos.Where(x => x.CanWrite))
            {
                var value = propInfo.GetValue(source, null);
                if (value == null) continue;
                
                var valType = value.GetType();

                if (valType.IsReferenceType() && !valType.IsString())
                {
                    expressions.Add(QueryBuilder.GetProperty(propInfo.Name, newObjParamExpression).Assign(value.Call(nameof(CreateDeepCopy), typeof(MapperHelper), source)));
                }
                else
                {
                    expressions.Add(QueryBuilder.GetProperty(propInfo.Name, newObjParamExpression).Assign(value));
                }                
            }

            if(!expressions.Any())
            {
                return null;
            }
            expressions.Add(newObjParamExpression);
            var blockExpressions = Expression.Block(new[] { newObjParamExpression }, expressions);
            var lambda = blockExpressions.Compile<Func<T,T>>(paramExpression);
            return lambda(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            return AutoMapper.Mapper.Map<TDestination>(source);
        }

        public static IEnumerable<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            return source.MapTo<IEnumerable<TSource>, IEnumerable<TDestination>>();
        }

        public static IEnumerable<TDestination> MapTo<TDestination>(this IEnumerable source)
        {
            return source.MapTo<TDestination>();
        }

        public static List<TDestination> MapTo<TSource, TDestination>(this IList<TSource> source)
        {
            return source.MapTo<IList<TSource>, List<TDestination>>();
        }

        public static List<TDestination> MapTo<TDestination>(this IList source)
        {
            return source.MapTo<TDestination>();
        }

        public static IQueryable<TDestination> MapTo<TSource, TDestination>(this IQueryable<TSource> source)
        {
            return source.MapTo<TDestination>();
        }

        public static IQueryable<TDestination> MapTo<TDestination>(this IQueryable source)
        {
            return source.ProjectTo<TDestination>();
        }

        private static bool IsReferenceType(this Type type)
        {
            return type.IsClass && !type.IsValueType && !type.IsPrimitive;
        }

        private static bool IsStruct(this Type type)
        {
            return type.IsValueType && !type.IsEnum;
        }

        private static bool IsString(this Type type)
        {
            return type == typeof(String);
        }
    }
}
