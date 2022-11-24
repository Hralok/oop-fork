public class MineralResourceSource : ResourceSource
{
    public MineralResourceSource(int newCount)
        : base(newCount, ResourceTypeEnum.Mineral)
    { 
        bestSuitableInstruments.Add(InstrumentTypeEnum.PlantRoots);
    }
}
