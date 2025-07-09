using TMPro;
using UnityEngine;

public class TrackerRakelPressure : FloatValueSource
{
    private ButtonInteraction _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
    public override void Update()
    {
        Value = _interaction.GetPressure();
        Value = Mathf.Clamp01(Value);
        GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>().SetText(_interaction.GetPressure().ToString());
    }
}