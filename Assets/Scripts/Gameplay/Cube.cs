using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

[Serializable]
public struct CubeData
{
  public int Level;
  public int ColorNum;
}


public class Cube : MonoBehaviour
{
  public event Action<Cube, Cube> OnMerge;

  [SerializeField] private List<TextMeshPro> _allNumbers;
  [SerializeField] private List<Color> _allColors;

  [SerializeField] private AnimationCurve _jumpCurve;
  [SerializeField] private float _jumpSpeed;

  [SerializeField] private ParticleSystem _mergeEffect;

  public CubeData Data;
  public bool AlreadyMerging = false;

  private Rigidbody _rigidbody;
  private Renderer _renderer;
  private bool _flying = false;
  private float _yJumpPos = 0f;
  Vector3 _position;
  private void Awake()
  {
    _renderer = gameObject.GetComponent<Renderer>();
    _rigidbody = gameObject.GetComponent<Rigidbody>();
  }

  public void Init(CubeData data){
    Data.Level = data.Level;
    Data.ColorNum = data.ColorNum;

    UpdateCubeNumbers(Data.Level);
    SetColor();
  }

  private void UpdateCubeNumbers(int number)
  {
    foreach (var numText in _allNumbers)
      numText.text = number.ToString();
  }
  
  private void SetColor()
  {
    _renderer.material.SetColor("_Color", _allColors[Data.ColorNum]);
  }

  private void UpdateColor()
  {
    Data.ColorNum++;
    if (Data.ColorNum >= _allColors.Count)
      Data.ColorNum = 0;

    _renderer.material.SetColor("_Color", _allColors[Data.ColorNum]);
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

  public void Upgrade(List<Cube> allCubes)
  {
    Data.Level = Data.Level * 2;
    G.Instance.Currency.Scores.Value += (ulong)Data.Level;


    UpdateCubeNumbers(Data.Level);
    UpdateColor();
    _mergeEffect.Play();
    StopCoroutine(JumpAnimation());

    List<Cube> unLevelCubes = allCubes.FindAll(c => c.Data.Level == Data.Level && c != this);

    Cube nextMergeCube = FindNearCube(unLevelCubes);

    if (nextMergeCube != null 
        && nextMergeCube.gameObject.GetComponent<CubeSwipeHandler>() == null
        && Math.Abs(nextMergeCube.transform.position.x - transform.position.x) < 3.5f
        && Math.Abs(nextMergeCube.transform.position.z - transform.position.z) < 3.5f)
      _position = nextMergeCube.transform.position;
    else
      _position = transform.position;

    JumpToOtherCube(_position);
  }

  private Cube FindNearCube(List<Cube> allCubes)
  {

    Cube nearCube;

    if (allCubes.Count <= 0)
      return null;

    nearCube = allCubes[0];

    foreach (var cube in allCubes)
    {
      if (Math.Abs(transform.position.x - cube.transform.position.x) < Math.Abs(transform.position.x - nearCube.transform.position.x) &&
          Math.Abs(transform.position.z - cube.transform.position.z) < Math.Abs(transform.position.z - nearCube.transform.position.z))
        nearCube = cube;
    }

    return nearCube;
  }

  private void JumpToOtherCube(Vector3 position)
  {
    if (!_flying)
      return;

    _flying = false;
    StartCoroutine(JumpAnimation());
  }

  private IEnumerator JumpAnimation()
  {
    while(!_flying && _yJumpPos <= 1f)
    {
      transform.position = new Vector3(transform.position.x,
                                 _jumpCurve.Evaluate(_yJumpPos),
                                 transform.position.z);

      transform.position += new Vector3((_position.x - transform.position.x) * _yJumpPos / 10,
                                        0f,
                                        (_position.z - transform.position.z) * _yJumpPos / 10);

      _yJumpPos += _jumpSpeed * Time.deltaTime;
      yield return null;
    }

    _flying = true;
    _yJumpPos = 0f;
  }

  private void OnCollisionEnter(Collision collision)
  {
    _rigidbody.useGravity = true;

    if (collision.gameObject.layer == 10)
    {
      _flying = true;
      Merging(collision);
    }
  }

  private void OnCollisionStay(Collision collision)
  {
    if (collision.gameObject.layer == 10)
    {
      Merging(collision);
    }
  }
}
