namespace Tools
{
    public class ObservableVariable<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public event ValueChanged OnValueChanged;
        public delegate void ValueChanged(T value);
    }
}