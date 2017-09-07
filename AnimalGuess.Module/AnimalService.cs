using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalGuess.Module
{
    public interface IAnimalService
    {
        void SaveAnimals(Animals animals);
        void AddAnimal(Animal animal);
        List<string> GetAnimalNames();
        Animals GetAnimals();
        Animal GetAnimalByName(string name);
        AnimalNode GetAnimalTree();
    }


    /// <summary>
    /// Animal Service implementation for the game engine.
    /// Author: Alvin Husin
    /// </summary>
    public class AnimalService : IAnimalService
    {
        private readonly IDataRepository<Animals> _animalRepo;
        private readonly IAnimalTree _animalTree;

        public AnimalService(IDataRepository<Animals> animalRepo, IAnimalTree animalTree)
        {
            _animalRepo = animalRepo ?? throw new ArgumentNullException(nameof(animalRepo));
            _animalTree = animalTree ?? throw new ArgumentNullException(nameof(animalTree));
        }

        public void SaveAnimals(Animals animals)
        {
            if (animals == null)
                throw new ArgumentNullException(nameof(animals));

            foreach (var animal in animals)
            {
                if (!IsValid(animal))
                    throw new InvalidAnimalException();
            }

            _animalRepo.SaveData(animals);
        }

        public void AddAnimal(Animal animal)
        {
            if (!IsValid(animal))
                throw new InvalidAnimalException();

            var data = GetAnimals();
            data.Add(animal);
            SaveAnimals(data);
        }

        public Animals GetAnimals()
        {
            return _animalRepo.GetData();
        }
        
        public Animal GetAnimalByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var data = GetAnimals();
            return data?.FirstOrDefault(i => i.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public List<string> GetAnimalNames()
        {
            return GetAnimals()?.Select(i => i.Name).ToList();
        }

        public AnimalNode GetAnimalTree()
        {
            var data = GetAnimals();
            if (data == null) return null;

            _animalTree.BuildTree(data);

            return _animalTree.RootNode;
        }

        private bool IsValid(Animal animal)
        {
            return !string.IsNullOrWhiteSpace(animal?.Name) && animal.Attributes != null && animal.Attributes.Any();
        }
    }
}
