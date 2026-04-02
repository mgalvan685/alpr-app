using alpr.api.Services.Models;

namespace alpr.api.Services.Interfaces;

public interface IAlprEngine
{
    Task<AlprResult> ProcessVideoAsync(string filePath);
}