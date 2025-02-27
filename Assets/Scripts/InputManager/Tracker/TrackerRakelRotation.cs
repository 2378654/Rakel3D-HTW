using Unity.XR.OpenVR;
using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    public override void Update()
    {
        Value = GameObject.Find("RenderedRakel").transform.eulerAngles.z; // --> FIXED THE ROTATION PROBLEM
    }
}
