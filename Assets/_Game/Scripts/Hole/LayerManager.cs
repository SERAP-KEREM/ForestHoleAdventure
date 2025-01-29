using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private  string[] _layers= {"Default","noColl"};

    private void OnTriggerEnter(Collider other)
    {
        ChangeLayer(other, 1);//no call layer 
    }
    private void OnTriggerExit(Collider other)
    {
        ChangeLayer(other, 0);// default layer
    }

    private void ChangeLayer(Collider other, int index)
    {
        other.gameObject.layer = LayerMask.NameToLayer(_layers[index]);
    }
}
