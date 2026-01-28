public class SilencedEffect : StatusEffect
{
    protected override void OnTurnStartEffect()
    {
        owner.Silence(true);
    }

    protected override void OnExpire()
    {
        owner.Silence(false);
    }
}
