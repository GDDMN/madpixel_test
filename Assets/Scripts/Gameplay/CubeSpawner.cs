using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
  [SerializeField] private Cube _cube;

  public void SpawnCube()
  {
    var newCube = Instantiate(_cube, transform.position, Quaternion.identity);
    //newCube.OnMerge += SpawnCube;
  }
}
