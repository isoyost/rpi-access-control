namespace AccessControl.Camera;

public class CameraMock : ICamera
{
    public byte[] GetImage() => new byte[]{1, 2, 3, 4};
}