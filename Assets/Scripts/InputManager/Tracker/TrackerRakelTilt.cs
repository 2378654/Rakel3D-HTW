using UnityEngine;
using TMPro;
public class TrackerRakelTilt : FloatValueSource
{
    private TextMeshProUGUI _text = GameObject.Find("TiltText").GetComponent<TextMeshProUGUI>();
    private GameObject _top = GameObject.Find("Top");
    private GameObject _bot = GameObject.Find("Bottom");
    public override void Update()
    {
        //only get positive Values
        Value = (_top.transform.eulerAngles.y - 180 + (_bot.transform.eulerAngles.y - 180))/2;
        Value -= 15;
        Value = Rakel.ClampTilt(Value);
        _text.SetText(Value.ToString());
    }
}