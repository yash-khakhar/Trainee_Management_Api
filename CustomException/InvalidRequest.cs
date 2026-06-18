namespace TraineeManagement.api.CustomException
{
    public class InvalidRequest : Exception
    {
        public InvalidRequest(string message) : base(message) { }
    }
}
