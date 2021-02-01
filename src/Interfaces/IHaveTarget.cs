public interface IHaveTarget
{
    bool CanTarget(ITarget target);

    void SetLeftTarget(ITarget target);

    void SetRightTarget(ITarget target);
    void ClearTargets();
}