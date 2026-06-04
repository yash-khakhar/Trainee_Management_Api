using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TraineeManagement.api.repository;

namespace TraineeManagement.api.models
{
    public class TraineeModel: ITraineeRepo
    {
     
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string TechStack { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
