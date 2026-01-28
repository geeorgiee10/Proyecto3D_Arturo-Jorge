public class TunedEffect : StatusEffect
{
    private int bonus;

    public TunedEffect(int bonusStrength)
    {
        bonus = bonusStrength;
    }

    protected override void OnApply()
    {
        owner.AddStrength(bonus);
    }

    protected override void OnExpire()
    {
        owner.AddStrength(-bonus);
    }
}
