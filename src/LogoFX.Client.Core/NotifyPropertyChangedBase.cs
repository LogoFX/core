//note: I don't know who is responsible for writing out the most of this excellent stuff
//note: If you feel you are somehow involved and not mentioned in credits - let me know
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using LogoFX.Core;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// A base class for classes that need to implement <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <typeparam name="TObject">The type of the derived class.</typeparam>
    public abstract class NotifyPropertyChangedBase<TObject> : INotifyPropertyChanged, ISuppressNotify
        where TObject : NotifyPropertyChangedBase<TObject>
    {
        /// <summary>
        /// The backing delegate for <see cref="PropertyChanged"/>.
        /// </summary>
        private PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => _propertyChanged += value;
            remove => _propertyChanged -= value;
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the Items[] property.
        /// </summary>
        protected void OnItemsPropertyChanged()
        {
            InvokeViaDispatcher(() => _propertyChanged.RaiseItems(this));
        }

        /// <summary>
        /// Notifies the of property change.(GLUE: compatibility to caliburn micro)
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> expression)
        {
            OnPropertyChanged(expression);
        }

        /// <summary>
        /// Notifies the of property change.
        /// </summary>        
        /// <param name="propInfo">The expression.</param>
        protected void NotifyOfPropertyChange(PropertyInfo propInfo)
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            InvokeViaDispatcher(() => _propertyChanged.Raise((TObject) this, propInfo));
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            InvokeViaDispatcher(() => _propertyChanged.Raise((TObject) this, expression));
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <param name="name"></param>
        protected void NotifyOfPropertyChange([CallerMemberName] string name = "")
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            InvokeViaDispatcher(() => _propertyChanged.Raise((TObject) this, name));
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            _propertyChanged.Raise((TObject) this, expression);
        }

        /// <summary>
        /// Notifies of all properties change.
        /// </summary>
        protected void NotifyOfPropertiesChange()
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            InvokeViaDispatcher(() => _propertyChanged.Raise((TObject) this));
        }

        /// <summary>
        /// Returns per-object dispatch.
        /// Override to inject custom implementation of <see cref="IDispatch"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual IDispatch GetDispatch() => null;

        private void InvokeViaDispatcher(Action action)
        {
            if (!_notifyManager.IsMuted)
            {
                var dispatch = GetDispatchImpl();
                if (dispatch != null)
                {
                    dispatch.OnUiThread(action);
                }
                else
                {
                    action();
                }
            }
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
        protected void SetProperty<TProperty>(
            ref TProperty currentValue,
            TProperty newValue,
            SetPropertyOptions options = null,
            [CallerMemberName] string name = "")
        {
            if (Equals(currentValue, newValue))
            {
                return;
            }

            if (options?.CustomActionInvocation != null)
            {
                options.CustomActionInvocation(() =>
                {
                    options?.BeforeValueUpdate?.Invoke();
                });
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
                var dispatch = GetDispatchImpl();

                if (dispatch != null)
                {
                    dispatch.OnUiThread(() => { options?.BeforeValueUpdate?.Invoke(); });
                    currentValue = newValue;
                    NotifyOfPropertyChange(name);
                    dispatch.OnUiThread(() =>
                    {
                        options?.AfterValueUpdate?.Invoke();
                    });
                }
                else
                {
                    options?.BeforeValueUpdate?.Invoke();
                    currentValue = newValue;
                    NotifyOfPropertyChange(name);
                    options?.AfterValueUpdate?.Invoke();
                }
            }
        }

        private IDispatch GetDispatchImpl() => GetDispatch() ?? Dispatch.Current;

        private readonly INotifyManager _notifyManager = new NotifyManager();

        IDisposable ISuppressNotify.SuppressNotify => SuppressNotify;

        /// <summary>
        /// Gets the suppress notify.
        /// To be used in <c>using</c> statement.
        /// </summary>
        protected IDisposable SuppressNotify => new SuppressNotifyHelper(_notifyManager);
    }
}
