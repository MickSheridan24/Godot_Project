
public class RemoveStatusOrder : IOrder
{
    private ISufferStatusEffects effected;
    private eStatusEffect effect;

    public RemoveStatusOrder(ISufferStatusEffects effected, eStatusEffect effect)
    {
        this.effected = effected;
        this.effect = effect;
    }

    public bool Execute()
    {
        effected.RemoveEffect(effect);
        return true;
    }


}