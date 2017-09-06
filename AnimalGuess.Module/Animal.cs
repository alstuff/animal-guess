using System;
using System.Collections.Generic;

namespace AnimalGuess.Module
{
    /// <summary>
    /// Class to represent animal domain object.
    /// Author: Alvin Husin
    /// </summary>
    [Serializable]
    public class Animal
    {
        public string Name { get; set; }
        public List<AnimalAttribute> Attributes { get; set; }

        public Animal()
        {
        }

        public Animal(string name) : base()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Attributes = new List<AnimalAttribute>();
        }

        public void AddAttribute(AnimalAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (string.IsNullOrWhiteSpace(attribute.Text))
                throw new ArgumentException("AnimalAttribute cannot be null or empty");

            if (!Attributes.Exists(a =>
                a.Type == attribute.Type &&
                a.Text.Equals(attribute.Text, StringComparison.OrdinalIgnoreCase)))
            {
                Attributes.Add(attribute);
            }
        }
    }

    [Serializable]
    public class Animals : List<Animal> { }
}
