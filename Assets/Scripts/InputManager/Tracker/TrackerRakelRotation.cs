using Unity.XR.OpenVR;
using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    public override void Update()
    {
        float rotation = GameObject.Find("RenderedRakel").transform.eulerAngles.x;
        Value = (rotation);
    }
}
