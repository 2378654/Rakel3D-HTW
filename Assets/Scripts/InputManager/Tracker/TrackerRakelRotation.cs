using UnityEngine;

public class TrackerRakelRotation : FloatValueSource
{
    //private AutoRotation AutoRotation;

    //public TrackerRakelRotation()
    //{
    //    AutoRotation = new AutoRotation();
    //}

    public override void Update()
    {
        //AutoRotation.Update(GameObject.Find("RenderedRakel").transform.position);
        //Value = AutoRotation.Value;
        
       /* if (GameObject.Find("RenderedRakel").transform.rotation.x * (-90) > 180)
        {
            Value = (GameObject.Find("RenderedRakel").transform.rotation.x * (-90)) - 180;
        }
        else
        {
            Value = (GameObject.Find("RenderedRakel").transform.rotation.x) * (-90f); 
        }
        */
       Value = (GameObject.Find("RenderedRakel").transform.rotation.x) * (-128); 
    }
}
