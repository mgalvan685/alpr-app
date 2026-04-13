using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace alpr.api.Helpers;

public static class StopWatchHelper
{
    /// <summary>
    /// Resets the stopwatch and starts it.
    /// </summary>
    /// <param name="stopwatch">The stopwatch instance to stop and measure. Cannot be null.</param>
    public static void StartTimer(this Stopwatch stopwatch)
    {
        stopwatch.Restart();
        stopwatch.Start();
    }

    /// <summary>
    /// Stops the specified stopwatch and returns the elapsed time as a formatted string in milliseconds.
    /// </summary>
    /// <param name="stopwatch">The stopwatch instance to stop and measure. Cannot be null.</param>
    /// <returns>A string representing the elapsed time in milliseconds, formatted as "{elapsed} ms".</returns>
    public static string StopAndGetElapsed(this Stopwatch stopwatch)
    {
        stopwatch.Stop();
        return $"{stopwatch.ElapsedMilliseconds} ms";
    }
}
