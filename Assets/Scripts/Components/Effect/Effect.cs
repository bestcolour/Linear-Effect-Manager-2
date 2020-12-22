namespace LinearEffects
{
    [System.Serializable]
    public abstract class Effect { }

    public abstract class UpdateEffect : Effect
    {
        public bool HaltUntilFinished = false;
    }
}