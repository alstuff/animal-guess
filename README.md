# animal-guess for code challenge project
Author: Alvin Husin

This is a console application written in .NET to simulate animal game guessing.
Upon startup, user will be presented with main menu with several options:

        [1] Reset animal data to default values in XML file
        [2] Populate animal data from XML file
        [3] Add a new animal
        [4] Play game
        [X] Exit

The system will automatically create 5 animals with multiple attributes and save them into an XML file (animals.xml).

The list of default animals are: elephant, lion, dog, bird, and butterfly.
The descriptions for the animals can be found in AnimalGuess.UIConsole.AnimalGame.cs under SetDefaultData() method.

User will be able to add a new animal and save it to the file. The animal list will be refreshed automatically.
The options to add a new attribute of an animal are as follows.

        [A] Action (verb) e.g. 'fly', 'bark', 'eat cheese'
        [B] Has e.g. 'four legs', 'wings'
        [C] Is (adjective) e.g. 'big', 'yellow colour', 'an insect'
        [X] Cancel

To play the game user needs to select option [4] where the system will ask for questions to guess the animal.

Due to time limitation, unit tests are currently unavailable. However the classes are designed with testability in mind.

Enjoy the game.

Regards,
Alvin Husin