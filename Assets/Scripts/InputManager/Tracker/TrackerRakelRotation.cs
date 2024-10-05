using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    public override void Update()
    {
        Value = (GameObject.Find("RenderedRakel").transform.rotation.x -180) * (-140);
    }
}
