using System;

namespace BPlay.BHubPlay.Infrastructure.CrossCutting
{
    public static class Check
    {
        public static void IsNull<TException>(object anObject) where TException : Exception, new()
        {
            if (anObject is null)
            {
                Throw<TException>();
            }
        }

        public static void If<TException>(Func<bool> condition) where TException : Exception, new()
        {
            if (condition.Invoke())
            {
                Throw<TException>();
            }
        }

        private static void Throw<TException>() where TException : Exception, new()
        {
            var exception = (Exception)Activator.CreateInstance(typeof(TException));
            throw exception;
        }
    }
}
