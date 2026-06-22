namespace TraineeManagement.api.CustomException
{
    public class InvalidFileSubmission : Exception
    {
        public InvalidFileSubmission(string message) : base(message) { }
    }
}
