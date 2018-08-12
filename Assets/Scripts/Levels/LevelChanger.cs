using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public bool debugTrigger;                               //debug tigger for the editor to trigger the movement animation
    public GameObject GameScreen;                           //the game screen
    public GameObject OverworldScreen;                      //the overworld
    public OverworldCharacter overworldCharacter;           //the overworld character

    private void Start()
    {
        if (overworldCharacter.onMoveFinished == null)
        {
            overworldCharacter.onMoveFinished = FinalizeTransition;
        }
        else
        {
            overworldCharacter.onMoveFinished += FinalizeTransition;
        }
    }

    private void Update()
    {
        if (debugTrigger)
        {
            debugTrigger = false;
            MoveLevel();
        }
    }

    void FinalizeTransition()
    {
        GameScreen.SetActive(true);
        OverworldScreen.SetActive(false);
    }

    public void MoveLevel()
    {
        LevelGenerator.StoreCurrentLevel();
        OverworldScreen.SetActive(true);
        GameScreen.SetActive(false);
    }



}
