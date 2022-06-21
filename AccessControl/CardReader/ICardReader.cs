namespace AccessControl.CardReader;

public interface ICardReader
{
    public delegate void CardReadHandler(string cardId);
    public void SetReadHandler(CardReadHandler handler);
    public void Listen();
}