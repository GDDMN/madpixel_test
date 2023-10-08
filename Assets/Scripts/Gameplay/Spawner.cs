using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [SerializeField] private CubeSwipeHandler _cube;

  [Header("Spawn Range")]
  [SerializeField] private float _minSpawnPosition;
  [SerializeField] private float _maxSpawnPosition;

  [Header("Managers")]
  [SerializeField] private Merger _merger;
  [SerializeField] private CubePool _cubePool;
  [SerializeField] private EndGameLine _endGameLine;

  [SerializeField] private List<CubeData> _allCubesData = new List<CubeData>();

  private readonly float _zSpawnPos = -6;
  private bool _isSpawningValid = true;

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
    _endGameLine.OnEndGame += OnEndGame;
  }

  private void OnDestroy()
  {
    _endGameLine.OnEndGame -= OnEndGame;
  }

  private void SpawnCube()
  {
    if (!_isSpawningValid)
      return;

    var cube = Instantiate(_cube, new Vector3(0f, .8f, _zSpawnPos), Quaternion.identity);
    cube.Init(_minSpawnPosition, _maxSpawnPosition);
    cube.OnCollision.AddListener(SpawnCube);

    Cube cubeComponent = cube.gameObject.GetComponent<Cube>();
    cubeComponent.Init(_allCubesData[Random.Range(0, 3)]);

    _cubePool.Cubes.Add(cubeComponent);
    _cubePool.SetCubeInPool(cubeComponent);


    cube.GetComponent<Cube>().OnMerge += _merger.Merging;
  }

  private void OnEndGame()
  {
    _isSpawningValid = false;
  }
}
