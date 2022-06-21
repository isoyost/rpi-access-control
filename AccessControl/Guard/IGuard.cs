namespace AccessControl.Guard;

public interface IGuard
{
    public bool Allows(string id, byte[] image);
}