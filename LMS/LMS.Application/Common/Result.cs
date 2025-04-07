namespace LMS.Application.common
{
    public class Result<TEntity> where TEntity : class
    {
        public bool IsSuccess { get; }
        public string Message { get; set; }
        public TEntity? Entity { get; }

        private Result(bool isSuccess, string message, TEntity entity)
        {
            IsSuccess = isSuccess;
            Message = message;
            Entity = entity;
        }

        private Result(string error) 
        {
            IsSuccess = false;
            Message = error;
            Entity = null;
        }

        public static Result<TEntity> Success(string SuccessMessage, TEntity entity) => new Result<TEntity>(true, SuccessMessage, entity);
        public static Result<TEntity> Failure(string errorMessage) => new Result<TEntity>(errorMessage);
    } 
}
