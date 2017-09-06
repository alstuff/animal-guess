using System;

namespace AnimalGuess.Module
{
    public class InvalidAnimalException : Exception
    {
        public InvalidAnimalException(): base() {}
        public InvalidAnimalException(string message) : base(message) { }
    }
}
