namespace IntelligentVacuum.Agent
{
    using System;
    using System.Collections.Generic;
    using Environments;
    
    public class Agent
    {
        private MovementPlan Plan { get; set; }
        public Agent(Sensor sensor)
        {
            this.Plan = new MovementPlan(sensor);
        }

        public AgentAction DecideAction(Room room)
        {
            return Plan.NextAction(room);
        }
    }
}