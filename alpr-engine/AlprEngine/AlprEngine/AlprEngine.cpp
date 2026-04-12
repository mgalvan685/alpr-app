#include <opencv2/opencv.hpp>
#include <string>
#include <cstring>

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