namespace TraineeManagement.api.CustomException
{
    public class RabbitMqException : Exception
    {
        public RabbitMqException(string message) : base(message) { }
    }
}
