using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
  [SerializeField] private CubeSwipeHandler _cube;

  [SerializeField] private float _minSpawnPosition;
  [SerializeField] private float _maxSpawnPosition;
  
  private List<Cube> _allCubes;
  private readonly float _zSpawnPos = -6;

  public List<Cube> Cubes => _allCubes;

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;

    Gizmos.DrawSphere(new Vector3(_minSpawnPosition, 0f, _zSpawnPos), .1f);
    Gizmos.DrawSphere(new Vector3(_maxSpawnPosition, 0f, _zSpawnPos), .1f);

    Gizmos.DrawLine(new Vector3(_minSpawnPosition, 0f, _zSpawnPos),
                    new Vector3(_maxSpawnPosition, 0f, _zSpawnPos));
  }

  private void Start()
  {
    SpawnCube();
  }

  private void SpawnCube()
  {
    var cube = Instantiate(_cube, new Vector3(0f, .6f, _zSpawnPos), Quaternion.identity);
    cube.Init(_minSpawnPosition, _maxSpawnPosition);
    cube.OnCollision += SpawnCube;
  }

  public void Merging(Cube fstCube, Cube scndCube)
  {
    GameObject cube = new GameObject();
    cube.AddComponent<Cube>();
    cube.GetComponent<Cube>().Data.Level = fstCube.GetComponent<Cube>().Data.Level * 2;
  }
}
