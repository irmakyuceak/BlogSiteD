namespace BlogSite.Helpers.Result
{
    public class DataResult<T> : ResultBase
    {
        public T Data { get; private set; }

        private DataResult(bool isSuccess, string message, T data)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static DataResult<T> Success(T data) => new DataResult<T>(true, string.Empty, data);
        public static DataResult<T> Failure(string message) => new DataResult<T>(false, message, default);
    }
}
