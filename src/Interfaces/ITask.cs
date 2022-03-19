public interface ITask
{
    bool CanExecute();

    bool WhenCannotExecute();

    bool Execute();
}