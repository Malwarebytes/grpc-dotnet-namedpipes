using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcDotNetNamedPipes
{
    internal class Task2
    {
        private static Task s_completedTask;

        public static Task CompletedTask
        {
            get
            {
                var completedTask = s_completedTask;
                if (completedTask == null)
                    s_completedTask = Task.Delay(0);
                return completedTask;
            }
        }
        
        public static Task FromCanceled(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                throw new ArgumentOutOfRangeException("cancellationToken");
            }

            return Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
    
    internal class Task2<TResult>
    {
        public static Task<TResult> FromCanceled(CancellationToken _)
        {
            var tcs = new TaskCompletionSource<TResult>(TaskCreationOptions.None);
            tcs.SetCanceled();
            return tcs.Task;
        }
    }
}