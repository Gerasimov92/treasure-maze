using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    [SerializeField] private GameObject closed;
    [SerializeField] private GameObject opened;

    private GameController _gameController;

    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Open()
    {
        closed.SetActive(false);
        opened.SetActive(true);
        _gameController.Win();
    }
}
