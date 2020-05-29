using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Village : SlotAddition
{
    Slot farm;

    private void Start()
    {
        farm = slotBase.gm.slots.Find(x => x.slotName == "Farm");
    }

    public override void PostProcessOutcome()
    {
        if(farm.cardOnMe == null)
        {
            slotBase.availableMoney += farm.availableMoney + 1;
            farm.availableMoney = 0;
        }
    }

    public override void PreProcessOutcome()
    {
        
    }

    public override void Setup()
    {

    }
}
