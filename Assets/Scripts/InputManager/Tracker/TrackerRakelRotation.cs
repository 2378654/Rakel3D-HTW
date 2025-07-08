using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    private float _rotation;
 
    public override void Update()
    {
        Vector3 world_up = Vector3.up;
        Vector3 rakel_up = (GameObject.Find("Top").transform.up + GameObject.Find("Bottom").transform.up)/2;
        float rotationZ = Vector3.SignedAngle(rakel_up, world_up, Vector3.forward);
        Value = rotationZ;
    }
}