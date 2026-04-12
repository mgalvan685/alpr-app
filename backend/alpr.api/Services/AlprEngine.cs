using System.Runtime.InteropServices;
using System.Text;

namespace alpr.api.Services;

public static class AlprEngine
{
    [DllImport("AlprEngine.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern int ProcessFrame(
        string imagePath,
        StringBuilder plateOut,
        StringBuilder stateOut,
        out float confidenceOut
    );

    public static (bool found, string plate, string state, float confidence) Analyze(string framePath)
    {
        var plate = new StringBuilder(32);
        var state = new StringBuilder(16);

        float confidence;

        int result = ProcessFrame(framePath, plate, state, out confidence);

        if (result == 0)
            return (false, "", "", 0);

        return (true, plate.ToString(), state.ToString(), confidence);
    }
}