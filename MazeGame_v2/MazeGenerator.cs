using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame_v2
{
    //This class contains a recursive algorithm to generate a maze using paths, as well as setting start and end points

    public class MazeGenerator
    {
        private int[,] maze; //declaring 2d array maze, width and height, random, and endpoint as coordinates
        private int width, height;
        private Random rand = new Random();
        private (int x, int y) endPoint;

        public MazeGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
            maze = new int[width, height]; //initialising the maze array
            GenerateMaze(); //call GenerateMaze
        }
        private void GenerateMaze()
        {
            for (int x = 0; x < width; x++) //Fill the maze array with 0s (walls)
                for (int y = 0; y < height; y++)
                    maze[x, y] = 0;

            CreatePath(1, 1); //calling createpath method passing 1,1 position of the maze array (starting at top left corner 1 diagonally down right to keep walls)
            SetStartEndPoints();
        }
        private void CreatePath(int x, int y) //Recursive backtracking (not sure if this is most efficient) method to create a maze path
        {
            //note - dx/dy = direction x/direction y & nx/ny = next x/next y

            maze[x, y] = 1; //Set the current position in the maze to 1 to turn it into a path
            var directions = new (int dx, int dy)[] { (0, -1), (1, 0), (0, 1), (-1, 0) }; //define possible directions as coordinates that could be moved to next (up, right, down, left)
            directions = directions.OrderBy(d => rand.Next()).ToArray(); //order them randomly into an array

            foreach (var (dx, dy) in directions) //loop through each random direction
            {
                //calculating the position
                int nx = x + dx * 2, ny = y + dy * 2; //moving 2 steps in any given direction so that the paths are wide enough and don't go next to each other
                if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[nx, ny] == 0) //check if the position is valid by checking that its both in the bounds of the maze array and also not been turned into a path already
                {
                    maze[x + dx, y + dy] = 1; //connect the current position to the next step in the maze by setting it to also a path (1)
                    CreatePath(nx, ny); //Recursive call that repeats the method until no more valid moves are available
                }
            }
        }
        private void SetStartEndPoints() //Method to set start point and end point of the maze
        {
            //Starting to search for maze start position
            for (int x = 1; x < width; x++) //Go through every position
            {
                for (int y = 1; y < height; y++)
                {
                    if (maze[x, y] == 1) //The first available path position will be the start point
                    {
                        maze[x, y] = 2; //Set current position in maze array to 2 to represent start point
                                        //Not storing coordinates for start point because it is found in a different way to spawn the player
                        break; //Exit and move on to finding end point
                    }
                }
                break;
            }

            //This completes the previous search in reverse order to find the furthers point from the start
            for (int x = width - 2; x > 0; x--) //Searching array in oposite direction
            {
                for (int y = height - 2; y > 0; y--)
                {
                    if (maze[x, y] == 1) //If current position is available path
                    {
                        maze[x, y] = 3; //Set current position to 3 to represent end point
                        endPoint = (x, y); //store coordinates for end point
                        return; //end
                    }
                }
            }
        }

        public int[,] GetMaze() => maze; //Returning the final maze
        public (int x, int y) GetEndPoint() => endPoint; //return end point
    }

}
