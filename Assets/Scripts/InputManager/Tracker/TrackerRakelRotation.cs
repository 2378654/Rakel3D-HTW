using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    private float _rotation;
    private Vector3 _topUp = GameObject.Find("Top").transform.up;
    private Vector3 _botUp = GameObject.Find("Bottom").transform.up;
    public override void Update()
    {
        _topUp = GameObject.Find("Top").transform.up;
        _botUp = GameObject.Find("Bottom").transform.up;
        Vector3 world_up = Vector3.up;
        Vector3 rakel_up = (_topUp + _botUp)/2;
        float rotationZ = Vector3.SignedAngle(rakel_up, world_up, Vector3.forward);
        Value = rotationZ;
    }
}