using UnityEngine;

public class TrackerRakelTilt : FloatValueSource
{
    public override void Update()
    {
        //not really working right now
        Value = GameObject.Find("RenderedRakel").transform.rotation.y * (128); 
    }
}