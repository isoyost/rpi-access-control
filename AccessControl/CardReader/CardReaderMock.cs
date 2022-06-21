namespace AccessControl.CardReader;

public class CardReaderMock : ICardReader
{
    private ICardReader.CardReadHandler? _cardReadHandler;
    
    public void SetReadHandler(ICardReader.CardReadHandler handler)
    {
        _cardReadHandler = handler;
    }

    public void Listen()
    {
        if (_cardReadHandler == null)
        {
            throw new Exception("Read handler is not set.");
        }
        while (true)
        {
            Thread.Sleep(5000);
            _cardReadHandler("1234");
        }
    }
}