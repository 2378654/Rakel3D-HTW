using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    private float _rakelRotationZ;
    private float _rakelPositionX;
    public override void Update()
    {
        // float offset = _rakelRotationZ * -0.1f;
        //Value = (GameObject.Find("RenderedRakel").transform.localPosition.x) * 8f; //vor Ort - 0.2f
        
        //2 Trackers
        float top_x = GameObject.Find("TOP").transform.position.x;
        float bot_x = GameObject.Find("BOTTOM").transform.position.x;
        float pos_x = (top_x + bot_x) / 2;
        Value = pos_x * 8f;
    }
}