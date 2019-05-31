using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainUserInterface : MonoBehaviour
{
    public GameController gameController;

    public Slider playerCountSlider;
    public Button newGameButton;


    public void StartGame()
    {
        int playerCount = (int)playerCountSlider.value;

        gameController.StartNewGame(playerCount);
    }
}

