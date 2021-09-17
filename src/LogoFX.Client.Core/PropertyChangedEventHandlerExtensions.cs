//note: I don't know who is responsible for writing out the most of this excellent stuff
//note: If you feel you are somehow involved and not mentioned in credits - let me know
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Provides extension methods for <see cref="PropertyChangedEventHandler"/> delegates.
    /// </summary>
    public static class PropertyChangedEventHandlerExtensions
    {
        /// <summary>
        /// Subscribes a handler to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a specific property.
        /// </summary>
        /// <typeparam name="TObject">The type implementing <see cref="INotifyPropertyChanged"/>.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The object implementing <see cref="INotifyPropertyChanged"/>.</param>
        /// <param name="expression">The lambda expression selecting the property.</param>
        /// <param name="handler">The handler that is invoked when the property changes.</param>
        /// <returns>The actual delegate subscribed to <see cref="INotifyPropertyChanged.PropertyChanged"/>.</returns>
        public static PropertyChangedEventHandler SubscribeToPropertyChanged<TObject, TProperty>(
            this TObject source,
            Expression<Func<TObject, TProperty>> expression,
            Action<TObject> handler)
            where TObject : INotifyPropertyChanged
        {
            // This is similar but not identical to:
            //   http://www.ingebrigtsen.info/post/2008/12/11/INotifyPropertyChanged-revisited.aspx
            string propertyName = source.GetPropertyName(expression);
            PropertyChangedEventHandler ret = (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    handler(source);
                }
            };
            source.PropertyChanged += ret;
            return ret;
        }

        /// <summary>
        /// Notifies the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="propertySelector">The property selector.</param>
        public static void Notify<T>(this PropertyChangedEventHandler handler, Expression<Func<T>> propertySelector)
        {
            if (handler != null)
            {
                var memberExpression = propertySelector.GetMemberExpression();
                if (memberExpression != null)
                {
                    var sender = ((ConstantExpression)memberExpression.Expression).Value;
                    handler(sender, new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
        }

        /// <summary>
        /// Raises the delegate for the property identified by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public static void Raise<TObject, TProperty>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            Expression<Func<TObject, TProperty>> expression)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(sender.GetPropertyName(expression)));
        }

        /// <summary>
        /// Raises the delegate for the property identified by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public static void Raise<TObject, TProperty>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            Expression<Func<TProperty>> expression)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(sender.GetPropertyName(expression)));
        }

        /// <summary>
        /// Raises the delegate for the property identified by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>        
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        /// <param name="name">The lambda expression identifying the property that changed.</param>
        public static void Raise<TObject>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            string name)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="info"></param>
        public static void Raise<TObject>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            PropertyInfo info)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(info.Name));
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        public static void Raise<TObject>(
            this PropertyChangedEventHandler handler,
            TObject sender)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Raises the delegate for the items property (with the name "Items[]").
        /// </summary>
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        public static void RaiseItems(this PropertyChangedEventHandler handler, object sender)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs("Items[]"));
        }
    }
}
