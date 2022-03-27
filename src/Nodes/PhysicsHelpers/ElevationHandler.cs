
using System;
using System.Collections.Generic;

public class ElevationHandler
{
    public IElevatable subject { get; set; }
    public eCollisionLayers Level { get; set; }
    private List<eCollisionLayers> AllLevels;
    private Runtime runtime;

    public ElevationHandler(IElevatable sub, Runtime runtime, eCollisionLayers startingLevel = eCollisionLayers.LEVEL3)
    {

        this.subject = sub;
        this.Level = startingLevel;
        this.runtime = runtime;
        AllLevels = new List<eCollisionLayers>()
        {
            eCollisionLayers.LEVEL1, eCollisionLayers.LEVEL2, eCollisionLayers.LEVEL3, eCollisionLayers.LEVEL4, eCollisionLayers.LEVEL5
        };
        SetElevationLayers();
    }

    private void SetElevationLayers()
    {
        var currentLevel = AllLevels.Find(l => l == Level);
        subject.SetCollisionLayerBit((int)currentLevel, true);
        subject.SetCollisionMaskBit((int)currentLevel, false);
        foreach (var level in AllLevels)
        {
            if (level != currentLevel && (int)level > (int)currentLevel)
            {
                subject.SetCollisionLayerBit((int)level, false);
                subject.SetCollisionMaskBit((int)level, true);
            }
            else
            {
                subject.SetCollisionLayerBit((int)level, true);
                subject.SetCollisionMaskBit((int)level, false);
            }
        }
    }

    public void HandleElevation(int customLevel = -1)
    {
        var currentElev = customLevel == -1 ? runtime.World.GetElevation(subject.GlobalPosition)
                                            : (eCollisionLayers)customLevel;

        if (Level != currentElev && !subject.isFallDisabled)
        {
            Level = currentElev;
            SetElevationLayers();
        }

    }
}
