
namespace TraineeManagement.api.Repository.RabbitMQ
{
    public interface IRabbitMqPublisher
    {
        System.Threading.Tasks.Task PublishAsync<T>(
            string exchange, 
            string routingKey, 
            T message, 
            string correlationId
        );
    }
}
