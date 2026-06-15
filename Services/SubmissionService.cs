using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.SubmissionDto;
using TraineeManagement.api.Models;
using TraineeManagement.api.Repository.Submission;
using TraineeManagement.api.Repository.TaskAssignment;

namespace TraineeManagement.api.Services
{
    public class SubmissionService : ISubmissionService
    {
        private AppDbContext _context;

        public SubmissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SubmissionResponse> AddSubmission(CreateSubmissionRequest submissionRequest)
        {

            DateTime dueDate = await _context.TaskAssignment
                .Where(t => t.Id == submissionRequest.TaskAssignmentId)
                .Select(t => t.DueDate).FirstOrDefaultAsync();

            if (submissionRequest.SubmittedDate > dueDate) throw new Exception("You cannot submit task after due date!");

            SubmissionModel submissionModel = new SubmissionModel(
                     submissionRequest.TaskAssignmentId,
                     submissionRequest.SubmissionUrl,
                     submissionRequest.Notes,
                     submissionRequest.SubmittedDate,
                     submissionRequest.Status
                 );


            _context.Submission.Add(submissionModel);

            await _context.SaveChangesAsync();

            return SubmissionModel.ToDto(submissionModel);
        }

        public async Task<SubmissionResponse> GetSubmissionById(int id)
        {
            SubmissionResponse? submissionResponse = await _context.Submission
                .Where(s => s.Id == id)
                .Select(s => new SubmissionResponse(
                    s.Id, 
                    s.TaskAssignmentId, 
                    s.SubmissionUrl, 
                    s.Notes, 
                    s.SubmittedDate, 
                    s.Status
                ))
                .FirstOrDefaultAsync();

            if (submissionResponse == null) throw new NotFoundException("Submission Not Found");

            return submissionResponse;
        }

        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionList()
        {
            List<SubmissionResponse> submissionResponse = await _context.Submission
                .Select(s => new SubmissionResponse(
                    s.Id, 
                    s.TaskAssignmentId, 
                    s.SubmissionUrl, 
                    s.Notes, 
                    s.SubmittedDate, 
                    s.Status
                ))
                .ToListAsync();

            return submissionResponse;
        }
    }
}
