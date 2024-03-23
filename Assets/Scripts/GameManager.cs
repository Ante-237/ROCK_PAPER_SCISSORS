using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MOVE
{
    ROCK = 0,
    PAPER = 1,
    SCISSORS = 2,
    DEFAULT = -1
}

public class GameManager : MonoBehaviour
{
    // GET REFFERENCE TO ALL PANELS
    // GET LIST OF SPRITES FOR ROCK PAPER SCISSORS
    // GET REFERENCE TO BUTTON LISTENERS
    // SET WAIT TIME FOR EVENT TRIGGERS
    // WRITE LOGIC FOR INPUT COMPARISMS
    // WRITE LOGIC FOR REPLAY AND EXITING APPLICATION

    [Header("Button References")] 
    [SerializeField] private Button Rock_Button;
    [SerializeField] private Button Paper_Button;
    [SerializeField] private Button Scissors_Button;
    [SerializeField] private Button Shoot_Button;
    [SerializeField] private Button Replay_Button;
    [SerializeField] private Button Exit_Button;

    [Header("UI References")] 
    [SerializeField] private GameObject RockPaperScissors_Panel;
    [SerializeField] private GameObject PlayModeHuman_Panel;
    [SerializeField] private GameObject ShootMode_Panel;
    [SerializeField] private GameObject FinalResults_Panel;
    [SerializeField] private GameObject PlaySequence_Panel;


    [Header("Text Reference Panels")] 
    [SerializeField] private TextMeshProUGUI CheckofHumanPlayerText;
    [SerializeField] private TextMeshProUGUI WinnerText;
    [SerializeField] private Image HumanSpriteSelected;
    [SerializeField] private Image MachineSpriteSelected;

    [Header("Sprite Types")] [SerializeField]
    private List<Sprite> SpriteTypes = new List<Sprite>();


    private MOVE HumanMove = MOVE.DEFAULT;
    private MOVE MachineMove = MOVE.DEFAULT;
    private Sprite HumanSprite;
    private Sprite MachineSprite;

    private void Start()
    {
        NullCheck();
        
        Rock_Button.onClick.AddListener(RockSelected);
        Paper_Button.onClick.AddListener(PaperSelected);
        Scissors_Button.onClick.AddListener(ScissorSelected);
        Shoot_Button.onClick.AddListener(ShootSelected);
        Replay_Button.onClick.AddListener(ReplaySelected);
        Exit_Button.onClick.AddListener(ExitSelected);
        
    }

    private void NullCheck()
    {
        
        Assert.IsNotNull(Rock_Button);
        Assert.IsNotNull(Paper_Button);
        Assert.IsNotNull(Scissors_Button);
        Assert.IsNotNull(Shoot_Button);
        Assert.IsNotNull(Replay_Button);
        Assert.IsNotNull(Exit_Button);
        
        Assert.IsNotNull(FinalResults_Panel);
        Assert.IsNotNull(PlaySequence_Panel);
        Assert.IsNotNull(ShootMode_Panel);
        Assert.IsNotNull(RockPaperScissors_Panel);
        Assert.IsNotNull(PlayModeHuman_Panel);
        
        Assert.IsNotNull(CheckofHumanPlayerText);
        Assert.IsNotNull(HumanSpriteSelected);
        Assert.IsNotNull(MachineSpriteSelected);
        
    }

    IEnumerator EnableWithinTime(GameObject mainObject, bool state, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        if (mainObject != null)
        {
            mainObject.SetActive(state);
        }
    }

    private void DisableRockPaperScissorsB()
    {
        Rock_Button.interactable = false;
        Paper_Button.interactable = false;
        Scissors_Button.interactable = false;
        StartCoroutine(EnableWithinTime(PlayModeHuman_Panel, false, 0));
        StartCoroutine(EnableWithinTime(ShootMode_Panel, true, 0.5f));
    }

    private void RockSelected()
    {
        // set rock as main move
        // enable shoot button and disable all main buttons
        HumanMove = MOVE.ROCK;
        DisableRockPaperScissorsB();
    }

    private void PaperSelected()
    {
        HumanMove = MOVE.PAPER;
        DisableRockPaperScissorsB();
    }

    private void ScissorSelected()
    {
        HumanMove = MOVE.SCISSORS;
        DisableRockPaperScissorsB();
    }

