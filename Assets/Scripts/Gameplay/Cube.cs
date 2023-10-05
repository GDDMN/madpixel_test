using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
  public event Action OnMerge;

  [SerializeField] private List<TextMeshPro> _allNumbers;
  private int _number = 2;

  private void UpdateCubeNumbers(int number)
  {
    foreach (var numText in _allNumbers)
      numText.text = number.ToString();
  }


  private void Merging(Collision collision)
  {
    UpdateCubeNumbers(1 + _number);
    OnMerge?.Invoke();
  }

  private void OnCollisionEnter(Collision collision)
  {
    if(collision.gameObject.layer == 10)
      Merging(collision); 
  }
}
