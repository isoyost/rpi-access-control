using PCSC;
using PCSC.Exceptions;
using PCSC.Iso7816;

namespace AccessControl.CardReader;

public class AcrCardReader : ICardReader
{
    private ICardReader.CardReadHandler? _cardReadHandler;
    private const string ReadeName = "ACS ACR1222 Dual Reader 00 01";

    public void SetReadHandler(ICardReader.CardReadHandler handler)
    {
        _cardReadHandler = handler;
    }

    public void Listen()
    {
        var contextFactory = ContextFactory.Instance;

        using var context = contextFactory.Establish(SCardScope.System);

        if (_cardReadHandler == null)
        {
            throw new Exception("Read handler is not set.");
        }

        while (true)
        {
            try
            {
                using var isoReader =
                    new IsoReader(context, ReadeName, SCardShareMode.Shared, SCardProtocol.Any, false);
                var apdu = new CommandApdu(IsoCase.Case2Short, isoReader.ActiveProtocol)
                {
                    CLA = 0xFF,
                    Instruction = InstructionCode.GetData,
                    P1 = 0x00,
                    P2 = 0x00,
                    Le = 0x00
                };

                var response = isoReader.Transmit(apdu);
                var status = response.StatusWord.ToString("X");

                if (status != "9000") continue;
                
                var uuid = response.GetData().Aggregate("", (current, b) => current + b.ToString("X"));
                _cardReadHandler(uuid);
            }
            catch (PCSCException)
            {
                Thread.Sleep(50);
            }
        }
    }
}