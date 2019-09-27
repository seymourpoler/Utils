using System;
using System.Threading.Tasks;

namespace BPlay.BHubPlay.Infrastructure.CrossCutting
{
    public static class ActionAsyncExecutor
    {
        public static async Task<bool> ExecuteWithRetryAsync<TException>(
            Func<Task> action,
            Action<TException> onFailure,
            int numberOfRetries = 3,
            int sleepBetweenRetriesInMilliseconds = 200) where TException : Exception
        {
            const int minimumNumberOfRetries = 0;

            Check.IsNull<ArgumentNullException>(action);
            Check.If<ArgumentException>(() => numberOfRetries < minimumNumberOfRetries);

            while (numberOfRetries >= minimumNumberOfRetries)
            {
                try
                {
                    await action.Invoke().ConfigureAwait(false);
                    return true;
                }
                catch (TException exception)
                {
                    if(onFailure != null)
                    {
                        onFailure.Invoke(exception);
                    }
                    numberOfRetries = numberOfRetries - 1;
                    await Task.Delay(sleepBetweenRetriesInMilliseconds).ConfigureAwait(false);
                }
            }

            return false;
        }
    }
}
