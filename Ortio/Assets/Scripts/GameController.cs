using UnityEngine;
using UnityEngine.SceneManagement;
using Game;
using UnityEngine.Events;
using UnityEngine.UI;

public class Constant
{
    public static float defaultDampSpeed = 900f;
}

public class GameController : MonoBehaviour, Logic.Listener
{
    Game.Logic gameLogic;

    public GameObject userInterface;

    public Board board;
    public Hilight spotLight;
    int playerTurn = 0;

    private bool _waitingPlayerUpdate = false;

    public bool IsUserInteractionEnabled => 
        _waitingPlayerUpdate == false && 
        gameLogic.GameIsOver == false;

    GameObject selectedObj;

    private void Awake()
    {
        gameLogic = new Game.Logic(new Game.State());
        gameLogic.listeners.Add(this);

        var boardData = gameLogic.CurrentState.RawBoardData;
        
        foreach (Position position in Position.all) 
        {
            int player = boardData[position.index];
            if (player == -1)
            {
                continue;
            }

            var peg = board.SpawnPeg(player, position);
        }
    }

    void DeselectCurrent()
    {
        Deselect(selectedObj);
        selectedObj = null;
    }

    void Deselect(GameObject obj)
    {
        if (obj == null) { return; };
        obj.SendMessage("SetSelected", false);
        Hilight(null, 0);
    }

    void Select(GameObject obj)
    {
        if (obj == null) { return; };
        obj.SendMessage("SetSelected", true);
        selectedObj = obj;

        var peg = obj.GetComponent<Peg>();
        if (peg != null)
        {
            Hilight(obj, 5 + 10 * peg.size);
        }
    }

    public void SetSelected(GameObject obj)
    {
        if (IsUserInteractionEnabled == false) {
            Debug.LogWarning("User interaction is disabled");
            return; 
        }

        if (!CanSelectPeg(obj))
        {
            return;
        }

        // Same object picked == just deselect;
        if (selectedObj == obj)
        {
            DeselectCurrent();
            return;
        }

        Deselect(selectedObj);
        selectedObj = obj;
        Select(selectedObj);
    }

    bool CanSelectPeg(GameObject obj)
    {
        var peg = obj.GetComponent<Peg>();
        if (peg == null)
        {
            return true;
        }

        if (peg.Player != gameLogic.CurrentPlayer)
        {
            Debug.LogErrorFormat("Peg player ({0}) is not the same as the current player ({1}).",
                peg.Player, gameLogic.CurrentPlayer);
            return false;
        }

        return true;
    }

    void Hilight(GameObject obj, float size)
    {
        if (obj != null)
        {
            spotLight.target = obj.transform;
            spotLight.size = size;
        }
    }

    public void PickTargetTouched(PickTarget target)
    {
        if (IsUserInteractionEnabled == false) {
            Debug.LogWarning("User interaction is disabled");
            return; 
        }

        if (selectedObj != null)
        {
            var peg = selectedObj.GetComponent<Peg>();
            var position = new Game.Position(peg.size, target.index);

            if (gameLogic.PlayTurn(peg.gamePosition, position))
            {
                DeselectCurrent();
                Hilight(target.gameObject, spotLight.size);

                if (gameLogic.HasWinner)
                {
                    Debug.Log("There's a winner!");
                }

                
            }
        }
    }

    void Logic.Listener.OnPerformMove(Move move)
    {
        board.perform(move);
    }

    void Logic.Listener.OnCurrentPlayerChanged()
    {
        _waitingPlayerUpdate = true;
        Invoke("IncrementPlayerTurn", 1f);
        Invoke("ReadyNextTurn", 1.7f);
    }

    void Logic.Listener.OnGameStateChanged()
    {
        UpdateFromState();
    }

    void UpdateFromState()
    {
        int pegIndex = 0;
        var boardData = gameLogic.CurrentState.RawBoardData;
        playerTurn = gameLogic.CurrentPlayer;

        foreach (Position position in Position.all)
        {
            int player = boardData[position.index];
            if (player == -1)
            {
                continue;
            }

            if (pegIndex >= board.pegs.Count)
            {
                Debug.LogErrorFormat("Pegs array out of bounds ({0})", pegIndex);
            }
            var peg = board.pegs[pegIndex++];

            peg.Player = player;
            peg.MoveTo(position);
        }
    }

    void perform(Game.Move move)
    {
        board.perform(move);
    }

    void IncrementPlayerTurn()
    {
        playerTurn = gameLogic.CurrentPlayer;
    }

    void ReadyNextTurn()
    {
        _waitingPlayerUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateFromState();
            //SceneManager.LoadScene(0);
            //   pegMedium.transform.localPosition = Vector3.left * stable + Vector3.up * offset;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (userInterface.activeSelf)
            {
                if (!gameLogic.GameIsOver)
                {
                    userInterface.SetActive(false);
                }
            }
            else
            {
                userInterface.SetActive(true);
            }
        }
    }

    float currentRotationVelocity = 0f;
    private void FixedUpdate()
    {
        var target = 180f + playerTurn * 90f;
        var current = transform.rotation.eulerAngles;
        var speed = 900f;

        //var angle = Mathf.MoveTowardsAngle(current.y, target, speed * Time.deltaTime);
        var angle = Mathf.SmoothDampAngle(current.y, target, ref currentRotationVelocity, 0.2f, speed);

        transform.eulerAngles = new Vector3(current.x, angle, current.z);

    }

    public void StartNewGame(int playerCount)
    {
        gameLogic.Reset(playerCount);
        userInterface.SetActive(false);
    }
}
