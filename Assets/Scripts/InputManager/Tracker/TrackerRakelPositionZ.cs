
using UnityEngine;

public class TrackerRakelPositionZ : FloatValueSource
{
    public override void Update()
    {
        //Don't use it like that.
        //There is a implemented Script to automatically calculate the Z-Coordinate (look at AutoRakelPositionZ.cs by Brzoska (2023))
        //Value = GameObject.Find("RenderedRakel").transform.position.z;
    }
}
