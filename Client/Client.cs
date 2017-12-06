namespace IntelligentVacuum.Client
{
    using System;
    using Agent;
    using Environments;

    public class Client
    {

        public void Run(int xSize, int ySize, int rounds, bool lockRooms)
        {
            var map = new Environments.AreaMap(xSize, ySize, .5f, lockRooms);
            var actionResult = new ActionResult(map.AgentRoom);
            var sensor = new Sensor(map);
            var agent = new Agent(sensor);
            var engine = new GameEngine(map);
            Room agentCurrentRoom = map.Rooms[0,0];
            int startDirt = map.GetDirtCount();
            for(int i = 0; i < rounds; i++)
            {
                var action = agent.DecideAction(map.AgentRoom);
                Update(engine, action);
                Draw(map, i);
            }
            System.Console.WriteLine("===GAME OVER===");
            Console.WriteLine("Starting Dirt: {0}", startDirt);
            Console.WriteLine("Ending Dirt: {0}", map.GetDirtCount());
        }

        public ActionResult Update(GameEngine engine, AgentAction action)
        {
            return engine.DoAction(action);
        }

        public void Draw(AreaMap map, int round)
        {
            Console.WriteLine("Round {0}", round);
            DrawLine(map.Rooms.GetLength(0));
            for(int y = 0; y < map.Rooms.GetLength(1); y++)
            {
                Console.WriteLine();
                Console.Write('|');
                for (int x = 0; x < map.Rooms.GetLength(0); x++)
                {
                    Room room = map.Rooms[x,y];
                    // could maybe replace with a reference check...
                    if (room.XAxis == map.AgentRoom.XAxis && room.YAxis == map.AgentRoom.YAxis)
                    {
                        ConsoleColor prevColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('V');
                        Console.ForegroundColor = prevColor;
                    }
                    else if (room.IsDirty)
                    {
                        Console.Write('D');
                    }
                    else if (room.IsLocked)
                    {
                        ConsoleColor prevColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('L');
                        Console.ForegroundColor = prevColor;
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                    Console.Write('|');
                }
                DrawLine(map.Rooms.GetLength(0));
            }
            Console.WriteLine();
        }

        private void DrawLine(int length)
        {
            Console.WriteLine();
            for (int i = 0; i <= length * 2; i++)
            {
                System.Console.Write('-');
            }
        }
    }
}