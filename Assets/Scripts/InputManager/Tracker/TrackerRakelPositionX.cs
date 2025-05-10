using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    private float _rakelRotationZ;
    private float _rakelPositionX;
    public override void Update()
    {
        // float offset = _rakelRotationZ * -0.1f;
        Value = (GameObject.Find("RenderedRakel").transform.localPosition.x) * 8f; //vor Ort - 0.2f
    }
}