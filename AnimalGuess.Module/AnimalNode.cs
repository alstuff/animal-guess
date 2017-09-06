using System.Collections.Generic;
using System.Linq;

namespace AnimalGuess.Module
{
    /// <summary>
    /// Class to represent a node in the Animal Tree.
    /// Author: Alvin Husin
    /// </summary>
    public class AnimalNode
    {
        internal List<Animal> MatchedAnimals { get; set; }
        public AnimalAttribute Attribute { get; set; }
        public AnimalNode MatchedNode { get; set; }
        public AnimalNode UnmatchedNode { get; set; }

        public string Question
        {
            get
            {
                if (Attribute == null) return null;

                var question = string.Empty;
                switch (Attribute.Type)
                {
                    case AttributeType.Action:
                        question = $"Does it {Attribute.Text}";
                        break;

                    case AttributeType.Has:
                        question = $"Does it have {Attribute.Text}";
                        break;

                    case AttributeType.Is:
                        question = $"Is it {Attribute.Text}";
                        break;
                }
                return question;
            }
        }

        public bool HasResult => MatchedNode == null && UnmatchedNode == null;

        public string Result => MatchedAnimals?.FirstOrDefault()?.Name;
    }
}
