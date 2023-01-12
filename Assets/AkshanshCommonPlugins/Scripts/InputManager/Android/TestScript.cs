using AkshanshKanojia.Inputs.Mobile;
public class TestScript : MobileInputs
{
    public override void OnTapEnd(MobileInputManager.TouchData _data)
    {

    }

    public override void OnTapMove(MobileInputManager.TouchData _data)
    {
    }

    public override void OnTapped(MobileInputManager.TouchData _data)
    {
        print("tapped at " + _data.TouchPosition);
    }

    public override void OnTapStay(MobileInputManager.TouchData _data)
    {

    }
}
