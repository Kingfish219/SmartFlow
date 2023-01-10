﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Interfaces
{
    public interface IStateMachineOperator : ISmartFlowOperator
    {
        bool RegisterFlow<TFlow>() where TFlow : IStateMachine, new();
        ProcessResult Execute(IStateMachine process);
        Task<ProcessResult> ExecuteAsync(IStateMachine process);
        Task<ProcessResult> AdvanceAsync(ProcessEntity entity,
            ProcessUser user,
            ProcessStepInput input,
            IEntityRepository entityRepository,
            EntityCommandType commandType = EntityCommandType.Update,
            Dictionary<string, object> parameters = null
        );
    }
}