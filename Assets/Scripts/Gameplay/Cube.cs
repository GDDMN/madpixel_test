using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public struct CubeData
{
  public int Level;
  public Color Color;
}


public class Cube : MonoBehaviour
{
  public event Action<Cube, Cube> OnMerge;

  [SerializeField] private List<TextMeshPro> _allNumbers;

  public CubeData Data;
  public bool AlreadyMerging = false;

  public void Init(int level, Color color)
  {
    Data.Level = level;
    Data.Color = color;

    UpdateCubeNumbers(Data.Level);
    gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
  }

  private void UpdateCubeNumbers(int number)
  {
    foreach (var numText in _allNumbers)
      numText.text = number.ToString();
  }


  private void Merging(Collision collision)
  {
    if (AlreadyMerging)
      return;

    if (this.Data.Level != collision.gameObject.GetComponent<Cube>().Data.Level)
      return;

    Cube mergingCube = collision.gameObject.GetComponent<Cube>();
    mergingCube.AlreadyMerging = true;
    OnMerge?.Invoke(this, mergingCube);

  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.layer == 10)
      Merging(collision);
  }
}