    private void ShootSelected()
    {
        // make moves for machine
        // disable rock, scissors, paper panel
        // check who wins
        // updates local state for sprites
        // update play state text
        // play sequence panel enable
        
        MachineMoves();
        WhoWins();
        StartCoroutine(EnableWithinTime(RockPaperScissors_Panel, false, 0.5f));
        StartCoroutine(EnableWithinTime(FinalResults_Panel, true, 0.6f));
        StartCoroutine(EnableWithinTime(ShootMode_Panel, false, 0.6f));
        StartCoroutine(EnableWithinTime(PlaySequence_Panel, true, 2f));

    }
    
  
    
    private void MachineMoves()
    {
        int Mach = Random.Range(0, 2);
        MachineMove = (MOVE) Mach;
    }

    private void UpdatePlayText(String Comparism, Sprite Human, Sprite Machine)
    {
        HumanSpriteSelected.sprite = Human;
        MachineSpriteSelected.sprite = Machine;

        CheckofHumanPlayerText.text = "Human <color=yellow>V.S</color> Machine";
        CheckofHumanPlayerText.text += "\n" + Comparism;
    }

    IEnumerator UpdateWinnerText(String theWinner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        WinnerText.text = theWinner;
    }
    
    // Rock beats Scissors
    // Scissors beats Paper
    //Paper beats Rock
    
    private void WhoWins()
    {
        if (HumanMove == MachineMove)
        {
            // We have a Tie
            // No one wins
            UpdatePlayText(MoveTypeCharacter(HumanMove) + " <color=yellow>V.S</color> " + MoveTypeCharacter(MachineMove), SpriteTypes[(int)HumanMove], SpriteTypes[(int)MachineMove] );
            StartCoroutine(UpdateWinnerText("We have a tie", 1.0f));
            return;
        }
        
        if (HumanMove == MOVE.ROCK && MachineMove == MOVE.SCISSORS)
        {
            // ROCK BEATS SCISSORS
            // HUMAN WINS THIS ROUND
            UpdatePlayText("ROCK <color=yellow>V.S</color> SCISSORS", SpriteTypes[0], SpriteTypes[2] );
            StartCoroutine(UpdateWinnerText("Human Wins",1.0f));
            return;
        }
        
        if(HumanMove == MOVE.PAPER && MachineMove == MOVE.ROCK){
            // PAPER BEATS ROCK
            // HUMAN WINS THIS ROUND
            UpdatePlayText("PAPER <color=yellow>V.S</color> ROCK", SpriteTypes[1], SpriteTypes[0] );
            StartCoroutine(UpdateWinnerText("Human Wins",1.0f));
            return;
        }

        if (HumanMove == MOVE.SCISSORS && MachineMove == MOVE.PAPER)
        {
            // SCISSORS BEATS PAPER
            // HUMAN WINS THIS ROUND
            UpdatePlayText("SCISSORS <color=yellow>V.S</color> PAPER", SpriteTypes[2], SpriteTypes[1] );
            StartCoroutine(UpdateWinnerText("Human Wins",1.0f));
            return;
        }
        
        UpdatePlayText( MoveTypeCharacter(HumanMove) + " <color=yellow>V.S</color> " + MoveTypeCharacter(MachineMove), SpriteTypes[(int)HumanMove], SpriteTypes[(int)MachineMove]);
        StartCoroutine(UpdateWinnerText("<color=red>Machine Wins</color>\n as always we remain Superior!!!",1.0f));
        // we can conclude machine wins.
        
    }

    private void ReplaySelected()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void ExitSelected()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    string MoveTypeCharacter(MOVE move)
    {
        if (move == MOVE.ROCK)
        {
            return "Rock";
        }

        if (move == MOVE.PAPER)
        {
            return "Paper";
        }
        
        if(move == MOVE.SCISSORS)
        {
            return "Scissors";
        }

        return "";
    }
    private void OnDisable()
    {
        Rock_Button.onClick.RemoveListener(RockSelected);
        Paper_Button.onClick.RemoveListener(PaperSelected);
        Scissors_Button.onClick.RemoveListener(ScissorSelected);
        Shoot_Button.onClick.RemoveListener(ShootSelected);
        Replay_Button.onClick.RemoveListener(ReplaySelected);
        Exit_Button.onClick.RemoveListener(ExitSelected);
    }
}
