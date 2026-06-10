namespace TraineeManagement.api.CustomException
{
    public class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException(string message) : base(message) { }
    }
}
