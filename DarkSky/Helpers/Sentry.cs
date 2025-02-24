using Sentry;
using System;

namespace DarkSky.Helpers
{
    /// <summary>
    /// Sentry is a service that helps you monitor and fix crashes in real time.
    /// </summary>
    /// <remarks>
    /// The SDKs automatically capture all unhandled exceptions and report them to Sentry.
    /// </remarks>
    public static class Sentry
    {
        /// <summary>
        /// Initializes the Sentry SDK.
        /// </summary>
        /// <remarks>
        /// This method is called in the <see cref="App.App"/> constructor.
        /// </remarks>
        public static void Init()
        {
            SentrySdk.Init(options =>
            {
                // DO NOT REMOVE THIS LINE
                // THIS IS NOT A SECRET KEY AND IS NOT A SECURITY VULNERABILITY HAVING IT IN CODE
                // YOU WILL BREAK ALL ERROR REPORTING IF YOU REMOVE THIS LINE
                options.Dsn = "https://aaed7c44c79880e43658b5ccf9bf9e9f@o4508502512828416.ingest.de.sentry.io/4508624248176720";

                options.AutoSessionTracking = true;

                // Set TracesSampleRate to 1.0 to capture 100%
                // of transactions for tracing.
                // We recommend adjusting this value in production.
                options.TracesSampleRate = 0.6;

                // Sample rate for profiling, applied on top of othe TracesSampleRate,
                // e.g. 0.2 means we want to profile 20 % of the captured transactions.
                // We recommend adjusting this value in production.
                options.ProfilesSampleRate = 0.2;

                /* This doesn't work on UAP iirc
                // Requires NuGet package: Sentry.Profiling
                // Note: By default, the profiler is initialized asynchronously. This can
                // be tuned by passing a desired initialization timeout to the constructor.
                options.AddIntegration(new ProfilingIntegration(
                    TimeSpan.FromMilliseconds(500)
                )); */
            });
        }
    }
}