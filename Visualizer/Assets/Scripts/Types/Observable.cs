using System;

namespace Assets.Scripts.Types
{
    public class Observable<T> : Reference<T>
    {
        private Action<T, T> _subscriber;

        public Observable(T value) : base(value)
        {
        }

        public void OnValueChanged(Action<T, T> subscriber)
        {
            this._subscriber = subscriber;
        }

        protected override void OnValueUpdated(T oldValue, T newValue)
        {
            this._subscriber?.Invoke(oldValue, newValue);
        }
    }
}
