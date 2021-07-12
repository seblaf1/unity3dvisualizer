namespace Assets.Scripts.Types
{
    /// <summary>
    /// This type is useful for when we want to initialize classes with struct values, or nullable classes that 
    /// could be modified at runtime. It decouples us with the responsability to update said classes once the 
    /// value is updated in an elegant way.
    /// 
    /// This is has some similarities with pointers.
    /// </summary>
    /// <typeparam name="T">Struct type that's passed by reference instead of by value.</typeparam>
    public class Reference<T> : IReadOnlyReference<T>
    {
        private T _valueInternal;

        public T Value
        {
            get => this._valueInternal;
            set
            {
                this.OnValueUpdated(this._valueInternal, value);
                this._valueInternal = value;
            }
        }

        public Reference(T value) { this.Value = value; }

        protected virtual void OnValueUpdated(T oldValue, T newValue)
        {
        }
    }

    public interface IReadOnlyReference<T>
    {
        /// <summary>
        /// Value of the reference.
        /// </summary>
        T Value { get; }
    }
}
