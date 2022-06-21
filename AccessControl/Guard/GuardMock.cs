namespace AccessControl.Guard;

public class GuardMock : IGuard
{
    private readonly bool _shouldAllowAll;
    
    public GuardMock(bool shouldAllowAll = true)
    {
        Thread.Sleep(1000);
        _shouldAllowAll = shouldAllowAll;
    }
    
    public bool Allows(string id, byte[] image) => _shouldAllowAll;
}