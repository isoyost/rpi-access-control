namespace AccessControl.Signaler;

public interface ISignaler
{
    public void YellowOn();
    public void YellowOff();
    public void RedOn();
    public void RedOff();
    public void GreenOn();
    public void GreenOff();
}