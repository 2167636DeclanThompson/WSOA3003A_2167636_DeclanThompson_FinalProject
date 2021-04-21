using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{
    public GameObject BattleSystem;
    public GameObject gameScreen;   
   

    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            BattleSystem.SetActive(true);
            gameScreen.SetActive(false);
                       

            this.gameObject.SetActive(false);

        }
        
    }

    
}
