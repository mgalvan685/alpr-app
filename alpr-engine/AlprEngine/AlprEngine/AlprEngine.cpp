#include <opencv2/opencv.hpp>
#include <string>
#include <cstring>

struct BoundingBox
{
    int x, y, width, height;
};

struct PlateDetection
{
	char plate[32];
    double timestampSeconds;
    int frameNumber;
	double confidence;
	BoundingBox box;
};

// Force the C++ struct to match the C# struct byte‑for‑byte
#pragma pack(push, 1)

struct NativeBoundingBox
{
    int x;
    int y;
    int width;
    int height;
};

struct NativePlateDetection
{
    char plate[32];          // Matches [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    double timestampSeconds; // Matches double
    int frameNumber;         // Matches int
    double confidence;       // Matches double
    NativeBoundingBox box;   // Matches nested struct
};

#pragma pack(pop)

extern "C" __declspec(dllexport)
int ProcessVideo(
    const char* videoPath,
    NativePlateDetection* detectionsOut,
    int maxDetections
);

int ProcessVideo(const char* videoPath, NativePlateDetection* detectionsOut, int maxDetections)
{
    int count = 0;

    if (maxDetections < 2)
        return 0;

    // Detection #1
    strncpy_s(detectionsOut[count].plate, "ABC123", 31);
    detectionsOut[count].plate[31] = '\0';

    detectionsOut[count].timestampSeconds = 1.5;
    detectionsOut[count].frameNumber = 42;
    detectionsOut[count].confidence = 0.92;

    detectionsOut[count].box.x = 100;
    detectionsOut[count].box.y = 120;
    detectionsOut[count].box.width = 200;
    detectionsOut[count].box.height = 80;

    count++;

    // Detection #2
    strncpy_s(detectionsOut[count].plate, "XYZ789", 31);
    detectionsOut[count].plate[31] = '\0';

    detectionsOut[count].timestampSeconds = 3.2;
    detectionsOut[count].frameNumber = 84;
    detectionsOut[count].confidence = 0.88;

    detectionsOut[count].box.x = 300;
    detectionsOut[count].box.y = 150;
    detectionsOut[count].box.width = 180;
    detectionsOut[count].box.height = 70;

    count++;

    return count;
}

extern "C" __declspec(dllexport)
int ProcessFrame(const char* imagePath, char* plateOut, char* stateOut, float* confidenceOut)
{
    // Load the image
    cv::Mat img = cv::imread(imagePath);
    if (img.empty())
        return 0;

    // TODO: Replace this with the real ALPR model
    std::string detectedPlate = "ABC123";
    std::string detectedState = "IL";
    float conf = 0.92f;

    // Copy results into output buffers
    strcpy_s(plateOut, 32, detectedPlate.c_str());
    strcpy_s(stateOut, 16, detectedState.c_str());
    *confidenceOut = conf;

    return 1; // 1 = found, 0 = not found
}