using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    private AutoRotation AutoRotation;

    public TrackerRakelRotation()
    {
        AutoRotation = new AutoRotation();
    }

    public override void Update()
    {
        AutoRotation.Update(GameObject.Find("RenderedRakel").transform.position);
        Value = AutoRotation.Value;
    }
}
