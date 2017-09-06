using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AnimalGuess.Module;

namespace AnimalGuess.UIConsole
{
    public enum AnimalAttributeType { Physical, Characteristic }

    /// <summary>
    /// Game engine for Animal guessing.
    /// Author: Alvin Husin
    /// </summary>
    public class AnimalGame
    {
        private readonly IDataRepository<Animals> _animalRepo;
        private readonly IAnimalService _animalService;
        private readonly IAnimalTree _animalTree;
        private readonly string[] _menuOptions = { "1", "2", "3", "4", ExitOption };
        private const string AnimalFile = "animals.xml";
        private const string ExitOption = "X";
        
        
        private Animals _animals;

        public AnimalGame()
        {
            _animalRepo = new DataRepositoryXml<Animals>(AnimalFile);
            _animalTree = new AnimalTree();
            _animalService = new AnimalService(_animalRepo, _animalTree);
        }

        public void Start()
        {
            SetDefaultData();

            Console.Clear();
            Console.WriteLine("Welcome to the Animal Guessing Game");
            Console.WriteLine("===================================");

            while (GetInput() != ExitOption) { }
        }

        public string GetInput()
        {
            Console.WriteLine("Choose one of the following options:");
            Console.WriteLine("[1] Reset animal data to default values in XML file");
            Console.WriteLine("[2] Populate animal data from XML file");
            Console.WriteLine("[3] Add a new animal");
            Console.WriteLine("[4] Play game");
            Console.WriteLine($"[{ExitOption}] Exit");

            var input = GetValidInput(_menuOptions);

            Console.WriteLine();
            Console.WriteLine($"Selected option: [{input}]");    

            switch (input)
            {
                case "1":
                    SetDefaultData();
                    break;

                case "2":
                    GetDataFromFile();
                    break;

                case "3":
                    AddAnimal();
                    break;

                case "4":
                    PlayGame();
                    break;
            }
            return input;
        }


        private void SetDefaultData()
        {
            var animals = new Animals();

            var animal1 = new Animal("elephant");
            animal1.AddAttribute(new AnimalAttribute(AttributeType.Has, "a trunk"));
            animal1.AddAttribute(new AnimalAttribute(AttributeType.Has, "trumpets"));
            animal1.AddAttribute(new AnimalAttribute(AttributeType.Is, "grey"));
            animal1.AddAttribute(new AnimalAttribute(AttributeType.Is, "big"));
            animals.Add(animal1);

            var animal2 = new Animal("lion");
            animal2.AddAttribute(new AnimalAttribute(AttributeType.Has, "a mane"));
            animal2.AddAttribute(new AnimalAttribute(AttributeType.Action, "roar"));
            animal2.AddAttribute(new AnimalAttribute(AttributeType.Has, "four legs"));
            animal2.AddAttribute(new AnimalAttribute(AttributeType.Is, "yellow"));
            animal2.AddAttribute(new AnimalAttribute(AttributeType.Is, "big"));
            animals.Add(animal2);

            var animal3 = new Animal("dog");
            animal3.AddAttribute(new AnimalAttribute(AttributeType.Action, "bark"));
            animal3.AddAttribute(new AnimalAttribute(AttributeType.Has, "four legs"));
            animal3.AddAttribute(new AnimalAttribute(AttributeType.Is, "men's best friend"));
            animal3.AddAttribute(new AnimalAttribute(AttributeType.Is, "small"));
            animals.Add(animal3);

            var animal4 = new Animal("bird");
            animal4.AddAttribute(new AnimalAttribute(AttributeType.Action, "fly"));
            animal4.AddAttribute(new AnimalAttribute(AttributeType.Has, "wings"));
            animal4.AddAttribute(new AnimalAttribute(AttributeType.Has, "feathers"));
            animal4.AddAttribute(new AnimalAttribute(AttributeType.Is, "small"));
            animals.Add(animal4);

            var animal5 = new Animal("butterfly");
            animal5.AddAttribute(new AnimalAttribute(AttributeType.Action, "fly"));
            animal5.AddAttribute(new AnimalAttribute(AttributeType.Has, "wings"));
            animal5.AddAttribute(new AnimalAttribute(AttributeType.Is, "an insect"));
            animals.Add(animal5);

            _animalService.SaveAnimals(animals);

            _animals = _animalService.GetAnimals();
            ConsoleWrite($"Done. Animal data successfully saved to '{AnimalFile}'. Data has been refreshed.");
        }

