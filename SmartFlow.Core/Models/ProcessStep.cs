
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessStep
    {
        public Guid Id { get; set; }
        public Process Process { get; set; }
        public List<TransitionAction> TransitionActions { get; set; }
        public Entity Entity { get; set; }
        public bool IsCompleted { get; set; }
        public int EntityType { get; set; }
        public Guid ProcessId { get; set; }
    }
}
