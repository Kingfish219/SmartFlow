
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core
{
    internal interface IProcessStepManager
    {
        ProcessStep GetActiveProcessStep(Guid userId, Entity entity);
        ProcessStep InitializeActiveProcessStep(Guid userId, Entity entity, bool initializeFromFirstState);
        ProcessResult FinalizeActiveProcessStep(ProcessStep processStep);
    }
}