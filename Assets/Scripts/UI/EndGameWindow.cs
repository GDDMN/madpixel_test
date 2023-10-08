using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameWindow : MonoBehaviour
{
  public void Click()
  {
    G.Instance.Currency.Scores.Value = (ulong)0;
    Debug.Log("Click");
    SceneManager.LoadScene("game");
  }
}
