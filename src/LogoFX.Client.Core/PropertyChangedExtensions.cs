//note: I don't know who is responsible for writing out the most of this excellent stuff
//note: If you feel you are somehow involved and not mentioned in credits - let me know
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace LogoFX.Client.Core
{
    /// <remarks>
    /// - ideas from http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree
    /// </remarks>
    public static class PropertyChangedExtensions
    {
        private const string SELECTOR_MUSTBEPROP = "PropertySelector must select a property type.";

        /// <summary>
        /// Notifies the specified notifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="notifier">The notifier.</param>
        /// <param name="propertySelector">The property selector.</param>
        public static void Notify<T>(this Action<string> notifier, Expression<Func<T>> propertySelector)
        {
            if (notifier != null)
                notifier(GetPropertyName(propertySelector));
        }

        /// <summary>
        /// Retrieves the name of a property referenced by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The object containing the property.</param>
        /// <param name="expression">A lambda expression selecting the property from the containing object.</param>
        /// <returns>The name of the property referenced by <paramref name="expression"/>.</returns>
        public static string GetPropertyName<TObject, TProperty>(this TObject @this, Expression<Func<TObject, TProperty>> expression)
        {
            // For more information on the technique used here, see these blog posts:
            //   http://themechanicalbride.blogspot.com/2007/03/symbols-on-steroids-in-c.html
            //   http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree
            //   http://joshsmithonwpf.wordpress.com/2009/07/11/one-way-to-avoid-messy-propertychanged-event-handling/
            // Note that the following blog post:
            //   http://www.ingebrigtsen.info/post/2008/12/11/INotifyPropertyChanged-revisited.aspx
            // uses a similar technique, but must also account for implicit casts to object by checking for UnaryExpression.
            // Our solution uses generics, so this additional test is not necessary.
            return expression != null ? ((MemberExpression)expression.Body).Member.Name : ".";
        }
        /// <summary>
        /// Retrieves the name of a property referenced by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The object containing the property.</param>
        /// <param name="expression">A lambda expression selecting the property from the containing object.</param>
        /// <returns>The name of the property referenced by <paramref name="expression"/>.</returns>
        public static string GetPropertyName<TObject, TProperty>(this TObject @this, Expression<Func<TProperty>> expression)
        {
            // For more information on the technique used here, see these blog posts:
            //   http://themechanicalbride.blogspot.com/2007/03/symbols-on-steroids-in-c.html
            //   http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree
            //   http://joshsmithonwpf.wordpress.com/2009/07/11/one-way-to-avoid-messy-propertychanged-event-handling/
            // Note that the following blog post:
            //   http://www.ingebrigtsen.info/post/2008/12/11/INotifyPropertyChanged-revisited.aspx
            // uses a similar technique, but must also account for implicit casts to object by checking for UnaryExpression.
            // Our solution uses generics, so this additional test is not necessary.

            return expression != null ? ((MemberExpression)expression.Body).Member.Name : ".";
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <returns></returns>
        public static string GetPropertyName<T>(this Expression<Func<T>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = propertySelector.Body as UnaryExpression;
                if (unaryExpression != null) memberExpression = unaryExpression.Operand as MemberExpression;
            }
            if (memberExpression != null)
            {
                Debug.Assert(memberExpression.Member is PropertyInfo,
                    "propertySelector" + SELECTOR_MUSTBEPROP);
                return memberExpression.Member.Name;
            }
            return null;
        }

        /// <summary>
        /// Gets the member expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression<T>(this Expression<Func<T>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression != null)
            {
                Debug.Assert(memberExpression.Member is PropertyInfo,
                                   "propertySelector" + SELECTOR_MUSTBEPROP);
                return memberExpression;
            }

            // for WPF
            var unaryExpression = propertySelector.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var innerMemberExpression = unaryExpression.Operand as MemberExpression;
                if (innerMemberExpression != null)
                {
                    Debug.Assert(memberExpression.Member is PropertyInfo,
                                       "propertySelector" + SELECTOR_MUSTBEPROP);
                    return innerMemberExpression;
                }
            }

            // all else
            return null;
        }
    }
}
