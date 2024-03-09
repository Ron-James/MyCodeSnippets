using UnityEngine;
using System;
using Autosave;
public class PlayerController : MonoBehaviour, ISaveable
{
    private int playerHealth = 100;
    private Vector3 playerPosition;

    // Implement ISaveable interface
    public SaveData GetSaveData()
    {
        return new PlayerSaveData
        {
            playerHealth = playerHealth,
            playerPosition = transform.position
        };
    }
    //Restores the state of the player
    public void RestoreState(SaveData saveData, Action onComplete)
    {
        if (saveData is PlayerSaveData playerSaveData)
        {
            playerHealth = playerSaveData.playerHealth;
            transform.position = playerSaveData.playerPosition;
            Debug.Log("Player state restored, position set to " + playerSaveData.playerPosition + "health set to " + playerSaveData.playerHealth);
        }
        else
        {
            Debug.LogError("Failed to restore player state: Invalid save data");
        }

        onComplete?.Invoke();
    }

    // Example method to update player health
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death
    }
}
