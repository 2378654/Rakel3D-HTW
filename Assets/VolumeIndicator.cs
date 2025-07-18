using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VolumeIndicator : MonoBehaviour {    
    private ButtonInteraction _interaction;    
    float currentVolume, oldVolume;    
    private LineRenderer _line, _lineContainer;    
    private Coroutine _coroutine;    
    public float currentX;

    void Start()
    {
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();        
               
        _lineContainer = GameObject.Find("VolumeContainer").GetComponent<LineRenderer>();                
        _lineContainer.startWidth = 0.7f;        
        _lineContainer.endWidth = 0.7f;
        
        _line = GameObject.Find("VolumeIndicator").GetComponent<LineRenderer>(); 
        _line.startWidth = 0.5f;        
        _line.endWidth = 0.5f;  
        _line.startColor = Color.black;
        _line.endColor = Color.black;
    }

    void Update()
    {
        currentVolume = _interaction.GetVolume();
        if (currentVolume != oldVolume)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }            
            _coroutine = StartCoroutine(ShowVolumeIndicator());            
            oldVolume = currentVolume;
        }
    }   
    private IEnumerator ShowVolumeIndicator()  
    { 
        //PaintVolume
        _line.SetPosition(0, new Vector3(currentX, -1.8f, -0.15f));        
        _line.SetPosition(1, new Vector3(currentX, -1.8f+currentVolume/60, -0.15f));                
        
        //PaintContainer
        _lineContainer.SetPosition(0, new Vector3(currentX, -2f, -0.15f));        
        _lineContainer.SetPosition(1, new Vector3(currentX, 2.7f, -0.15f));                
        
        yield return new WaitForSeconds(2);        
        
        //PaintContainer
        _lineContainer.SetPosition(0, new Vector3(currentX, 0, -0.15f));        
        _lineContainer.SetPosition(1, new Vector3(currentX, 0, -0.15f));                
        
        //PaintVolume
        _line.SetPosition(0, new Vector3(currentX, 0, -0.15f));        
        _line.SetPosition(1, new Vector3(currentX, 0, -0.15f));
    }
}