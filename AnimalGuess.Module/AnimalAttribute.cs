using System;

namespace AnimalGuess.Module
{
    public enum AttributeType { Action, Has, Is }

    [Serializable]
    public class AnimalAttribute
    {
        public AttributeType Type { get; set; }
        public string Text { get; set; }

        public AnimalAttribute() { }

        public AnimalAttribute(AttributeType type, string attribute)
        {
            Type = type;
            Text = attribute?.Trim().ToLower();
        }
    }
}
