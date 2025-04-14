namespace BlogSite.Helpers.Result
{
    public class Result : ResultBase
    {
        private Result(bool isSuccess, string message)
            : base(isSuccess, message) { }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string message) => new Result(false, message);
    }
}
