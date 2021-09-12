# TurtleSimple

## Program
Reads input, creates DTOs and executes GameManager Loop

## Performance
O(2n) since it parses throught N actions twice
- First to create the list of Actions to be later executed
- Second when executing said Actions

These could be merged into a single moment, improving the algorithm by
directly calling the actions upon reading instruction.

I took the deliberate decision to separate these two, so it eases