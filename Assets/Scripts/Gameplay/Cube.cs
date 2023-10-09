using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class Cube : MonoBehaviour
{
  public event Action<Cube, Cube> OnMerge;

  [Header("Jumping")]
  [SerializeField] private AnimationCurve _jumpCurve;
  [SerializeField] private float _jumpSpeed;
  [SerializeField] private float _jumpForce;

  [Header("Visual")]
  [SerializeField] private ParticleSystem _mergeEffect;
  [SerializeField] private List<TextMeshPro> _allNumbers;
  [SerializeField] private List<Color> _allColors;

  [Header("Data")]
  public CubeData Data;

  public bool AlreadyMerging = false;

  private Rigidbody _rigidbody;
  private Renderer _renderer;
  private bool _flying = false;
  private float _yJumpPos = 0f;
  private Vector3 _position;

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
    _position = ValidateCubeJumpPos(nextMergeCube);
    JumpToOtherCube(_position);
  }

  private Cube FindNearCube(List<Cube> allCubes)
  { 
    Cube nearCube;

    if (allCubes.Count <= 0)
      return null;

    nearCube = allCubes[0];
    Transform nearCubePos = nearCube.transform;


    foreach (var cube in allCubes)
    {
      float nearCubeDistance = Vector3.Distance(transform.position, nearCubePos.position);
      float newCubeDistance = Vector3.Distance(transform.position, cube.transform.position);

      if (nearCubeDistance > newCubeDistance)
      {
        nearCube = cube;
        nearCubePos = nearCube.transform;
      }
    }

    return nearCube;
  }

  private Vector3 ValidateCubeJumpPos(Cube nextMergeCube)
  {
    Vector3 position;

    if (nextMergeCube != null && nextMergeCube.gameObject.GetComponent<CubeSwipeHandler>() == null)
      position = nextMergeCube.transform.position;
    else
      position = transform.position;

    return position;
  }

  private void JumpToOtherCube(Vector3 position)
  {
    if (_flying)
      return;

    _yJumpPos = 0f;
    _flying = true;
    _rigidbody.useGravity = false;
    StartCoroutine(JumpAnimation());
  }

  private IEnumerator JumpAnimation()
  {
    float yPos = transform.position.y;

    while (_flying && _yJumpPos <= 1f)
    {
      _yJumpPos += _jumpSpeed * Time.deltaTime;
      
      transform.position = new Vector3(transform.position.x,
                                 yPos + (_jumpCurve.Evaluate(_yJumpPos) * _jumpForce),
                                 transform.position.z);

      transform.position += new Vector3((_position.x - transform.position.x) * _yJumpPos / 10,
                                        0f,
                                        (_position.z - transform.position.z) * _yJumpPos / 10);
      Debug.Log(yPos + (_jumpCurve.Evaluate(_yJumpPos) * _jumpForce));

      yield return null;
    }

    _flying = false;
    _yJumpPos = 0f;
  }

  private void OnCollisionEnter(Collision collision)
  {
    _rigidbody.useGravity = true;

    if (collision.gameObject.layer == 10)
    {
      Merging(collision);
    }
  }
}
