using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotAddition : MonoBehaviour
{

    public Slot slotBase;

    public abstract void PreProcessOutcome();

    public abstract void PostProcessOutcome();
}
