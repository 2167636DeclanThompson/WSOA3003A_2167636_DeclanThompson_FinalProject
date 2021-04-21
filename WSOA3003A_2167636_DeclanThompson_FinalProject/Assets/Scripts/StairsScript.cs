using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairsScript : MonoBehaviour
{
    
    public enum Stairs { Slime, Golem, Dragon, Demon, Death }

    public Stairs StairType = Stairs.Slime;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            if (StairType == Stairs.Slime)
            {
                SceneManager.LoadScene(2);
            }
            else if (StairType == Stairs.Golem)
            {
                SceneManager.LoadScene(3);
            }
            else if (StairType == Stairs.Dragon)
            {
                SceneManager.LoadScene(4);
            }
            else if (StairType == Stairs.Demon)
            {
                SceneManager.LoadScene(5);                
            }
            else if (StairType == Stairs.Death)
            {
                SceneManager.LoadScene(6);
            }

            
        }
    }
}
