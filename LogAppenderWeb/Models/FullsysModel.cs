namespace LogAppenderWeb.Models
{
    public class FullsysModel
    {
        public string Content { get; set; }

        public int Length => Content.Length;
    }
}