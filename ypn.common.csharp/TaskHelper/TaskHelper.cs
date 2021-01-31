using System;
using System.Threading;
using System.Threading.Tasks;

namespace ypn.common.csharp.TaskHelper
{
    /// <summary>
    /// 线程帮助类(处理单线程任务)
    /// </summary>
    public class TaskHelper
    {
        private static LimitedTaskScheduler _defaultScheduler = new LimitedTaskScheduler();

        /// <summary>
        /// 执行 
        /// 例：ThreadHelper.Run(() => { }, (ex) => { });
        /// </summary>
        /// <param name="doWork">在线程中执行</param>
        /// <param name="errorAction">错误处理</param>
        public static System.Threading.Tasks.Task RunToLimit(Action<object> doWork, object state, LimitedTaskScheduler scheduler = null, Action<Exception> errorAction = null)
        {
            if (scheduler == null) scheduler = _defaultScheduler;
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew((obj) =>
            {
                try
                {
                    if (doWork != null)
                    {
                        doWork(state);
                    }
                }
                catch (Exception ex)
                {
                    if (errorAction != null) errorAction(ex);
                }
            }, state,CancellationToken.None, TaskCreationOptions.None, scheduler);
            return task;
        }

        /// <summary>
        /// 执行 
        /// 例：ThreadHelper.Run(() => { }, (ex) => { });
        /// </summary>
        /// <param name="doWork">在线程中执行</param>
        /// <param name="errorAction">错误处理</param>
        public static System.Threading.Tasks.Task Run(Action doWork, LimitedTaskScheduler scheduler = null, Action<Exception> errorAction = null)
        {
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    if (doWork != null)
                    {
                        doWork();
                    }
                }
                catch (Exception ex)
                {
                    if (errorAction != null) errorAction(ex);
                }
            });
            return task;
        }

        /// <summary>
        /// 封装Dispatcher.BeginInvoke 
        /// 例：ThreadHelper.BeginInvoke(this.Dispatcher, () => { }, (ex) => { });
        /// </summary>
        /// <param name="errorAction">错误处理</param>
        //public static void BeginInvoke(Dispatcher dispatcher, Action action, Action<Exception> errorAction = null)
        //{
        //    dispatcher.InvokeAsync(new Action(() =>
        //    {
        //        try
        //        {
        //            DateTime dt = DateTime.Now;
        //            action();
        //            double d = DateTime.Now.Subtract(dt).TotalSeconds;
        //            if (d > 0.01) LoggerHelper.LogInfo("ThreadHelper.BeginInvoke UI耗时：" + d + "秒 " + action.Target.ToString());
        //        }
        //        catch (Exception ex)
        //        {
        //            if (errorAction != null) errorAction(ex);
        //            throw;
        //        }
        //    }), DispatcherPriority.Background);
        //}
    }
}
