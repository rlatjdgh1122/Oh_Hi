using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Animator anim;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        playerInput.OnFirePressed += FireButtonHandle;
    }
    public void FireButtonHandle()
    {
        Debug.Log("ewr");
        playerMovement?.SetRotation();
        gun?.Fire();
    }
    void Update()
    {
        if (playerInput.reload)
        {
            if (gun.Reload())
                anim.SetTrigger("Reload");
        }
    }

}
