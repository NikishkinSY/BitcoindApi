using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RestSharp.Extensions;

namespace Bitcoind.Service.HostServices
{
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts =
            new CancellationTokenSource();
        private CancellationTokenSource _continueLoop;
        private Object _continueLoopLock = new Object();
        protected IServiceScope _serviceScope;


        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it, 
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _continueLoop.Cancel();
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        public void ContinueLoop()
        {
            lock (_continueLoopLock)
            {
                _continueLoop?.Cancel();
            }
        }

        protected async Task Delay(int seconds)
        {
            try
            {
                lock (_continueLoopLock)
                {
                    _continueLoop = new CancellationTokenSource();
                }

                await Task.Delay(seconds * 1000, _continueLoop.Token);
            }
            catch (TaskCanceledException)
            {
            }
        }

        public virtual void Dispose()
        {
            _serviceScope?.Dispose();
            _continueLoop.Cancel();
            _stoppingCts.Cancel();
        }
    }
}
