using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.TaskAssignmentDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Models;
using TraineeManagement.api.Repository.TaskAssignment;

namespace TraineeManagement.api.Services
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private AppDbContext _context;

        public TaskAssignmentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<TaskAssignmentResponse> AddTaskAssignment(CreateTaskAssignmentRequest taskAssignment)
        {

            if(taskAssignment.DueDate < taskAssignment.AssignedDate)
            {
                throw new Exception("Due Date cannot be before Assigned Date!");
            }

            TaskAssignmentModel taskAssignmentModel = new TaskAssignmentModel(
                     taskAssignment.TraineeId,
                     taskAssignment.MentorId,
                     taskAssignment.TaskId,
                     taskAssignment.AssignedDate,
                     taskAssignment.DueDate,
                     taskAssignment.Status,
                     taskAssignment.Remarks
                 );


            _context.TaskAssignment.Add(taskAssignmentModel);

            await _context.SaveChangesAsync();

            return TaskAssignmentModel.ToDto(taskAssignmentModel);
        }

        public async Task<TaskAssignmentResponse> GetTaskAssignmentById(int id)
        {
            TaskAssignmentResponse? taskAssignment = await _context.TaskAssignment
                 .Where(t => t.Id == id)
                 .Select(t => new TaskAssignmentResponse(t.Id, t.TraineeId, t.MentorId, t.TaskId, t.AssignedDate, t.DueDate, t.Status, t.Remarks))
                 .FirstOrDefaultAsync();

            if (taskAssignment == null) throw new NotFoundException("Task Assignment Not Found");

            return taskAssignment;
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> GetTaskAssignmentList()
        {
            List<TaskAssignmentResponse> taskAssignmentList = await _context.TaskAssignment
              .Select(t => new TaskAssignmentResponse(t.Id, t.TraineeId, t.MentorId, t.TaskId, t.AssignedDate, t.DueDate, t.Status, t.Remarks))
              .ToListAsync();

            return taskAssignmentList;
        }

        public async Task<TaskAssignmentResponse> UpdateTaskAssignment(int id, UpdateTaskAssignmentRequest taskAssignment)
        {
            TaskAssignmentModel? task = await _context.TaskAssignment.FindAsync(id);
            if (task == null) throw new NotFoundException("Task Assignment Not Found");

            if (taskAssignment.TraineeId != null) task.TraineeId = (int)taskAssignment.TraineeId;
            if (taskAssignment.MentorId != null) task.MentorId = (int)taskAssignment.MentorId;
            if (taskAssignment.TaskId != null) task.TaskId = (int)taskAssignment.TaskId;
            if (taskAssignment.AssignedDate != null) task.AssignedDate = (DateTime)taskAssignment.AssignedDate;
            if (taskAssignment.DueDate != null) task.DueDate = (DateTime)taskAssignment.DueDate;
            if (taskAssignment.Status != null) task.Status = (TaskAssignmentStatusEnum)taskAssignment.Status;
            if (taskAssignment.Remarks != null) task.Remarks = taskAssignment.Remarks;

            await _context.SaveChangesAsync();

            return TaskAssignmentModel.ToDto(task);
        }
    }
}
