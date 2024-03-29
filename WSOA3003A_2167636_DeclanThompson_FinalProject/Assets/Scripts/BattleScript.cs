﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Brackeys. "Turn-Based Combat in Unity." YouTube. November 24, 2019. [Video file] Available at: https://www.youtube.com/watch?v=_1pz_ohupPs. 

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleScript : MonoBehaviour
{
    public GameObject battleSystem;
    public GameObject gameScreen;

    public BattleState state;

    public BattleHUD battleHUD;

    public GameObject enemy;    

    public Text dialogue;
    public Text TurnText;

    public Text Health;
    public Text Magic;

    public GameObject Sword;

    public GameObject player;   

    public GameObject explosion;
    public GameObject hit;
    
    public Transform enemyShadow;    

    public Camera cam;
       

    public Color green = Color.green;
    public Color black = Color.black;
    public Color red = Color.red;
    public Color white = Color.white;

    UnitScript playerUnit;
    UnitScript enemyUnit;

    public BattleHUD playerHUD;

    public Animator screenShake;

    public KeyCode Quit;

    public AudioSource playerHit;
    public AudioSource enemyHit;
    public AudioSource playerWin;
    public AudioSource playerLose;
    public AudioSource playerHeal;
    public AudioSource Fireball;
    public AudioSource enemyCrit;
    public AudioSource playerCrit;

    public bool isDeath;     

    private void Awake()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle()); 
        
    }

    private void Update()
    {
        if (state == BattleState.START)
        {
            TurnText.text = "Commence Battle!";
        }
        else if (state == BattleState.PLAYERTURN)
        {
            TurnText.text = "Player Turn!";
        }
        else if (state == BattleState.ENEMYTURN)
        {
            TurnText.text = "Enemy Turn!";
        }
        else
        {
            TurnText.text = "Battle Over!";
        }       

    }

    IEnumerator SetupBattle()
    {
        playerUnit = player.GetComponent<UnitScript>();

        GameObject enemyGO = Instantiate(enemy, enemyShadow);
        enemyUnit = enemyGO.GetComponent<UnitScript>();        

        dialogue.text = enemyUnit.Name + " appears!";
        TurnText.text = "Commence Battle!";

        playerHUD.SetHUD(playerUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        int RandNum;
        RandNum = Random.Range(0, 11);

        if (RandNum >= 9)
        {
            bool isDead = enemyUnit.TakeDamage(playerUnit.Attack * 2);

            dialogue.text = "Critical Hit! You deal " + (playerUnit.Attack * 2) + " points of damage!";
            GameObject playerHit = Instantiate(hit, enemyShadow);
            enemyCrit.Play();            


            yield return new WaitForSeconds(2f);

            

            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else if (RandNum < 1)
        {
            dialogue.text = "Your attack misses!";
            yield return new WaitForSeconds(2f);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            bool isDead = enemyUnit.TakeDamage(playerUnit.Attack);

            dialogue.text = "You deal " + (playerUnit.Attack) + " points of damage!";
            enemyHit.Play();
            GameObject playerHit = Instantiate(hit, enemyShadow);


            yield return new WaitForSeconds(2f);


            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        


    }

    IEnumerator PlayerHeal()
    {
        if (playerUnit.currentHP < playerUnit.maxHP)
        {
            if (playerUnit.currentMP >= 5)
            {
                int HealNum;
                HealNum = Random.Range(0, 11);

                if (HealNum == 0)
                {
                    playerUnit.MP(5);
                    playerHUD.SetMP(playerUnit.currentMP);
                    Magic.text = playerUnit.currentMP.ToString();
                    dialogue.text = "The spell fizzles out!";
                    yield return new WaitForSeconds(2f);
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
                else if (HealNum == 10)
                {
                    dialogue.text = "The Spell goes haywire!";
                    yield return new WaitForSeconds(2f);

                    playerUnit.Heal(playerUnit.MagicAttack * 2);
                    playerUnit.MP(5);

                    playerHUD.SetHP(playerUnit.currentHP);
                    playerHUD.SetMP(playerUnit.currentMP);

                    Health.text = playerUnit.currentHP.ToString();
                    Magic.text = playerUnit.currentMP.ToString();

                    dialogue.text = "You heal " + (playerUnit.MagicAttack * 2) + " points of Health";
                    playerHeal.Play();
                    cam.backgroundColor = green;

                    yield return new WaitForSeconds(2f);

                    cam.backgroundColor = black;
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
                else
                {
                    playerUnit.Heal(playerUnit.MagicAttack);
                    playerUnit.MP(5);

                    playerHUD.SetHP(playerUnit.currentHP);
                    playerHUD.SetMP(playerUnit.currentMP);

                    Health.text = playerUnit.currentHP.ToString();
                    Magic.text = playerUnit.currentMP.ToString();

                    dialogue.text = "You heal " + (playerUnit.MagicAttack) + " points of Health";
                    playerHeal.Play();
                    cam.backgroundColor = green;

                    yield return new WaitForSeconds(2f);

                    cam.backgroundColor = black;
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }

                
            }
            else
            {
                dialogue.text = "You dont have enough MP!";

            }
        }
        else
        {
            dialogue.text = "You are already at full HP!";
        }
        
        

    }

    IEnumerator PlayerMagic()
    {
        
        if (playerUnit.currentMP >= 10)
        {
            int MagicNumber;
            MagicNumber = Random.Range(0, 11);

            if (MagicNumber == 0)
            {
                playerUnit.MP(10);
                playerHUD.SetMP(playerUnit.currentMP);
                Magic.text = playerUnit.currentMP.ToString();
                dialogue.text = "The spell fizzles out!";
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());

            }
            else if (MagicNumber == 10)
            {
                bool isDead = enemyUnit.MagicDamage(playerUnit.MagicAttack * 2);
                playerUnit.MP(10);
                playerHUD.SetMP(playerUnit.currentMP);
                Magic.text = playerUnit.currentMP.ToString();
                dialogue.text = "The Spell goes haywire!";
                yield return new WaitForSeconds(2f);
                dialogue.text = "Demon King takes " + (playerUnit.MagicAttack * 2) + " magic damage!";
                Fireball.Play();
                GameObject playerHit = Instantiate(explosion, enemyShadow);

                yield return new WaitForSeconds(2f);

                if (isDead)
                {
                    state = BattleState.WON;
                    EndBattle();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
            else
            {
                bool isDead = enemyUnit.MagicDamage(playerUnit.MagicAttack);
                playerUnit.MP(10);
                playerHUD.SetMP(playerUnit.currentMP);
                Magic.text = playerUnit.currentMP.ToString();

                dialogue.text = "Demon King takes " + (playerUnit.MagicAttack) + " magic damage!";
                Fireball.Play();
                GameObject playerHit = Instantiate(explosion, enemyShadow);

                yield return new WaitForSeconds(2f);

                if (isDead)
                {
                    state = BattleState.WON;
                    EndBattle();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }

            
        }        
        else
        {
            dialogue.text = "You dont have enough MP!";
        }

    }
        

   IEnumerator EnemyTurn()
    {
        int EnemyNumber;
        EnemyNumber = Random.Range(0, 11);

        if (EnemyNumber >= 10)
        {
            dialogue.text = enemyUnit.Name + " attacks!";

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.Attack*2);
            dialogue.text = "A desperate attack! You take " + (enemyUnit.Attack*2) + " points of damage!";
            cam.backgroundColor = red;
            playerCrit.Play();
            OnCamShake();

            playerHUD.SetHP(playerUnit.currentHP);
            Health.text = playerUnit.currentHP.ToString();

            yield return new WaitForSeconds(1f);
            cam.backgroundColor = black;

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {

                state = BattleState.PLAYERTURN;
                PlayerTurn();


            }
        }
        else if (EnemyNumber <= 2)
        {
            dialogue.text = enemyUnit.Name + " attacks!";
            yield return new WaitForSeconds(1f);
            dialogue.text = "You dodge the enemy attack!";
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            dialogue.text = enemyUnit.Name + " attacks!";

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.Attack);
            dialogue.text = "You take " + (enemyUnit.Attack) + " points of damage!";
            cam.backgroundColor = red;
            playerHit.Play();
            OnCamShake();

            playerHUD.SetHP(playerUnit.currentHP);
            Health.text = playerUnit.currentHP.ToString();

            yield return new WaitForSeconds(1f);

            cam.backgroundColor = black;

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {

                state = BattleState.PLAYERTURN;
                PlayerTurn();


            }
        }
        

        
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            StartCoroutine(PlayerWon());
        }
        else if (state == BattleState.LOST)
        {

            StartCoroutine(PlayerLost());

        }
        
    }
    
        
   

    void PlayerTurn()
    {
        dialogue.text = "Select an option!";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack());
        } 
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN)
        {
            
            StartCoroutine(PlayerHeal());
        }
    }

    public void OnMagicButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN)
        {
           
            StartCoroutine(PlayerMagic());
        }
    }

    
    public void QuitGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }    
        
        
    }

    public void OnCamShake()
    {
        screenShake.SetTrigger("Shake");
    }

    IEnumerator PlayerWon()
    {
        if (isDeath == true)
        {
            Destroy(enemyShadow.gameObject);
            dialogue.text = "You are victorious!";
            cam.backgroundColor = green;
            playerWin.Play();
            yield return new WaitForSeconds(2f);
            dialogue.text = "You may now exit the dungeon!";
        }
        else
        {
            Destroy(enemyShadow.gameObject);
            dialogue.text = "You are victorious!";
            cam.backgroundColor = green;
            playerWin.Play();
            yield return new WaitForSeconds(2f);
            playerUnit.Level = playerUnit.Level + 1;
            battleHUD.levelText.text = playerUnit.Level.ToString();
            dialogue.text = "Level Up! Max HP and MP +10! Spell +5!";
            playerUnit.maxHP = playerUnit.maxHP + 10;
            playerUnit.currentHP = playerUnit.maxHP;
            playerUnit.maxMP = playerUnit.maxMP + 10;
            playerUnit.currentMP = playerUnit.maxMP;
            playerUnit.MagicAttack = playerUnit.MagicAttack + 5;
            yield return new WaitForSeconds(2f);
            cam.backgroundColor = black;
            Sword.SetActive(true);
            dialogue.text = "The enemy drops a Sword! Attack +5!";
            playerUnit.Attack = playerUnit.Attack + 5;
            yield return new WaitForSeconds(2f);            
            gameScreen.SetActive(true);
            battleSystem.SetActive(false);
        }
        
    }

    IEnumerator PlayerLost()
    {
        dialogue.text = "You have been slain!";
        cam.backgroundColor = red;
        playerLose.Play();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(7);
    }

         
       

    

    


}
