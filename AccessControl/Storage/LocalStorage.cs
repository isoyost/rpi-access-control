namespace AccessControl.Storage;

public class LocalStorage : IStorage
{
    private readonly string _directory;
    
    public LocalStorage(string directory)
    {
        _directory = directory;
    }
    
    public string Put(byte[] file, string extension)
    {
        string path;
        do
        {
            var filename = Guid.NewGuid().ToString();
            path = $"{_directory}/{filename}.{extension}";
        } while (File.Exists(path));
        File.WriteAllBytes(path, file);

        return path;
    }
}