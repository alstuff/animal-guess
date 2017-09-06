using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalGuess.Module
{
    public interface IAnimalTree
    {
        AnimalNode RootNode { get; set; }
        void BuildTree(Animals data);
    }

    /// <summary>
    /// Class to represent decision tree consisting of animal nodes.
    /// Author: Alvin Husin
    /// </summary>
    public class AnimalTree : IAnimalTree
    {
        public AnimalNode RootNode { get; set; }

        public void BuildTree(Animals data)
        {
            var currAttributes = GetAnimalAttributes(data);
            RootNode = CreateNode(RootNode, currAttributes, data);
        }

        private List<AnimalAttribute> GetAnimalAttributes(Animals data)
        {
            var attributes = new List<AnimalAttribute>();
            foreach (var item in data)
            {
                foreach (var attr in item.Attributes)
                {
                    if (!attributes.Any(q => q.Text.Equals(attr.Text, StringComparison.OrdinalIgnoreCase)))
                        attributes.Add(attr);
                }
            }

            // randomise questions so that we'll get different first question each play
            var random = new Random();
            attributes = attributes.OrderBy(i => random.Next(0, attributes.Count -1)).ToList();

            return attributes;
        }

        private AnimalNode CreateNode(AnimalNode animalNode, List<AnimalAttribute> currAttributes, List<Animal> data)
        {
            if (data == null)
                return null;

            if (animalNode == null)
                animalNode = new AnimalNode();

            var question = currAttributes.FirstOrDefault();
            if (question == null)
            {
                // no more question/attribute to check so this is the end of the branch node
                animalNode.MatchedAnimals = data;
                return animalNode;
            }

            animalNode.Attribute = question;

            List<Animal> matched = null, unmatched = null;
            foreach (var item in data)
            {
                if (item.Attributes.Any(i => i.Text.Equals(question.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    if (matched == null) matched = new List<Animal>();
                    if (!matched.Contains(item))
                        matched.Add(item);
                }
                else
                {
                    if (unmatched == null) unmatched = new List<Animal>();
                    if (!unmatched.Contains(item))
                        unmatched.Add(item);
                }
            }
            if (matched != null)
            {
                RemoveAttribute(matched, question);
                animalNode.MatchedAnimals = matched;
            }

            var yesQuestions = (from i in animalNode.MatchedAnimals from a in i.Attributes
                                where !a.Text.Equals(question.Text, StringComparison.OrdinalIgnoreCase)
                                select a).Distinct().ToList();

            if (unmatched != null)
            {
                RemoveAttribute(unmatched, question);
            }

            var noQuestions = unmatched == null ? null : (from i in unmatched from a in i.Attributes
                                                           where !a.Text.Equals(question.Text, StringComparison.OrdinalIgnoreCase)
                                                           select a).Distinct().ToList();

            animalNode.MatchedNode = CreateNode(animalNode.MatchedNode, yesQuestions, matched);
            animalNode.UnmatchedNode = CreateNode(animalNode.UnmatchedNode, noQuestions, unmatched);

            return animalNode;
        }

        private void RemoveAttribute(List<Animal> list, AnimalAttribute attrToRemove)
        {
            foreach (var i in list)
            {
                var item = i.Attributes.FirstOrDefault(v => v.Type == attrToRemove.Type && v.Text == attrToRemove.Text);
                if (item != null)
                    i.Attributes.Remove(item);
            }
        }
    }
}
