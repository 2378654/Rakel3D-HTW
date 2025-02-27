using Unity.XR.OpenVR;
using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    public override void Update()
    {
        //float rotation = GameObject.Find("RenderedRakel").transform.eulerAngles.x;
        //Value = (rotation);
        //Value = (GameObject.Find("RotationValue").transform.rotation.x * 140) - 180;
        Value = (GameObject.Find("RotationValue").transform.eulerAngles.z);
    }
}
