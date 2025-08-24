namespace GrowDay.Domain.Helpers
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static Result SuccessResult(string message = "")
        {
            return new Result { Success = true, Message = message };
        }

        public static Result FailureResult(string message)
        {
            return new Result { Success = false, Message = message };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> SuccessResult(T data, string message = "")
        {
            return new Result<T> { Success = true, Data = data, Message = message };
        }

        public static Result<T> FailureResult(string message)
        {
            return new Result<T> { Success = false, Message = message };
        }
    }
}
