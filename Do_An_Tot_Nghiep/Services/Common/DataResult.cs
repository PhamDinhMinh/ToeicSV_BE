
#nullable disable

using System.Net;

namespace NewProject.Services.Common
{
  public class DataResult : IDataResult
  {
    public string Message { get; set; }

    public string Error { get; set; }

    public bool Success { get; set; } = true;

    public object Data { get; set; }

    public int? TotalRecords { get; set; }

    public int? StatusCode { get; set; }

    public static DataResult ResultSuccess(
      object data,
      string message,
      int totalReCords)
    {
      return new DataResult()
      {
        Data = data,
        Message = message,
        Success = true,
        TotalRecords = new int?(totalReCords)
      };
    }
    public static DataResult ResultSuccess(
      object? data,
      string message
      )
    {
      return new DataResult()
      {
        Data = data,
        Message = message,
        Success = true,
      };
    }

    public static DataResult ResultError(string err, string message)
    {
      return new DataResult()
      {
        Data = null,
        Error = err,
        Message = message,
        Success = false
      };
    }

    public static DataResult ResultSuccess(string message)
    {
      return new DataResult()
      {
        Message = message,
        Success = true
      };
    }

    public static DataResult ResultFail(string message, int? result_code = (int)HttpStatusCode.InternalServerError)
    {
      return new DataResult()
      {
        Message = message,
        Success = false,
        StatusCode = result_code
      };
    }

    public static DataResult ResultCode(
      object data,
      string message,
      int result_code)
    {
      return new DataResult()
      {
        Data = data,
        Message = message,
        Success = result_code == 200,
        StatusCode = new int?(result_code)
      };
    }
  }
}
