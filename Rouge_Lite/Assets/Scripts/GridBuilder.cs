using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GridBuilder : MonoBehaviour {
    public GameObject basicLand, safeLand, player, playerDisplay;
    public GameObject[,] grid = new GameObject[50, 50];
    public Vector2 gridSize;
    public Vector3 playerStartingPos;
    public int enemyCount = 1;
    int moves = 3;
    int health = 3;
    public int enemysSpawned = 0;
    public int enemysCought = 0;   
    public static GameObject gm;
    private void Start() {
        gm = this.gameObject;
        gridSize = new Vector2(GameMaster.level * 3, GameMaster.level * 3);
        playerCurrentGrid = new Vector2(playerStartingPos.x, playerStartingPos.z);
        playerStartingPos = new Vector3(Mathf.RoundToInt(gridSize.x / 2), 0, 1);
        playerCurrentGrid = new Vector2(playerStartingPos.x, playerStartingPos.z);
        player.transform.position = playerStartingPos;
        health = GameMaster.playerhealth;
        if (gridSize.x > 20) {

            gridSize.y = 20;
            gridSize.x = 20; 
        }

        enemyCount = GameMaster.level; 
        for (int y = 0; y < gridSize.y; y++) {
            for (int x = 0; x < gridSize.x; x++) {
                Vector3 pos = new Vector3(x, 0, y);
                if (basicLand != null) {
                    if (pos != playerStartingPos) {
                        grid[y, x] = Instantiate(basicLand, pos, Quaternion.Euler(-90, 0, 0));
                        grid[y, x].transform.SetParent(this.gameObject.transform);
                    } else {
                        grid[y, x] = Instantiate(safeLand, pos, Quaternion.Euler(-90, 0, 0));
                        grid[y, x].transform.SetParent(this.gameObject.transform);
                    } 


                }
            }
        }
        Vector2[] currentRange = new Vector2[enemyCount];
        for (int i = 0; i < currentRange.Length; i++) {
            currentRange[i] = Vector2.zero;
        }
        int currnetPoint = 0;
        while (enemyCount > 0) {

            Vector2 gridPos = new Vector2(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));

            if (grid[(int)gridPos.x, (int)gridPos.y].GetComponent<GroundScript>().canMoveHere == true) {
                bool canSpawn = true;
                for (int i = 0; i < currentRange.Length; i++) {
                    if (gridPos == currentRange[i]) {
                        canSpawn = false;
                        currnetPoint++;
                    }
                }
                if (canSpawn) {
                    currentRange[currnetPoint] = gridPos;
                    Vector3 enemyStartingPos = new Vector3(Mathf.RoundToInt(gridPos.x), 0, Mathf.RoundToInt(gridPos.y));
                    enemyCount--;
                    enemysSpawned++; 
                    GameObject dude = Instantiate(badGuy, grid[(int)gridPos.x, (int)gridPos.y].transform.position, Quaternion.Euler(-90, 0, 0));
                    HightUpdate(dude);
                }
            }


            Invoke("PlayerSetUp", 0.02f);
            //playerControler = player.GetComponent<Animator>(); 
            enemys = GameObject.FindGameObjectsWithTag("BadGuy");
            if (enemyMove) {
                foreach (GameObject g in enemys) {
                    HightUpdate(g);
                }
            }
        }
        UpdateHeath();
    }

    void PlayerSetUp() {
        HightUpdate(player);
    }

    public Animator playerControler; 
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
        FineMovement();

        if (Input.GetKeyDown(KeyCode.F)) {
            Damage(); 

        }
        if (enemysSpawned == enemysCought && canLoad) {
            canLoad = false; 
            Debug.Log("GameOver");
            GameMaster.level++;
            StartCoroutine(LoadNextLevel("LevelOne", true));
        }
    }
    bool canLoad = true;
    public GameObject Victory; 
    IEnumerator LoadNextLevel(string x, bool win) {
        if(win){
            Victory.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(x);
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
                    g.GetComponent<BadGuy>().Flash(); 
            }
        }
    }
    void EnemyMoves(GameObject enemy) {
        StartCoroutine(WaitForMove(enemy)); 
    }

    IEnumerator WaitForMove(GameObject enemy) {

        yield return new WaitForSeconds(0.1f);
        int x = (int)Random.Range(1,5);
        switch (x) {
            case 1:
                //enemy.transform.rotation = Quaternion.Euler(-90, 0, -180);
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x, (int)enemy.transform.position.z + 1) && enemy.transform.position.z < gridSize.y - 1) {
                    //enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z + 1);
                    HightUpdate(player);
                } else {
                    StartCoroutine(WaitForMove(enemy)); 
                }
                break;
            case 2:
                //enemy.transform.rotation = Quaternion.Euler(-90, 0, 0);
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x, (int)enemy.transform.position.z - 1) && enemy.transform.position.z > 1) {
                    //enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z - 1);
                    HightUpdate(enemy);
                }else {
                    StartCoroutine(WaitForMove(enemy));
                }
        break;
            case 3:
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x + 1, (int)enemy.transform.position.z) && enemy.transform.position.x < gridSize.x - 1) {
                    //enemy.GetComponent<BadGuy>().moves--;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + 1, enemy.transform.position.y, enemy.transform.position.z);
                    HightUpdate(enemy);
                } else {
                    StartCoroutine(WaitForMove(enemy));
                }
                break;

            case 4:
                if (CheckIfCanMoveThatWay((int)enemy.transform.position.x - 1, (int)enemy.transform.position.z) && enemy.transform.position.x > 1) {
                    //enemy.GetComponent<BadGuy>().moves--;
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
            EnemyPosCheck();
        }
    }
    public void EnemyCought() {
        enemysCought++;

    }
    void FineMovement() {

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            direction = 0;
            playerDisplay.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            direction = 1;
            playerDisplay.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            direction = 2;
            playerDisplay.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = 3;
            playerDisplay.transform.rotation = Quaternion.Euler(0, -90, 0);
        }



    }
    public Image[] moveCount;
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
    int direction = 0; 
    void PlayerMoveMent() {
        playerControler.SetBool("Attack", false);
        if (Input.GetKeyDown(KeyCode.W) && canMove && player.transform.position.z < gridSize.y - 1) {
            direction = 0; 
            playerDisplay.transform.rotation = Quaternion.Euler(0, 0 ,0);
            if (CheckIfCanMoveThatWay((int)player.transform.position.x, (int)player.transform.position.z + 1)) {
                moves--; 
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
                HightUpdate(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && canMove && player.transform.position.z > 0) {
            playerDisplay.transform.rotation = Quaternion.Euler(0,-180, 0);
            direction = 1;
            if (CheckIfCanMoveThatWay((int)player.transform.position.x, (int)player.transform.position.z - 1)) {
                moves--;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
                HightUpdate(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && canMove && player.transform.position.x < gridSize.x - 1) {
            playerDisplay.transform.rotation = Quaternion.Euler(0, 90, 0);
            direction = 2;
            if (CheckIfCanMoveThatWay((int)player.transform.position.x + 1, (int)player.transform.position.z)) {
                moves--;
                player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
                HightUpdate(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.A) && canMove && player.transform.position.x > 0) {
            playerDisplay.transform.rotation = Quaternion.Euler(0, -90, 0);
            direction = 3;
            if (CheckIfCanMoveThatWay((int)player.transform.position.x - 1, (int)player.transform.position.z)) {
                moves--;
                player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
                HightUpdate(player);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            moves--; 
            playerControler.SetBool("Attack", true);
            DropNet();
        }

    }

    public GameObject net; 
    void DropNet() {
        Vector3 dropPoint;
        switch (direction) {
            // up
            case 0:
                dropPoint = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z + 1);
                Instantiate(net, dropPoint, Quaternion.identity);
                break;
            //down
            case 1:
                dropPoint = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z - 1);
                Instantiate(net, dropPoint, Quaternion.identity);
                
                break;
            //right
            case 2:
                dropPoint = new Vector3(player.transform.position.x + 1, player.transform.position.y + 2, player.transform.position.z);
                Instantiate(net, dropPoint, Quaternion.identity);
                break;
            //left
            case 3:
                dropPoint = new Vector3(player.transform.position.x - 1, player.transform.position.y + 2, player.transform.position.z);
                Instantiate(net, dropPoint, Quaternion.identity);
                break;


        }

    }
    void HightUpdate(GameObject pawn) {
        
        if (pawn.tag == "BadGuy") {
            GameObject ground = grid[(int)pawn.transform.position.x, (int)pawn.transform.position.z];
            pawn.transform.position = new Vector3((int)pawn.transform.position.x, 2, (int)pawn.transform.position.z);
        } else {
            GameObject ground = grid[(int)pawn.transform.position.z, (int)pawn.transform.position.x];
            pawn.transform.position = new Vector3((int)pawn.transform.position.x, ground.GetComponent<GroundScript>().height * 2, (int)pawn.transform.position.z);
        }
       // ground.SetActive(false);
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
    public string gameOver; 
    public GameObject[] heathMarks; 
    public void Damage() {

        health--;
        GameMaster.playerhealth--;
        UpdateHeath();
    }

    void UpdateHeath() {
        for (int i = 0; i < heathMarks.Length; i++) {

            if (i <= health - 1) {
                heathMarks[i].SetActive(true);
            } else {
                heathMarks[i].SetActive(false);
            }
            if (health <= 0 && canLoad) {
                canLoad = false; 
                canMove = false;
                playerDisplay.SetActive(false);
                StartCoroutine(LoadNextLevel(gameOver, false));
            }

        }
    } 

    void EnemyPosCheck() {
        Vector3 enemyPos, playerPos;
        playerPos = player.transform.position;
        foreach (GameObject g in enemys) {
            enemyPos = g.transform.position;
            if (playerPos.x < enemyPos.x) {
                StartCoroutine(FlashBorder(right));
            }
            if (playerPos.x > enemyPos.x) {
                StartCoroutine(FlashBorder(left));
            }
            if (playerPos.z > enemyPos.z) {
                StartCoroutine(FlashBorder(down));
            }
            if (playerPos.z < enemyPos.z) {
                StartCoroutine(FlashBorder(up));
            }
        }
    }



    public GameObject up, down, left, right; 
    public IEnumerator FlashBorder(GameObject me) {
        bool canSee = true; 
        for (int i = 0; i < 5; i++) {
            canSee = !canSee;
            yield return new WaitForSeconds(0.25f);
            me.SetActive(canSee);
        }
        me.SetActive(false);
    }


} 

