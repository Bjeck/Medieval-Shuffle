using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantLands : SlotAddition
{
    public override void PostProcessOutcome()
    {
        
    }

    public override void PreProcessOutcome()
    {
        
    }

    public override void Setup()
    {
        Debug.Log("added setup " + slotBase.slotName);
        slotBase.possibleRandomEvents.Add(() => 
        {
            slotBase.gm.randomEvents.GainAuth(slotBase.cardOnMe, slotBase);
        });
        slotBase.possibleRandomEvents.Add(() => 
        {
            slotBase.gm.randomEvents.GainUnruliness(slotBase.cardOnMe, slotBase);
        });
        slotBase.possibleRandomEvents.Add(() => 
        {
            slotBase.gm.randomEvents.FindSomeMoney(slotBase.cardOnMe, slotBase);
        });
    }
}
