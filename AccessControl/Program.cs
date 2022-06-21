using AccessControl.Camera;
using AccessControl.CardReader;
using AccessControl.Guard;
using AccessControl.Signaler;

namespace AccessControl;

internal static class Program
{
    private static readonly IGuard Guard = new AzureGuard("https://cdv-entry-checker-rpi.azurewebsites.net", "MucYhSo9ZhwCIWEl7OMYjwu9uM8dAQfE7okWNXWoQfdNAzFuPfDh3w==");
    private static readonly ISignaler Signaler = new LedSignaler();
    private static readonly ICardReader Reader = new AcrCardReader();
    private static readonly ICamera Camera = new PiCamera();
    private const int LightLength = 600;
    private const int BreakLength = 1200;
    
    public static void Main()
    {
        Log("Started");
        Reader.SetReadHandler(CardReadHandler);
        Console.CancelKeyPress += KeyBoardInterruptHandler;

        try
        {
            Reader.Listen();
        }
        catch (Exception)
        {
            Shutdown();
            throw;
        }
    }

    private static void CardReadHandler(string cardId)
    {
        Log(cardId, "Connected");
        Signaler.YellowOn();
        var image = Camera.GetImage();
        Log(cardId, "Image captured");
        var isCardAllowed = Guard.Allows(cardId, image);
        Signaler.YellowOff();
        if (isCardAllowed)
        {
            Log(cardId, "Access allowed");
            Signaler.GreenOn();
            Thread.Sleep(LightLength);
            Signaler.GreenOff();
        }
        else
        {
            Log(cardId, "Access denied");
            Signaler.RedOn();
            Thread.Sleep(LightLength);
            Signaler.RedOff();
        }
        Thread.Sleep(BreakLength);
    }

    private static void Log(string message)
    {
        Console.WriteLine($"[{DateTime.UtcNow}] System: {message}");
    }

    private static void Log(string cardId, string message)
    {
        Console.WriteLine($"[{DateTime.UtcNow}] {cardId}: {message}");
    }

    private static void KeyBoardInterruptHandler(object? sender, ConsoleCancelEventArgs args)
    {
        Shutdown();
    }

    private static void Shutdown()
    {
        if (Signaler is IDisposable signaler)
        {
            signaler.Dispose();
        }
        Log("Shutdown");
    }
}