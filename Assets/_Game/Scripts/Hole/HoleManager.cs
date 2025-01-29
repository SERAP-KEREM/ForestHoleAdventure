using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class HoleManager : MonoBehaviour
{
    private float _circleCapacity;
    [SerializeField] private Image _circleImage;
    [SerializeField] private Transform _holeGameObject;

    private void Start()
    {
        _circleCapacity = _holeGameObject.localScale.x;
    }
    private void ProcessBarCircle(int number)
    {
        _circleCapacity=1f/number;

        _circleImage.fillAmount+=_circleCapacity;

        if(_circleImage.fillAmount.Equals(1f))        {
           _holeGameObject.localScale+=new Vector3(0.3f,0f,3f);
           _circleImage.fillAmount=0f;  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("cube"))
       {
        ProcessBarCircle(20);
        other.gameObject.SetActive(false);
       }
    }
}
