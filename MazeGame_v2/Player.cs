using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_v2
{
    public enum Direction //making an enumerable for directions
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Player //class player
    {
        public int X { get; private set; } //declaring x and y
        public int Y { get; private set; }
        public Direction Facing { get; private set; } //declaring direction facing
        private int[,] maze; //declaring maze

        public Player(int[,] maze)
        {
            this.maze = maze;
            FindStartPosition(); //finding start position for the player in the current maze
        }

        private void FindStartPosition() //Method to find the starting position
        {
            for (int i = 0; i < maze.GetLength(0); i++) //Going through all positions of the maze
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == 2) //Start position found (2 means starting point)
                    {
                        X = i; //Set the x and y position to the current position in the maze array
                        Y = j;

                        if (Y > 0 && maze[i, j - 1] == 1) Facing = Direction.Up; //If an open path is above then starting direction = up
                        else if (Y < maze.GetLength(1) - 1 && maze[i, j + 1] == 1) Facing = Direction.Down; //If an open path is below then starting direction = down
                        else if (X > 0 && maze[i - 1, j] == 1) Facing = Direction.Left; //If an open path is left then starting direction = left
                        else if (X < maze.GetLength(0) - 1 && maze[i + 1, j] == 1) Facing = Direction.Right; //If an open path is right then starting direction = right

                        return;
                    }
                }
            }
        }

        public bool Move(Direction direction) //method to move the player
        {
            int newX = X, newY = Y; //initialise newX and newY as current x and y positions
            Facing = direction; //intialise Facing as the current direction

            switch (direction) //moving player based on facing direction
            {
                case Direction.Up: newY--; break; //player facing up = decrease Y value
                case Direction.Down: newY++; break; //player facing down = increase Y value
                case Direction.Left: newX--; break; //player facing left = decrease X value 
                case Direction.Right: newX++; break;//player facing right = increase X value
            }

            if (newX >= 0 && newY >= 0 && newX < maze.GetLength(0) && newY < maze.GetLength(1)) //If new x and new y are within the maze array
            {
                if (maze[newX, newY] == 0) return false; //If they are in the position of a wall return false because player hit a wall

                //Otherwise set X and Y as the newX and newY and return true for valid move
                X = newX;
                Y = newY;
                return true;
            }
            return false;//Otherwise move not in bounds so return false
        }
        public void Turn(char turnDirection) //method to turn the character based on input
        {
            Facing = turnDirection == 'R' ? Facing switch //ternary operator to decide if the input is R (right) or L (left) for turning direction
            {
                //If its right, then change directions accordingly to turning right
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => Facing //otherwise remaining the same facing direction
            } :
            Facing switch
            {
                //If its left, then change directions accordingly to turning left
                Direction.Up => Direction.Left,
                Direction.Left => Direction.Down,
                Direction.Down => Direction.Right,
                Direction.Right => Direction.Up,
                _ => Facing //otherwise remaining the same facing direction
            };
        }
    }
}
