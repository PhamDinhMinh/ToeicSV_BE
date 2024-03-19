namespace NewProject.Services.Common;

public interface IDataResult
{
    string Message { get; set; }

    string Error { get; set; }

    bool Success { get; set; }

    object Data { get; set; }
}