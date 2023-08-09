using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to the enemy
public class DamageController : MonoBehaviour
{
    [SerializeField] private int EnemyDamage; // How many hearts the enemy is to remove (Put it one above the amount you acc want, I have no idea why)
    public Vector3 SpawnPoint; 
    [SerializeField] private HeartIndicator _healthController; //Gets the healthController function from HeartIndicatior

    private bool isDamaging = false;
    private bool isInvincible = false; 

    // Starts damaging the player when they get in contact with the enemy.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDamaging && !isInvincible)
            {
                StartCoroutine(DamagePlayer());
            }
        }
    }

    // Makes sure the isDamaging isn't counting damage when the player stops touching the enemy
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDamaging = false;
        }
    }

    
    // A routine to damage the player every after one second when they are in continuous contact with an enemy.
    IEnumerator DamagePlayer()
    {
        isDamaging = true;
        isInvincible = true; // Set invincible when taking damage

        while (isDamaging && _healthController.playerHealth > 0)
        {
            Damage();
            yield return new WaitForSeconds(1.0f); // Wait for 1 second before next damage
        }

        // This part deals with the actual respawning.
        if (_healthController.playerHealth <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = SpawnPoint;
            _healthController.playerHealth = 4;
            _healthController.UpdateHealth();
        }

        isInvincible = false; // Reset invincibility after taking damage
        yield return new WaitForSeconds(1.0f); // Invincibility timer
        isDamaging = false;
    }

    // Damages the player by reducing it's health by the amount of damages that the enemy gives.
    void Damage()
    {
        _healthController.playerHealth -= EnemyDamage;
        _healthController.UpdateHealth();
    }
}
