public class StatUpgradeEffect : UpgradeEffect
{
    public float StatIncrease { get; }

    public StatUpgradeEffect(float statIncrease)
    {
        StatIncrease = statIncrease;
    }

    public override void Apply()
    {
        
    }
}