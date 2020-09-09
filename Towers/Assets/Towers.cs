using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class Towers : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] Buttons;
    public GameObject[] Cubes;
    public TextMesh[] TopClues;
    public TextMesh[] BottomClues;
    public TextMesh[] LeftClues;
    public TextMesh[] RightClues;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    int[][] TowerSizes = new int[5][] {
      new int[] {1, 2, 3, 4, 5},
      new int[] {1, 2, 3, 4, 5},
      new int[] {1, 2, 3, 4, 5},
      new int[] {1, 2, 3, 4, 5},
      new int[] {1, 2, 3, 4, 5}
    };
    int TryThis = 0;

    void Awake () {
        moduleId = moduleIdCounter++;
        /*
        foreach (KMSelectable object in keypad) {
            object.OnInteract += delegate () { keypadPress(object); return false; };
        }
        */

        //button.OnInteract += delegate () { buttonPress(); return false; };

    }

    // Use this for initialization
    void Start () {
      Whoops:
      TryThis = 0;
      for (int i = 0; i < 5; i++) {
        TowerSizes[i].Shuffle();
      }
      for (int i = 1; i < 5; i++) {
        for (int j = 0; j < 5; j++) {
          Reroll:
          if (TryThis >= 100) {
            goto Whoops;
          }
          if ((TowerSizes[i][j] == TowerSizes[0][j]) || (TowerSizes[i][j] == TowerSizes[1][j] && i != 1) || (TowerSizes[i][j] == TowerSizes[2][j] && i != 2)
          || (TowerSizes[i][j] == TowerSizes[3][j] && i != 3) || (TowerSizes[i][j] == TowerSizes[4][j] && i != 4)) {
            TowerSizes[i].Shuffle();
            TryThis++;
            goto Reroll;
          }
        }
      }
      int Count = 0;
      int CurrentBiggest = 0;
      for (int i = 0; i < 5; i++) {
        for (int j = 0; j < 5; j++) {
          if (TowerSizes[j][i] > CurrentBiggest) {
            Count++;
            CurrentBiggest = TowerSizes[j][i];
          }
        }
        TopClues[i].text = CurrentBiggest.ToString();
        CurrentBiggest = 0;
      }
      for (int i = 0; i < TowerSizes.Length; i++) {
        for (int j = 0; j < 5; j++) {
          Debug.Log(TowerSizes[i][j]);
        }
      }
    }



    // Update is called once per frame
    void Update () {

    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
    }
}
