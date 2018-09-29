using FluentValidation;

namespace UserTasks.ViewModels
{
    public class TaskItemViewModel
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool IsDone { get; set; }
        public int UserOwnerId { get; set; }
    }

    public class TaskItemViewModelValidator : AbstractValidator<TaskItemViewModel>
    {
        public TaskItemViewModelValidator()
        {
            RuleFor(t => t.Task).NotEmpty().WithMessage("Task cannot be empty");
        }
    }
}
