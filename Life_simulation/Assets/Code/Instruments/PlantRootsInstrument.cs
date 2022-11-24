public class PlantRootsInstrument : Instrument
{
    public PlantRootsInstrument(TierEnum tier)
        :base(tier)
    {

    }

    public override int CalculationOfEstimatedResourceProduction(ResourceSourceTypeEnum sourceType, ResourceTypeEnum resourceType)
    {
        switch (sourceType)
        {
            case ResourceSourceTypeEnum.Underground:
                switch (resourceType)
                {
                    case ResourceTypeEnum.Mineral:
                        switch (tier)
                        {
                            case TierEnum.First:
                                return 10;
                            case TierEnum.Second:
                                return 20;
                            default:
                                return 50;
                        }
                    default:
                        return 0;
                }
            default:
                return 0;
        }
    }


}
