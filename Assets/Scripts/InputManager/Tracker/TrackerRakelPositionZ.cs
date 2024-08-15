
using UnityEngine;

public class TrackerRakelPositionZ : FloatValueSource
{
    public override void Update()
    {
        Value = GameObject.Find("RenderedRakel").transform.position.z;
    }
}
