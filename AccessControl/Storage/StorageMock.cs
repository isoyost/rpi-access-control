namespace AccessControl.Storage;

public class StorageMock : IStorage
{
    public string Put(byte[] file, string extension)
    {
        return $"/some/path/on/drive.{extension}";
    }
}