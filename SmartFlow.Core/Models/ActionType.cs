using System;

namespace SmartFlow.Core.Models
{
    public sealed class ActionType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TypeCode { get; set; }
    }

    public enum ActionTypes
    {
        Crm = 1
    }
}
