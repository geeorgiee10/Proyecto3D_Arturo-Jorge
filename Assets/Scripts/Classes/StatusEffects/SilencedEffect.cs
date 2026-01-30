public class SilencedEffect : StatusEffect
{
    public SilencedEffect(int dur)
    {
        remainingTurns = dur;
    }
    
    protected override void OnTurnStartEffect()
    {
        owner.Silence(true);
    }

    protected override void OnExpire()
    {
        owner.Silence(false);
    }
}
