using System.Runtime.InteropServices;
using System.Text;

namespace alpr.api.Services;

public static class AlprEngineNative
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NativeBoundingBox
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NativePlateDetection
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Plate;

        public double TimestampSeconds;
        public int FrameNumber;
        public double Confidence;
        public NativeBoundingBox BoundingBox;
    }

    [DllImport("AlprEngine.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ProcessFrame(
        string imagePath,
        StringBuilder plateOut,
        StringBuilder stateOut,
        out float confidenceOut
    );

    [DllImport("AlprEngine.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ProcessVideo(
        string videoPath,
        [Out] NativePlateDetection[] detectionsOut,
        int maxDetections
    );
}