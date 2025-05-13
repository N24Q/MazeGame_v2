using MazeGame_v2;

//This class contains a BFS (Breadth first search) pathfinding algorithm, chosen after doing research on the best algorithm to complete a maze -
//Created mainly by myself but with the help of stackoverflow and asking chatgpt questions.

public class Pathfinder
{
    private int[,] maze; //initiating variables
    private (int x, int y) start;
    private (int x, int y) goal;

    public Pathfinder(int[,] maze, (int x, int y) start, (int x, int y) goal)
    {
        this.maze = maze;
        this.start = start;
        this.goal = goal;
    }

    public List<(int x, int y)> FindPath()
    {
        var queue = new Queue<(int x, int y)>();
        //queues are FIFO (first in last out) collections that allow ordering of executions which is extremely helpful in something like a BFS to be able to process nodes (coordinate points)
        //layer by layer and explore the grid properly
        var cameFrom = new Dictionary<(int x, int y), (int x, int y)>(); //dictionary to store previously visited coordinates which are used to reconstruct the final path
        
        var visited = new HashSet<(int x, int y)>(); 
        //hashsets are unordered collections of UNIQUE elements (automatically removing duplicates) which makes it easy to store and check items, these are also helpful for a BFS because
        //they allow, in this case, to easily track visited nodes

        //Note - queue and hashset work well together for a bfs algorithm like this

        queue.Enqueue(start); //adding the start position to the queue
        visited.Add(start); //marking the start position as visited
        cameFrom[start] = (-1, -1); //marking the start position as a flag (-1,-1) which is different to other coordinates because it has no previous position, this is an indicator
        //for the reconstructpath method to stop reconstructing when it reaches this flag

        while (queue.Count > 0) //while the queue has things to do lol
        {
            var current = queue.Dequeue(); //dequeue the every next position to process it

            if (current == goal) //if the goal position is reached (current position is goal position coords)
            {
                var path = ReconstructPath(cameFrom, current); //call reconstructpath to reconstruct the shortest path that the algorithm took to the goal from previous moves
                return path; //returning the constructed path
            }

            foreach (var neighbor in GetNeighbors(current.x, current.y)) //for each neighbor returned by the getneighbors method
            {
                if (!visited.Contains(neighbor)) //if the neighbor position has not been visited
                {
                    queue.Enqueue(neighbor); //queue the neighbor
                    visited.Add(neighbor); //add the neighbor to visited
                    cameFrom[neighbor] = current; //store where the neighbor came from (current) in cameFrom
                }
            }
        }
        //If the queue is emptied without finding a path to the goal position
        MessageBox.Show("No valid path found!"); //Message for debugging
        return new List<(int x, int y)>(); //Return empty list with no path
    }

    private List<(int x, int y)> GetNeighbors(int x, int y) //Method to find neighbors of current position
    {
        var neighbors = new List<(int x, int y)> //defining potential moves as the surrounding coordinates
        {
            (x, y - 1), (x, y + 1), (x - 1, y), (x + 1, y)
        };

        //
        return neighbors.Where(n => //retunring neighbors from the list of potential neighbors where they are both in bounds of the maze array and also not on the position of a wall
            n.x >= 0 && n.y >= 0 &&
            n.x < maze.GetLength(0) && n.y < maze.GetLength(1) &&
            maze[n.x, n.y] != 0).ToList();
    }

    private List<(int x, int y)> ReconstructPath(Dictionary<(int x, int y), (int x, int y)> cameFrom, (int x, int y) current) //Method to construct the shortest path to the goal position from the start
    {
        var path = new List<(int x, int y)>(); //creating a new list path

        while (cameFrom.ContainsKey(current) && cameFrom[current] != (-1, -1)) //backtracking from the goal position to the start position using the cameFrom dictionary
            //until the flag (-1, -1) set earlier is reached
        {
            path.Add(current); //adding the current position to the path (starting at the goal)
            current = cameFrom[current]; //changing current to cameFrom which contains its own current
        }

        path.Add(start); //add the start point to the path as well so it is included as part of the path
        path.Reverse(); //reverse the path list so that it goes from start to goal instead of goal to path
        return path; //finally return the path
    }
    public List<string> TranslateToCommands(List<(int x, int y)> path) //method to translate the path into executable commands
    {
        List<string> commands = new List<string>(); //intialising the command list 
        Direction currentDirection = DetermineDirection(path[0], path[1]); //defining the current direction using the DetermineDirection method

        for (int i = 1; i < path.Count; i++) //looping through each coordinate in path starting from the second position (no command needed for the first position because already facing correct direction)
        {
            var prev = path[i - 1]; //storing the previous and next position
            var next = path[i];

            Direction nextDirection = DetermineDirection(prev, next); //determining the next direction using method determinedirection using the previous and next position

            if (nextDirection != currentDirection) //if the next direction is not equal to the current direction aka turn is needed
            {
                commands.Add($"Turn[{GetTurnCommand(currentDirection, nextDirection)}]"); //add a turn command to the command list using GetTurnCommand to determine the correct turn
                currentDirection = nextDirection; //setting the current direction to the next direction
            }

            int moveSteps = 1; //initialising moveSteps as 1
            while (i + 1 < path.Count && DetermineDirection(path[i], path[i + 1]) == currentDirection) //while a turn is not needed while moving through the path
            {
                moveSteps++; //increment moveSteps
                i++; //increment i
            }
            commands.Add($"Move[{moveSteps}]"); //add the move command with the amount of steps moved
        }

        return commands; //return the commands list
    }

    private Direction DetermineDirection((int x, int y) from, (int x, int y) to) //method to determine the direction using coordinates (from and to)
    {
        if (to.y < from.y) return Direction.Up; //comparing coordinates of from and to, to determine the direction of movement
        if (to.y > from.y) return Direction.Down;
        if (to.x < from.x) return Direction.Left;
        return Direction.Right;
    }

    private char GetTurnCommand(Direction current, Direction next) //method to get the right turn direction using current and next direction
    {
        return (current, next) switch //using a switch statement
        {
            (Direction.Up, Direction.Right) or (Direction.Right, Direction.Down) or //case (all of the cases for the turning direction being right)
            (Direction.Down, Direction.Left) or (Direction.Left, Direction.Up) => 'R', //turning direction is right (R)
            _ => 'L' //otherwise turning direction is left (L)
        };
    }
}
