namespace IntercontinentalExchange.Host.Models
{
    public class TextValueModel<TValue>
    {
        public TextValueModel(TValue value, string text)
        {
            Value = value;
            Text = text;
        }
        public TValue Value { get; }
        public string Text { get; }
    }
}
