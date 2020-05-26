using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : SlotAddition
{
    public override void PostProcessOutcome()
    {
        if (slotBase.cardOnMe == null)
        {
            int mon = Random.Range(1, 4);
            if (slotBase.weatherActive)
            {
                mon *= 2;
            }

            slotBase.moneyToGenerate = mon;

            if (slotBase.moneyGenerator)
            {
                slotBase.availableMoney += slotBase.moneyToGenerate;
            }
        }
    }

    public override void PreProcessOutcome()
    {

    }
    
}
