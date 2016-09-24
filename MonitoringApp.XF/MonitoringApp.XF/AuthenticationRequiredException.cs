using System;

namespace MonitoringApp.XF
{
    public class AuthenticationRequiredException : Exception
    {
        public Type RequestingService { get; private set; }

        public AuthenticationRequiredException(Type requestingService)
        {
            RequestingService = requestingService;
        }
    }
}
