using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
  private List<Cube> _allCubes;

  public List<Cube> Cubes => _allCubes;
}
