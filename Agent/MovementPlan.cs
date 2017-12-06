namespace IntelligentVacuum.Agent
{
    using System;
    using System.Collections.Generic;
    using Environments;

    public class MovementPlan
    {
        public Stack<AgentAction> Plan { get; set; }
        private Sensor Sensor { get; set; }
        private AgentAction[] PossibleAgentActions = new AgentAction[] { AgentAction.MOVE_DOWN, AgentAction.MOVE_LEFT,  AgentAction.MOVE_RIGHT, AgentAction.MOVE_UP};
        public MovementPlan(Sensor sensor)
        {
            this.Sensor = sensor;
            this.Plan = new Stack<AgentAction>();
        }

        public AgentAction NextAction(Room room)
        {
            AgentAction action;
            if(!Plan.TryPop(out action))
            {
                BuildPlan(room);
                if(!Plan.TryPop(out action))
                {
                    action = AgentAction.NONE;
                }
            }
            return action;
        }

        private bool BuildPlan(Room room)
        {
            Plan.Clear();
            HashSet<Room> explored = new HashSet<Room>();
            Queue<GraphNode> frontier = new Queue<GraphNode>();
            GraphNode node = new GraphNode(room, null, AgentAction.NONE);
            frontier.Enqueue(node);
            do
            {
                if(!frontier.TryDequeue(out node))
                {
                    return false;
                }

                List<GraphNode> newNodes = DiscoverSurroundingNodes(node);
                explored.Add(node.Room);
                newNodes.RemoveAll(n => explored.Contains(n.Room));

                foreach(GraphNode newNode in newNodes)
                {
                    frontier.Enqueue(newNode);
                }

            } while(!node.Room.IsDirty);

            Plan.Push(AgentAction.CLEAN);
            while(node.Parent != null)
            {
                Plan.Push(node.Action);
                node = node.Parent;
            }
            return true;
        }

        private List<GraphNode> DiscoverSurroundingNodes(GraphNode node)
        {
            List<GraphNode> discovered = new List<GraphNode>();
            foreach(AgentAction action in PossibleAgentActions)
            {
                GraphNode newNode = Transition(node, action);
                if(newNode != null && !newNode.Room.IsLocked)
                {
                    discovered.Add(newNode);
                }
            }
            return discovered;
        }
        
        private GraphNode Transition(GraphNode node, AgentAction action)
        {
            Room newRoom = null;
            switch (action)
            {
                case AgentAction.MOVE_UP:
                newRoom = Sensor.SenseRoom(node.Room.XAxis, node.Room.YAxis - 1);
                break;

                case AgentAction.MOVE_DOWN:
                newRoom = Sensor.SenseRoom(node.Room.XAxis, node.Room.YAxis + 1);
                break;
                
                case AgentAction.MOVE_LEFT:
                newRoom = Sensor.SenseRoom(node.Room.XAxis - 1, node.Room.YAxis);
                break;
                
                case AgentAction.MOVE_RIGHT:
                newRoom = Sensor.SenseRoom(node.Room.XAxis + 1, node.Room.YAxis);
                break;
            }
            GraphNode newNode = null;
            if(newRoom != null)
            {
                newNode = new GraphNode(newRoom, node, action);
            }
            return newNode;
        }
    }
}