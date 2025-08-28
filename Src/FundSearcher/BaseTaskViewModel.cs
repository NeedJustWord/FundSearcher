using System.Threading;
using System.Threading.Tasks;
using Fund.DataBase;
using FundSearcher.Consts;
using Prism.Events;
using Prism.Regions;

namespace FundSearcher
{
    class BaseTaskViewModel : BaseViewModel
    {
        public bool TaskIsCancel { get; private set; }

        private Task task;
        private CancellationTokenSource cts;
        private bool runTask;

        public BaseTaskViewModel(IRegionManager regionManager, string regionName, IEventAggregator eventAggregator = null, FundDataBase fundDataBase = null) : base(regionManager, regionName, eventAggregator, fundDataBase)
        {
            RegisterCommand(CommandName.CancelTask, Cancel);
        }

        public bool TryGetCancellationTokenFault(out CancellationToken token)
        {
            if (runTask)
            {
                token = CancellationToken.None;
                return true;
            }

            cts = new CancellationTokenSource();
            token = cts.Token;
            return false;
        }

        public void SetRunTask(Task task)
        {
            this.task = task;
            runTask = true;
            TaskIsCancel = false;
        }

        public void TaskCompleted()
        {
            task = null;
            runTask = false;
        }

        private void Cancel()
        {
            if (runTask)
            {
                TaskIsCancel = true;
                cts.Cancel();
                PublishStatusMessage("取消任务");
            }
            else
            {
                PublishStatusMessage("没有正在执行的任务");
            }
        }
    }
}
