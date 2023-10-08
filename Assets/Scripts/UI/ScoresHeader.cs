using UnityEngine;
using TMPro;
using UniRx;

public class ScoresHeader : MonoBehaviour
{
  private CompositeDisposable _disposables = new CompositeDisposable();

  [SerializeField] private TextMeshProUGUI _scores;

  private void Start()
  {
    G.Instance.Currency.Scores.Subscribe(context => UpdateValue(G.Instance.Currency.Scores.Value)).AddTo(_disposables);
  }

  private void UpdateValue(ulong value)
  {
    _scores.text = value.ToString();
  }
}
