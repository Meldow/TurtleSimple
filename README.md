# TurtleSimple

Thank you for taking the time to look at my implementation for the TurtleChallenge.

## Expected Performance and overall Objective
O(n) since it parses throught all moves a single time

The program first reads `game-settings.in`, placing all GameObjects accordingly,
providing the appropriate feedback if any is placed outside the board, ignoring it.
Note that will never ignore when trying to place the Turtle, as this is the only
mandatory GameObject.

Once the GameBoard is created, it will read `moves.in` char by char, acting accordingly.
- If any collision is detected or the Turtle drops out of the GameBoard, the expected feedback
  is provided, ending the sequence, and all further moves in that line are ignored, jumping to the next one
- If no more moves are provided, the expected feedback is provided, ending the sequence, jumping to
  the next one

This loop will continue for every sequence (line) provided in the `moves.in` file.

## Classes description
### Actions
Contains all actions (interactions) possible

### Core
Contains core math concepts (similar to Unity3D)

### Exceptions
Game logic specific exceptions

### GameManagement
Main objects agregator. Contains a GameBoard and a Turtle. Maintains and updates each run state.

### GameObjects
Contains all objects
- GameBoard: Where all the Static objects are placed
- Static: Mines and Exits
- Movable: Turtle that will move

### InputDTO
Contains all the DTO necessary to create a GameBoard and later process moves.

### InputManagement
Knows how to read the specified input and generate the correct DTO to later create the GameBoard.
   
## Turtle Challenge Variants
I've also implemented a **Playable** version for this Challenge. 
- It is fully playable directly from the terminal
- Move the (T)urtle using the arrow keys
- Avoid the (M)ines
- Eat (A)pples to gain lives and destroy (M)ines
- Contains full session stats in the end, press `R` to restart
- Randomly placed items (you define the GameBoard size and how many items should be placed)

Important note, **its code should be disregarded** :p

See how many times you can escape from this evil pond! (??)

You can find it [here](https://github.com/Meldow/Turtle). 
To run it, download, and execute

`dotnet run p playable-game-settings.in moves.in`

Big thank you if you are still reading this!  
You are the best!