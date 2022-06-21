namespace AccessControl.Storage;

public interface IStorage
{
    public string Put(byte[] file, string extension);
}