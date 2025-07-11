using TMPro;
using UnityEngine;

public class TrackerRakelPressure : FloatValueSource
{
    private ButtonInteraction _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
    private TextMeshProUGUI _pressureText = GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>();
    public override void Update()
    {
        Value = _interaction.GetPressure();
        Value = Mathf.Clamp01(Value);
        _pressureText.SetText(_interaction.GetPressure().ToString());
    }
}