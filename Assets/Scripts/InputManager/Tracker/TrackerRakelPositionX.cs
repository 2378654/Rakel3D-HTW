using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    
    public override void Update()
    {
        //Value = (GameObject.Find("RenderedRakel").transform.position.x);
        Value = (GameObject.Find("RenderedRakel").transform.position.x + 0.06f) * 15f; //Offset und Mult wenn Tracker an oberer Ecke
    }
}