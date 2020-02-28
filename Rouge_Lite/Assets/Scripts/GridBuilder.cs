using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBuilder : MonoBehaviour {
    public GameObject basicLand, player, playerDisplay;
    public GameObject[ , ] grid = new GameObject[50,50];
    public Vector2 gridSize;
    public Vector3 playerStartingPos;
    public int enemyCount = 1; 
    int moves = 3; 
    private void Start() {
        for (int y = 0; y < gridSize.y; y++) {
            for (int x = 0; x < gridSize.x; x++) {
                Vector3 pos = new Vector3(x,0,y);
                if (basicLand != null) {
                    grid[y, x] = Instantiate(basicLand, pos, Quaternion.Euler(-90,0,0));
                    float boop = Random.Range(0f,1f);
                    if (boop < 0.2f && enemyCount > 0) {
                        enemyCount--; 
                        GameObject dude = Instantiate(badGuy, pos, Quaternion.Euler(-90, 0, 0));
                        HightUpdate(dude);
                    }
                }
            }
        }
        
        playerStartingPos = new Vector3(Mathf.RoundToInt(gridSize.x/2), 0, 1);
        playerCurrentGrid = new Vector2(playerStartingPos.x, playerStartingPos.z); 
        playerStartingPos.y = grid[1, Mathf.CeilToInt(gridSize.x / 2)].gameObject.GetComponent<GroundScript>().height;
        player.transform.position = playerStartingPos;
        Invoke("PlayerSetUp", 0.02f);
    }
    void PlayerSetUp() {
        HightUpdate(player);
    }

    public bool canMove = true;

    public Vector2 playerCurrentGrid = new Vector2(); 
    private void Update() {
        if (moves > 0 && canMove) {
            PlayerMoveMent();
        } else {
            canMove = false; 
            EnemyMovement(); 
        }
        UIUpdated();
        PlayerCanMoveAgain();
    }
    public GameObject badGuy; 
    GameObject[] enemys;
    bool enemyMove = true;
    public int enemyMoves = 3; 
    void EnemyMovement() {
        enemys = GameObject.FindGameObjectsWithTag("BadGuy");
        if (enemyMove) {
            foreach (GameObject g in enemys) {
                    EnemyMoves(g);
            }
        }
    }
    void EnemyMoves(GameObject enemy) {
        StartCoroutine(WaitForMove(enemy)); 
    }

    IEnumerator WaitForMove(GameObject enemy) {

        yield return new WaitForSeconds(0.1f);
        int x = (int)Random.Range(1,5);
        Debug.Log(x); 
        switch (x) {
            case 1:
                //enemy.transform.rotation = Quaternion.Euler(-90, 0, -180);
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x, (int)enemy.transform.position.z + 1) && enemy.transform.position.z < gridSize.y - 1) {
                    enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z + 1);
                    HightUpdate(player);
                } else {
                    StartCoroutine(WaitForMove(enemy)); 
                }
                break;
            case 2:
                //enemy.transform.rotation = Quaternion.Euler(-90, 0, 0);
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x, (int)enemy.transform.position.z - 1) && enemy.transform.position.z > 1) {
                    enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z - 1);
                    HightUpdate(enemy);
                }else {
                    StartCoroutine(WaitForMove(enemy));
                }
        break;
            case 3:
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x + 1, (int)enemy.transform.position.z) && enemy.transform.position.x < gridSize.x - 1) {
                    enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + 1, enemy.transform.position.y, enemy.transform.position.z);
                    HightUpdate(enemy);
                } else {
                    StartCoroutine(WaitForMove(enemy));
                }
                break;

            case 4:
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x - 1, (int)enemy.transform.position.z) && enemy.transform.position.x > 1) {
                    enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x - 1, enemy.transform.position.y, enemy.transform.position.z);
                    HightUpdate(enemy);
                } else {
                    StartCoroutine(WaitForMove(enemy));
                }
                break;
            default:
                break; 

        }
        yield return new WaitForSeconds(0.1f);
        PlayerCanMoveAgain();
    }

    void PlayerCanMoveAgain() {
        if (moves <= 0 && !canMove) {
            moves = 3;
            canMove = true;
        }
    } 
    public Image[] moveCount;
    bool UIflash = true;
    public Color[] colorToUse; 
    void UIUpdated() {
            for (int i = 0; i <= 2; i++) {
                if (i < moves ) {
                    moveCount[i].GetComponent<Image>().color = colorToUse[0];
                } else {
                    moveCount[i].GetComponent<Image>().color = colorToUse[1];
                }
            } 
    }
    void PlayerMoveMent() {
        if (Input.GetKeyDown(KeyCode.W) && canMove && player.transform.position.z < gridSize.y - 1) {
            playerDisplay.transform.rotation = Quaternion.Euler(0, 0 ,0);
            if (CheckIfCanMoveThatWay((int)player.transform.position.x, (int)player.transform.position.z + 1)) {
                moves--; 
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
                HightUpdate(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && canMove && player.transform.position.z > 0) {
            playerDisplay.transform.rotation = Quaternion.Euler(0,-180, 0);
            if (CheckIfCanMoveThatWay((int)player.transform.position.x, (int)player.transform.position.z - 1)) {
                moves--;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
                HightUpdate(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && canMove && player.transform.position.x < gridSize.x - 1) {
            playerDisplay.transform.rotation = Quaternion.Euler(0, 90, 0);
            if (CheckIfCanMoveThatWay((int)player.transform.position.x + 1, (int)player.transform.position.z)) {
                moves--;
                player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
                HightUpdate(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.A) && canMove && player.transform.position.x > 0) {
            playerDisplay.transform.rotation = Quaternion.Euler(0, -90, 0);
            if (CheckIfCanMoveThatWay((int)player.transform.position.x - 1, (int)player.transform.position.z)) {
                moves--;
                player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
                HightUpdate(player);
            }
        }

    } 
    void HightUpdate (GameObject pawn) {

        GameObject ground = grid[(int)pawn.transform.position.z,(int)pawn.transform.position.x];
        pawn.transform.position = new Vector3((int)pawn.transform.position.x, ground.GetComponent<GroundScript>().height*2, (int)pawn.transform.position.z); 
    }

    bool CheckIfCanMoveThatWay(int x, int y) {

        bool moveIsPossible = true;
        if (x > gridSize.x || x < 0 || y > gridSize.y || y < 0) {
            return false; 
        }
        GameObject land = grid[y, x];
        if (land == null) {
            return false;
        }
        //land.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        moveIsPossible = land.GetComponent<GroundScript>().canMoveHere;
       
        return moveIsPossible; 
    }

}
