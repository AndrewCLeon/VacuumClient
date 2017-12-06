namespace IntelligentVacuum.Agent
{
    using System;
    using System.Collections.Generic;
    using Environments;

    public class GraphNode
    {
        public GraphNode(Room room, GraphNode parent, AgentAction action)
        {
            this.Room = room;
            this.Parent = parent;
            this.Action = action;
        }
        
        public Room Room { get; set; }

        public GraphNode Parent { get; set; }

        public AgentAction Action { get; set; }
    }
}