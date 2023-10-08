using System.Collections.Generic;
using UnityEngine;

public class CubePool : MonoBehaviour
{
  public List<Cube> Cubes = new List<Cube>();

  public void SetCubeInPool(Cube cube)
  {
    cube.transform.SetParent(transform);
  }
}
