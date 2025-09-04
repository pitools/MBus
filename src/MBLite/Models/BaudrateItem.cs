namespace MBLite.Models
{
    public class BaudrateItem
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public BaudrateItem(int value, string description)
        {
            Value = value;
            Description = description;
        }

        public override string ToString() => Value.ToString();
    }
}
