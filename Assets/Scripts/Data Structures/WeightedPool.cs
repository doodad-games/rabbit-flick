using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class WeightedPool<T>
{
    readonly List<float> _weights = new();
    readonly List<T> _items = new();
    float _totalWeight;

    public void Add(float weight, T item)
    {
        _weights.Add(weight);
        _totalWeight += weight;
        _items.Add(item);
    }

    public T PickRandom()
    {
#if UNITY_EDITOR
        Assert.IsTrue(_weights.Count != 0);
#endif

        var point = Random.Range(0f, _totalWeight);
        var cumulativeWeight = 0f;
        for (var i = 0; i != _weights.Count; ++i)
        {
            cumulativeWeight += _weights[i];
            if (cumulativeWeight > point)
                return _items[i];
        }

        throw new InvalidOperationException("Weighted pool cumulative weight failure");
    }
}