        private void GetDataFromFile()
        {
            try
            {
                _animals = _animalService.GetAnimals();
                if (_animals == null || !_animals.Any())
                    ConsoleWrite("No data retrieved from file.");
                else
                    ConsoleWrite($"Done. Successfully retrieved {_animals.Count} records from file.");
            }
            catch (FileNotFoundException ffe)
            {
                ConsoleWrite("Error: The file has not been created. Please select the option to set up default data.", true);
            }
            catch (Exception ex)
            {
                ConsoleWrite(ex.Message, true);
            }
        }

        private void AddAnimal()
        {
            Console.Write("Enter the name of the animal to add: ");
            var name = Console.ReadLine();

            var currAnimal = _animalService.GetAnimalByName(name);
            if (currAnimal != null)
            {
                ConsoleWrite($"The animal you entered ('{name}') already exists in the file. Please enter a different name.", true);
                return;
            }


            var attrs = new List<AnimalAttribute>();
            var toContinue = true;
            while (toContinue)
            {
                Console.WriteLine();
                Console.WriteLine($"Select an attribute of animal \"{name}\":");
                Console.WriteLine("[A] Action (verb) e.g. 'fly', 'bark', 'eat cheese'");
                Console.WriteLine("[B] Has e.g. 'four legs', 'wings'");
                Console.WriteLine("[C] Is (adjective) e.g. 'big', 'yellow colour', 'an insect'");
                Console.WriteLine("[X] Cancel");

                var input = GetValidInput(new[] {"A", "B", "C", "X"});

                var message = string.Empty;
                var type = default(AttributeType);
                switch (input)
                {
                    case "A":
                        message = $"A(n) {name} can ";
                        type = AttributeType.Action;
                        break;
                    case "B":
                        message = $"A(n) {name} has ";
                        type = AttributeType.Has;
                        break;
                    case "C":
                        message = $"A(n) {name} is ";
                        type = AttributeType.Is;
                        break;
                    case "X":
                        ConsoleWrite("Cancelled adding animal.", true);
                        return;
                }
                Console.WriteLine();
                Console.Write(message);
                var attr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(attr))
                {
                    ConsoleWrite("At least one animal attribute must be entered.", true);
                }
                else
                {
                    if (attrs.Exists(
                        a => a.Type == type && a.Text.Equals(attr, StringComparison.OrdinalIgnoreCase)))
                    {
                        ConsoleWrite($"Error: The description you entered already exists for the {name}.", true);
                    }
                    else
                    {
                        attrs.Add(new AnimalAttribute() {Text = attr, Type = type});
                    }
                    Console.WriteLine();
                    Console.Write("Do you want to add another description [Y/N]? ");
                    toContinue = IsYes();
                }
            }

            // save animal to data store (file)
            _animalService.AddAnimal(new Animal() { Name = name, Attributes = attrs });

            // refresh data
            _animals = _animalService.GetAnimals();

            ConsoleWrite($"Done. Successfully added '{name}' to animal list. Data has been refreshed.");
        }

        private bool IsYes()
        {
            var input = GetValidInput(new[] { "Y", "N" }, false);
            return input == "Y";
        }

        private string GetValidInput(string[] options, bool hideInput = true)
        {
            var input = string.Empty;
            while (options.All(o => o != input))
            {
                input = Console.ReadKey(hideInput).KeyChar.ToString().ToUpper();
            }
            return input;
        }


        private void PlayGame()
        {
            if (_animals == null)
            {
                ConsoleWrite("No animal data available. Please select the option to populate data from file.", true);
                return;
            }

            var names = string.Join(", ", _animalService.GetAnimalNames());
            
            ConsoleWrite($"Pick an animal from the following list: {names}.{Environment.NewLine}I will guess it by asking you some questions.");

            var tree = _animalService.GetAnimalTree();
            if (tree == null)
            {
                ConsoleWrite("Error in building animal tree questions.", true);
                return;
            }

            TraverseTree(tree);
        }

        private void TraverseTree(AnimalNode node)
        {
            Console.WriteLine();
            if (node == null)
            {
                ConsoleWrite("Uh, I don't know about the animal - it's not in my database.");
            }
            else if (node.HasResult)
            {
                Console.Write($"The answer is: {node.Result}. Am I right [Y/N]? ");
                var isYes = IsYes();
                Console.WriteLine();
                ConsoleWrite(isYes ? "I won! :)" : "Oh no.. looks like I need to learn more..");
            }
            else
            {
                Console.Write($"{node.Question} [Y/N]? ");
                TraverseTree(IsYes() ? node.MatchedNode : node.UnmatchedNode);
            }
        }

        private static void ConsoleWrite(string message, bool isError = false)
        {
            Console.WriteLine();
            var tmpForeColour = Console.ForegroundColor;
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine($"> {message}");
            Console.ForegroundColor = tmpForeColour;
            Console.WriteLine();
        }
    }
}
