using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _startPlayButton;
    [SerializeField] private GameObject _winButton;

    private Game _game;

    public GameObject StartPLayButton => _startPlayButton;
    public GameObject WinButton => _winButton;

    public void Initialize(Game game)
    {
        _game = game;
    }

    public void StartPlay()
    {
        _game.IsPlay = true;
        _startPlayButton.SetActive(false);
    }

    public void Win()
    {
        _winButton.SetActive(true);
    }
}
