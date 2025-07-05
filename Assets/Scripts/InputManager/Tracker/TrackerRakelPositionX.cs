using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    
    private float _rakelPositionX;
    private Vector3 _top, _bot;
    private float _rakelSideOffset = -0.05f;
    public override void Update()
    {
        _top = GameObject.Find("Top").transform.position;
        _bot = GameObject.Find("Bottom").transform.position;
        Vector3 rakelDir = (_top - _bot).normalized;

            
        Vector3 rakelRight = Vector3.Cross(rakelDir, Vector3.forward).normalized;
        
        Vector3 center = (_top + _bot) / 2f;

        center += rakelRight * _rakelSideOffset;
        
        Value = (center.x + 0.5f) * 8.9f;
    }
}