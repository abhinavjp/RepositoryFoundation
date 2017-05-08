//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using RepositoryFoundation.Helper.ExpressionBuilder;
//using System.Linq.Expressions;

//namespace RepositoryFoundation.Helper.Mapper
//{
//    public static class MapperHelper
//    {
//        /// <summary>
//        /// Copies all public properties from one class to another.
//        /// </summary>
//        /// <param name="source">The source.</param>
//        /// <param name="destination">The destination.</param>
//        /// <exception cref="System.Exception">Source and destination cannot be null and must be 
//        /// of the same type!</exception>
//        public static T CreateDeepCopy<T>(this T source) where T : class
//        {
//            // Source and destination must exist.
//            if ((source == null)) return null;

//            // Get properties
//            var propertyInfos = source.GetType().GetProperties();
//            if (!propertyInfos.Any()) return null;

//            List<Expression> expressions = new List<Expression>();
//            // Process only public properties
//            foreach (var propInfo in propertyInfos.Where(x => x.CanWrite))
//            {
//                // Get value from source and assign to destination.
//                var value = propInfo.GetValue(source, null);
//                if (value == null) continue;

//                // Evaluate
//                var valType = value.GetType();

//                if (valType.IsReferenceType())
//                {
//                    expressions.Add(QueryBuilder.GetProperty<T>(propInfo.Name).Assign(value.Call("CreateDeepCopy", typeof(MapperHelper), source)));
//                }
//                else
//                {
//                    var assignment = QueryBuilder.GetProperty<T>(propInfo.Name).Assign(value);
//                }
//                // Collections

//                var sourceCollection = value as IList;
//                if (sourceCollection == null) continue;

//                // Create new instance of collection
//                IList destinationCollection = null;
//                destinationCollection = (valType.BaseType == typeof(Array))
//                    ? Array.CreateInstance(valType.GetElementType(), sourceCollection.Count)
//                    : (IList)Activator.CreateInstance(valType, null);
//                if (destinationCollection == null) continue;

//                // Map properties
//                foreach (var item in sourceCollection)
//                {
//                    // Map properties
//                    var newItem = item.CreateDeepCopy();

//                    // Add to destination collection
//                    if (valType.BaseType == typeof(Array))
//                    {
//                        destinationCollection[sourceCollection.IndexOf(item)] = newItem;
//                    }
//                    else
//                    {
//                        destinationCollection.Add(newItem);
//                    }
//                }

//                // Add new collection to destination
//            }

//        }

//        /// <summary>
//        /// Determines whether the type has a default contructor.
//        /// </summary>
//        /// <param name="type">The type.</param>
//        /// <returns></returns>
//        private static bool HasDefaultConstructor(Type type)
//        {
//            return
//                type.GetConstructor(Type.EmptyTypes) != null ||
//                type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
//                    .Any(x => x.GetParameters().All(p => p.IsOptional));
//        }

//        private static bool IsReferenceType(this Type type)
//        {
//            return type.IsClass && !type.IsValueType && !type.IsPrimitive;
//        }

//        private static bool IsStruct(this Type type)
//        {
//            return type.IsValueType && !type.IsEnum;
//        }

//        private static bool IsString(this Type type)
//        {
//            return type == typeof(String);
//        }
//    }
//}
