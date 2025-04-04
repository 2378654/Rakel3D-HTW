using TMPro;
using Unity.XR.OpenVR;
using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    private float _rotation;
 
    public override void Update()
    {
        Vector3 world_up = Vector3.up;
        Vector3 rakel_up = GameObject.Find("RenderedRakel").transform.up;
        float rotationZ = Vector3.SignedAngle(rakel_up, world_up, Vector3.forward);
        Value = rotationZ;
    }
}