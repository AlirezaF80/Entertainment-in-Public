using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkPointAudio;
    private Transform currentCheckPoint;
    private PlayerHealth playerPlayerHealth;
    private UIManager uiManager;
    private PlayerController playerController;
    [SerializeField] private Animator anim;


    private void Awake()
    {
        playerPlayerHealth = GetComponent<PlayerHealth>();
        uiManager = FindObjectOfType<UIManager>();
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    public void Respawn()
    {

        //if(currentCheckPoint == null)
        //{
        //uiManager.GameOver();
        //return;
        //}

        if (currentCheckPoint == null)
        {
            Die(); // Call Die() if no checkpoint
            return;
        }

        transform.position = currentCheckPoint.position;
        playerPlayerHealth.Respawn();
        //Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckPoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "CheckPoint")
        {
            currentCheckPoint = collision.transform;
            SoundManager.instance.PlaySound(checkPointAudio);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Enter");
        }
    }

    private void Die()
    {
        anim.SetTrigger("death");
        playerController.enabled = false; // Disable player controller
        uiManager.GameOver(); // Optionally display game over screen
    }
}
