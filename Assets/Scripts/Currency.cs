using System;
using UniRx;

public class Currency
{
  public ReactiveProperty<ulong> Scores = new(0);

}