using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using RNG = UnityEngine.Random;

public class TheOverflow : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable ElButtones;
    public TextMesh NamberFaker;
    public GameObject Thiccass;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    #pragma warning disable 0649
    bool TwitchPlaysActive;
    #pragma warning restore 0649

    private List<int> Numbers = new List<int> {};
    int Total = 0;
    int Submit = 0;
    float Iteration = 0f;

    void Awake () {
        moduleId = moduleIdCounter++;
        ElButtones.OnInteract += delegate () { ElButtonesPress(); return false; };
        GetComponent<KMBombModule>().OnActivate += Kwan;
    }

    void Kwan () {
      NumberChooser();
      if (TwitchPlaysActive) {
        StartCoroutine(Embiggenator());
      }
      StartCoroutine(Cycling());
    }

    void NumberChooser () {
      Numbers.Clear();
      if (TwitchPlaysActive) {
        for (int i = 0; i < 6; i++) {
          Numbers.Add(RNG.Range(100000, 1000000));
          Debug.LogFormat("[The Overflow #{0}] The next number is {1}.", moduleId, Numbers[i]);
        }
      }
      else {
        for (int i = 0; i < 4; i++) {
          Numbers.Add(RNG.Range(100000, 1000000));
          Debug.LogFormat("[The Overflow #{0}] The next number is {1}.", moduleId, Numbers[i]);
        }
      }
      Total = Numbers[0];
      Debug.LogFormat("[The Overflow #{0}] The total is currently {1}.", moduleId, Total);
      for (int i = 1; i < Numbers.Count(); i++) {
        Total *= Numbers[i];
        Total %= 1000000;
        Debug.LogFormat("[The Overflow #{0}] The total is currently {1}.", moduleId, Total);
      }
      if (Total < 0) {
        Submit = 10 - ((Math.Abs(Total) - 1) % 9 + 1);
      }
      else {
        Submit = (Math.Abs(Total) - 1) % 9 + 1;
      }
      Debug.LogFormat("[The Overflow #{0}] Submit at a {1}.", moduleId, Submit);
    }

    void ElButtonesPress () {
      if (Bomb.GetTime() % 10 - Bomb.GetTime() % 1 == Submit) {
        GetComponent<KMBombModule>().HandlePass();
        StopAllCoroutines();
        Thiccass.transform.localScale -= new Vector3(Iteration, Iteration, Iteration);
      }
      else {
        GetComponent<KMBombModule>().HandleStrike();
        StopAllCoroutines();
        Thiccass.transform.localScale -= new Vector3(Iteration, Iteration, Iteration);
        Iteration = 0f;
        NumberChooser();
        if (TwitchPlaysActive) {
          StartCoroutine(Embiggenator());
        }
        StartCoroutine(Cycling());
      }
    }

    IEnumerator Embiggenator () {
      while (true) {
        Thiccass.transform.localScale += new Vector3(.0001f, .0001f, .0001f);
        yield return new WaitForSeconds(.02f);
        Iteration += .0001f;
      }
    }

    IEnumerator Cycling () {
      while (true) {
        for (int i = 0; i < Numbers.Count(); i++) {
          if (i == 0) {
            NamberFaker.color = new Color32(0, 255, 255, 255);
          }
          else {
            NamberFaker.color = new Color32(255, 0, 0, 255);
          }
          NamberFaker.text = Numbers[i].ToString();
          yield return new WaitForSeconds(2f);
        }
      }
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} # to submit on that number.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim();
      yield return null;
      if (Command != "1" && Command != "2" && Command != "3" && Command != "4" && Command != "5" &&
      Command != "6" && Command != "7" && Command != "8" && Command != "9" && Command != "0") {
        yield return null;
        yield return "sendtochaterror I don't understand!";
      }
      else {
        yield return null;
        while (Bomb.GetTime() % 10 - Bomb.GetTime() % 1 != int.Parse(Command)) {
          yield return new WaitForSeconds(1f);
        }
        ElButtones.OnInteract();
      }
    }
}
