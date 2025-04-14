namespace BlogSite.Helpers.Result
{
    public abstract class ResultBase
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        protected ResultBase(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
