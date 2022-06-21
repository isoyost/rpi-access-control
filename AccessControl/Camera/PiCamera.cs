using Unosquare.RaspberryIO;

namespace AccessControl.Camera;

public class PiCamera : ICamera
{
    public byte[] GetImage() => Pi.Camera.CaptureImageJpeg(640, 480);
}