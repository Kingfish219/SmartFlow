using SmartFlow.Core.Db;
using SmartFlow.Core.Handlers;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System.Collections.Generic;

namespace SmartFlow.Core
{
    internal static class WorkflowHandlerFactory
    {
        internal static Queue<WorkflowHandler> BuildHandlers(
            ProcessStepContext processStepContext,
            IProcessRepository processRepository,
            IProcessStepManager processStepManager,
            IEntityRepository entityRepository,
            LogRepository logRepository,
            string connectionString
            )
        {

            return BuildDefaultHandlers(processStepContext, processRepository, processStepManager, entityRepository, logRepository, connectionString);
        }

        private static Queue<WorkflowHandler> BuildDefaultHandlers(
             ProcessStepContext processStepContext
            , IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , IEntityRepository entityRepository
            , LogRepository logRepository
            , string connectionString)
        {
            var actionHandler = new ActionHandler(processRepository, processStepManager, processStepContext);
            var actionActivityHandler = new ActionActivityHandler(processRepository, processStepManager, processStepContext);
            var transitionHandler = new TransitionHandler(processRepository, processStepManager, processStepContext, entityRepository);
            var transitionActivityHandler = new TransitionActivityHandler(processRepository, processStepManager, processStepContext);
            var processStepFinalizerHandler = new ProcessStepFinalizerHandler(processRepository, processStepManager, processStepContext);
            var stateActivityHandler = new StateActivityHandler(processRepository, processStepManager, processStepContext);

            actionHandler.SetNextHandler(actionActivityHandler);

            actionActivityHandler.SetNextHandler(transitionHandler);
            actionActivityHandler.SetPreviousHandler(actionHandler);

            transitionHandler.SetNextHandler(transitionActivityHandler);
            transitionHandler.SetPreviousHandler(actionActivityHandler);

            transitionActivityHandler.SetNextHandler(processStepFinalizerHandler);
            transitionActivityHandler.SetPreviousHandler(transitionHandler);

            processStepFinalizerHandler.SetNextHandler(stateActivityHandler);
            processStepFinalizerHandler.SetPreviousHandler(transitionActivityHandler);

            var queue = new Queue<WorkflowHandler>();

            if (processStepContext.EntityCommandType == EntityCommandType.Create)
            {
                var entityCommandHandler = new EntityHandler(processRepository, processStepManager, processStepContext, entityRepository.Create);
                entityCommandHandler.SetNextHandler(actionHandler);
                actionHandler.SetPreviousHandler(entityCommandHandler);
                queue.Enqueue(entityCommandHandler);
            }

            queue.Enqueue(actionHandler);
            queue.Enqueue(actionActivityHandler);
            queue.Enqueue(transitionHandler);
            queue.Enqueue(transitionActivityHandler);
            queue.Enqueue(processStepFinalizerHandler);
            queue.Enqueue(stateActivityHandler);

            return queue;
        }
    }
}
