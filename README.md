# animal-guess code challenge project
Author: Alvin Husin

This is a console application written in C# to simulate animal guessing game.
Upon startup, user will be presented with a main menu with several options:

        [1] Reset animal data to default values in XML file
        [2] Populate animal data from XML file
        [3] Add a new animal
        [4] Play game
        [X] Exit

The system will automatically create a default of 5 animals with multiple attributes and save them into an XML file (animals.xml).

The list of default animals are: elephant, lion, dog, bird, and butterfly.
The descriptions of the animals can be found in AnimalGuess.UIConsole.AnimalGame.cs under SetDefaultData() method.

User will be able to add a new animal and save it to the file as well as populate the list from the file. The animal list will be refreshed automatically.
At least one attribute must be entered for the animal. User only needs to enter the main words from which the system will construct questions.
The options to add a new attribute of an animal are as follows.

        [A] Action (verb) e.g. 'fly', 'bark', 'eat cheese'
        [B] Has e.g. 'four legs', 'wings'
        [C] Is (adjective) e.g. 'big', 'yellow colour', 'an insect'
        [X] Cancel

To play the game user needs to select option [4] where the system will ask a series of questions to guess the animal based on the information.

Due to time limitation, unit tests are currently unavailable. However the classes are designed with testability in mind.

Enjoy the game.

Regards,
Alvin Husin