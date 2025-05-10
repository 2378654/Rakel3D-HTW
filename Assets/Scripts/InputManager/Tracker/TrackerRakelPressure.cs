using UnityEngine;

public class TrackerRakelPressure : FloatValueSource
{
    
    private ButtonInteraction _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
    
    public override void Update()
    {
        float pressure = _interaction.GetPressure();
        Value = pressure;
    }
}