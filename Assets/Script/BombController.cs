using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float waitForExplode;
    [SerializeField] private float waitForDestroy;
    [SerializeField] private Transform myTransform;
    private Animator myAnimator;
    private bool isActive;
    private int IdIsActive;
    [SerializeField] private float expansiveWaveRange;
    [SerializeField] private LayerMask isDestroyable;

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
        IdIsActive = Animator.StringToHash("isActive");
    }

    private void Update()
    {
        waitForExplode -= Time.deltaTime;
        waitForDestroy -= Time.deltaTime;
        if(waitForExplode <= 0 && !isActive)
        {
            ActivatedBomb();            
        }
        if(waitForDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ActivatedBomb()
    {
        isActive = true;
        myAnimator.SetBool(IdIsActive, isActive);
        Collider2D[] destroyedObjects =  Physics2D.OverlapCircleAll(myTransform.position, expansiveWaveRange,isDestroyable);
        if(destroyedObjects.Length > 0)
        {
            foreach (var item in destroyedObjects)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(myTransform.position, expansiveWaveRange);
    }
}
