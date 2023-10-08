using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MonoBehaviour
{
  [SerializeField] private CubePool _cobePool;

  public void Merging(Cube fstCube, Cube scndCube)
  {
    if (fstCube.Data.Level != scndCube.Data.Level)
      return;

    fstCube.Upgrade(_cobePool.Cubes);
    scndCube.OnMerge -= Merging;
    _cobePool.Cubes.Remove(scndCube);
    Destroy(scndCube.gameObject);
  }
}
