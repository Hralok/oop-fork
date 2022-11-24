public class FoodPreference
{
    public ResourceTypeEnum foodType { get; }
    public int saturationFromOne { get; }
    public Effect effect { get; }

    public FoodPreference(ResourceTypeEnum foodType, int saturationFromOne, Effect effect)
    {
        this.foodType = foodType;
        this.saturationFromOne = saturationFromOne;
        this.effect = effect;
    }

}
