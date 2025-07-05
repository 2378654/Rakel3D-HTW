using UnityEngine;
using TMPro;
public class TrackerRakelTilt : FloatValueSource
{
    private TextMeshProUGUI _text = GameObject.Find("TiltText").GetComponent<TextMeshProUGUI>();
    public override void Update()
    {
        //only get positive Values
        Value = GameObject.Find("RenderedRakel").transform.eulerAngles.y - 180;

        Value = Rakel.ClampTilt(Value);
        _text.SetText(Value.ToString());
    }
}