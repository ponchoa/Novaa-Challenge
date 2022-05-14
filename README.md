# Novaa-Challenge
A small challenge issued by Novaa Inc. to assert my technical level using Unity.

## Scalable project

I did my best to make the project as scalable as possible. As such, here is how you would add any important element to the project:

### UI Scene

If, for any reason, a developer wanted to add any scene to the project, it wouldn’t be hard to do so and having it be integrated quickly.
Every scene represents one state of the game, and is loaded additively during gameplay at the appropriate time. As such, only the Main Scene holds an Event System and a Main Camera. If they are swapping an existing scene for a new one, they can skip steps 2 and 3.

1. The developer would have to create the scene, and build it the way they want.
2. Then they would have to modify the GameStateEnum.cs file in Scripts/Enum to add the corresponding game state.
3. They would also have to edit the GameStateController.cs file in Scripts/Controllers to change the method that loads a state according to the previous one, and insert their new state at the desired step.
4. Then they would either create and fill a new Scene Data/Database in the ScriptableObject/Scene Data folder, or edit an existing one.
5. They would finally need to check that the Game Manager in the Main scene is referencing the correct database in the Game State Controller component.

### New question

1. To add a new question, the developer would simply create a new Quiz/Question.
2. Then in the inspector they have options: They can enter a question statement and add answers.
3. Then they need to check the correct answer.

### New category

1. To add a new category, the developer would simply create a new Quiz/Category.
2. Then in the inspector, they can enter the category name, and link questions.
3. They have to have between 2 and 5 valid questions, if a question is invalid, the inspector will warn the developer.
4. But if everything is correct, they then need to link the category in the Category Choice UI Scene, on the Category Scene Manager GameObject.
5. The game  will automatically add the category with the linked questions to the game.

## Author

Benoît Ponchon
