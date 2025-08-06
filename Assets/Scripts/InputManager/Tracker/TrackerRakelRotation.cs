using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    private float _rotation;
    private GameObject _topUp = GameObject.Find("Top");
    private GameObject _botUp = GameObject.Find("Bottom");
    public override void Update()
    {
  
        Vector3 world_up = Vector3.up;
        Vector3 rakel_up = (_topUp.transform.up + _botUp.transform.up)/2;
        float rotationZ = Vector3.SignedAngle(rakel_up, world_up, Vector3.forward);
        Value = rotationZ;
    }
}