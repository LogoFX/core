using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogoFX.Core
{
    /// <summary>
    /// Helper class for enum operations.
    /// </summary>
    public static class EnumHelper
    {
        private static readonly IDictionary<Type, object[]> EnumCache = new Dictionary<Type, object[]>();

        /// <summary>
        /// Gets the boxed enum value.
        /// </summary>
        /// <param name="s">The unboxed enum value.</param>
        /// <returns></returns>
        public static object GetBoxed(Enum s)
        {
            var enumType = s.GetType();
            var ret = GetValues(enumType).FirstOrDefault(ss => ss.ToString() == s.ToString());
            return ret;
        }

        /// <summary>
        /// Gets all enum values from the specified enum type.
        /// </summary>
        /// <typeparam name="T">The specified enum type.</typeparam>
        /// <returns></returns>
        public static T[] GetValues<T>()
        {
            return GetValues(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Gets the unboxed enum values from the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Type ' + enumType.Name + ' is not an enum</exception>
        public static object[] GetValues(Type enumType)
        {
            if (enumType.GetTypeInfo().IsEnum == false)
            {
                throw new ArgumentException($"Type '{enumType.Name}' is not an enum");
            }

            if (EnumCache.TryGetValue(enumType, out var values) == false)
            {
                values = (from field in enumType.GetTypeInfo().DeclaredFields
                    where field.IsLiteral
                    select field.GetValue(enumType)).ToArray();
                EnumCache[enumType] = values;
            }
            return values;
        }
    }
}