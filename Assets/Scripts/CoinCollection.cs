using UnityEngine;
using System.Collections.Generic;

public class CoinCollection : MonoBehaviour
{
    readonly HashSet<Coin> allCoins = new HashSet<Coin>();
    int maxCoins;

    public int NumCoins
    {
        get
        {
            return allCoins.Count;
        }
    }

    public int MaxCoins
    {
        get
        {
            return maxCoins;
        }
    }

    void Start ()
    {
        Coin[] coins = GetComponentsInChildren<Coin>();
        foreach (Coin coin in coins)
        {
            coin.Collection = this;
            allCoins.Add(coin);
        }
        maxCoins = NumCoins;
    }

    public void RemoveCoin(Coin coin)
    {
        allCoins.Remove(coin);
    }
}
