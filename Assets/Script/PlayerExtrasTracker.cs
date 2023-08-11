using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtrasTracker : MonoBehaviour
{
    [SerializeField] private bool _canDobleJump;
    [SerializeField] private bool _canDash;
    [SerializeField] private bool _canEnterBallMode;
    [SerializeField] private bool _canDropBombs;

    public bool CanDobleJump { get => _canDobleJump; set => _canDobleJump = value; }
    public bool CanDash { get => _canDash; set => _canDash = value; }
    public bool CanEnterBallMode { get => _canEnterBallMode; set => _canEnterBallMode = value; }
    public bool CanDropBombs { get => _canDropBombs; set => _canDropBombs = value; }
}