using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
  [Header("Windows")]
  [SerializeField] private EndGameWindow _endGameWindow;

  [Header("Other Managers")]
  [SerializeField] private EndGameLine _endGameLine;

  private void OnEnable()
  {
    _endGameLine.OnEndGame += SetUpEndGameWindow;
  }

  private void OnDisable()
  {
    _endGameLine.OnEndGame -= SetUpEndGameWindow;
  }

  private void OnDestroy()
  {
    _endGameLine.OnEndGame -= SetUpEndGameWindow;
  }

  private void SetUpEndGameWindow()
  {
    _endGameWindow.gameObject.SetActive(true);
  }
}
