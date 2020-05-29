using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSlotEvents : MonoBehaviour
{
    [SerializeField]
    GameManager gm;

    public void KeepCardOnSlot(Slot sl, Card c)
    {

    }

    public void FindSomeMoney(Card c, Slot sl)
    {
        c.wealth += 2;
        gm.AddMessageToLog(c.nam + " found some money in the " + sl.slotName + "!");
    }

    public void GainAuth(Card c, Slot sl)
    {
        c.auth += 2;
        gm.AddMessageToLog(c.nam + " learned something about themselves in the " + sl.slotName + ". Their Authority grows.");
    }

    public void GainUnruliness(Card c, Slot sl)
    {
        c.unruliness += 1;
        gm.AddMessageToLog(c.nam + " is annoyed. Their unruliness increases in the " + sl.slotName + ".");
    }


}
