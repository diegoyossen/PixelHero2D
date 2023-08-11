using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockExtras : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerExtrasTracker playerExtrasTracker;
    [SerializeField] private bool canDobleJump;
    [SerializeField] private bool canDash;
    [SerializeField] private bool canEnterBallMode;
    [SerializeField] private bool canDropBombs;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerExtrasTracker = player.GetComponent<PlayerExtrasTracker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTracker();            
        }
        Destroy(gameObject);
    }

    private void SetTracker()
    {
        if (canDobleJump)
        {
            playerExtrasTracker.CanDobleJump = !playerExtrasTracker.CanDobleJump;
        }

        if (canDash)
        {
            playerExtrasTracker.CanDash = !playerExtrasTracker.CanDash;
        }

        if (canEnterBallMode)
        {
            playerExtrasTracker.CanEnterBallMode = !playerExtrasTracker.CanEnterBallMode;
        }

        if (canDropBombs)
        {
            playerExtrasTracker.CanDropBombs = !playerExtrasTracker.CanDropBombs;
        }
    }
}
