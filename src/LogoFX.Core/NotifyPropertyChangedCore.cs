//note: I don't know who is responsible for writing out the most of this excellent stuff
//note: If you feel you are somehow involved and not mentioned in credits - let me know
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LogoFX.Core
{
    /// <summary>
    /// Implements <see cref="INotifyPropertyChanged"/> on behalf of a container class.
    /// </summary>
    /// <typeparam name="T">The type of the containing class.</typeparam>
    public sealed class NotifyPropertyChangedCore<T> : INotifyPropertyChanged, ISuppressNotify
    {
        /// <summary>
        /// The backing delegate for <see cref="PropertyChanged"/>.
        /// </summary>
        private PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// The object that contains this instance.
        /// </summary>
        private readonly T _obj;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedCore{T}"/> class that is contained by <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object that contains this instance.</param>
        public NotifyPropertyChangedCore(T obj)
        {
            _obj = obj;
        }

        /// <summary>
        /// Provides notification of changes to a property value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }

            remove
            {
                _propertyChanged -= value;
            }
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public void OnPropertyChanged<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            InvokeAction(() => _propertyChanged.Raise(_obj, expression));
        }
        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            InvokeAction(() => _propertyChanged.Raise(_obj, expression));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            InvokeAction(() => _propertyChanged.Raise(this, name));
        }

        /// <summary>
        /// Compares the current and new values. If they are different,
        /// updates the respective field 
        /// and fires the property change notification.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="currentValue">The current value field reference.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="name">The property name.</param>
        /// <param name="options">The set property options.</param>        
        public void SetProperty<TProperty>(
            ref TProperty currentValue,
            TProperty newValue,
            SetPropertyOptions options = null,
            [CallerMemberName] string name = "")
        {
            //Can't use delegate here because of ref parameter
            if (!_notifyManager.IsMuted)
            {
                SetPropertyImpl(ref currentValue, newValue, options, name);
            }
        }

        private void SetPropertyImpl<TProperty>(ref TProperty currentValue, TProperty newValue, SetPropertyOptions options,
            string name)
        {
            if (Equals(currentValue, newValue))
            {
                return;
            }

            if (options?.CustomActionInvocation != null)
            {
                options.CustomActionInvocation(() => { options?.BeforeValueUpdate?.Invoke(); });
                currentValue = newValue;
                options.CustomActionInvocation(() =>
                {
                    if (!_notifyManager.IsMuted)
                    {
                        _propertyChanged.Raise(this, name);
                    }

                    options?.AfterValueUpdate?.Invoke();
                });
            }
            else
            {
                options?.BeforeValueUpdate?.Invoke();
                currentValue = newValue;
                OnPropertyChanged(name);
                options?.AfterValueUpdate?.Invoke();
            }
        }

        private readonly INotifyManager _notifyManager = new NotifyManager();

        IDisposable ISuppressNotify.SuppressNotify => SuppressNotify;

        /// <summary>
        /// Gets the suppress notify.
        /// To be used in <c>using</c> statement.
        /// </summary>
        public IDisposable SuppressNotify => new SuppressNotifyHelper(_notifyManager);

        private void InvokeAction(Action action)
        {
            if (!_notifyManager.IsMuted)
            {
                action();
            }
        }
    }
}
