using System.Device.Gpio;

namespace AccessControl.Signaler;

public class LedSignaler : ISignaler, IDisposable
{
    private const ushort GreenLedPin = 17;
    private const ushort YellowLedPin = 27;
    private const ushort RedLedPin = 22;
    private readonly GpioController _controller;

    public LedSignaler()
    {
        _controller = new GpioController();
        _controller.OpenPin(GreenLedPin, PinMode.Output);
        _controller.OpenPin(YellowLedPin, PinMode.Output);
        _controller.OpenPin(RedLedPin, PinMode.Output);
    }

    public void YellowOn() => LedOn(YellowLedPin);

    public void YellowOff() => LedOff(YellowLedPin);

    public void RedOn() => LedOn(RedLedPin);

    public void RedOff() => LedOff(RedLedPin);

    public void GreenOn() => LedOn(GreenLedPin);

    public void GreenOff() => LedOff(GreenLedPin);

    private void LedOn(ushort pin) => _controller.Write(pin, PinValue.High);

    private void LedOff(ushort pin) => _controller.Write(pin, PinValue.Low);

    public void Dispose()
    {
        GreenOff();
        RedOff();
        YellowOff();
    }

    ~LedSignaler() => Dispose();
}