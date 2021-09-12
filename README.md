# TurtleSimple

## Classes description
### Program
- Reads input 
- creates DTOs
- executes GameManager actions per char read
- uses GameManager to orchestrate all the work

### GameManager
Contains a GameBoard and Turtle. Validates gamestate by checking colisions.

### GameBoard
Contains a matrix (2x2 array) with all the game objects that exist in the game.

### ITransform
Inspired in Unity3D, contains Vec2 for Location and Rotation

### IMovable (Turtle)
- Contains Rotate and Move methods
- Contains a Transform

## Performance
O(n) since it parses throught all moves once

