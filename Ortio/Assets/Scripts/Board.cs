using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public Peg smallPegPrefab;
    public Peg mediumPegPrefab;
    public Peg largePegPrefab;
    public PickTarget pickTargetPrefab;

    public List<Peg> pegs = new List<Peg>();

    public Transform parent;

    static float offsetStable = 7f;
    static float offsetPeg = 3f;

    public static Vector3 GetVectorForGamePosition(Game.Position gamePosition)
    {
        if (gamePosition.isStable)
        {
            return GetVectorForStableIndex(gamePosition.player * 3 + gamePosition.x);
        }
        else
        {
            return GetVectorForBoardPosition(gamePosition.x, gamePosition.y);
        }
    }

    public static Vector3 GetVectorForBoardPosition(int x, int y)
    {
        return new Vector3(x * offsetPeg - offsetPeg, 0, y * offsetPeg - offsetPeg);
    }

    public static Vector3 GetVectorForBoardIndex(int index) 
    {
        int x = index % 3;
        int y = index / 3;
        return GetVectorForBoardPosition(x, y);
    }

    public static Vector3 GetVectorForStableIndex(int index)
    {
        switch(index)
        {
            case 0: return new Vector3(-offsetPeg, 0, offsetStable);
            case 1: return new Vector3(0, 0, offsetStable);
            case 2: return new Vector3(offsetPeg, 0, offsetStable);

            case 3: return new Vector3(-offsetStable, 0, -offsetPeg);
            case 4: return new Vector3(-offsetStable, 0, 0);
            case 5: return new Vector3(-offsetStable, 0, offsetPeg);

            case 6: return new Vector3(-offsetPeg, 0, -offsetStable);
            case 7: return new Vector3(0, 0, -offsetStable);
            case 8: return new Vector3(offsetPeg, 0, -offsetStable);

            case 9: return new Vector3(offsetStable, 0, -offsetPeg);
            case 10: return new Vector3(offsetStable, 0, 0);
            case 11: return new Vector3(offsetStable, 0, offsetPeg);
        }
        Debug.LogError("Index out of bounds");
        return new Vector3();
    }

    public void PlaceTargetAtPosition(Vector3 position, int index)
    {
        var target = GameObject.Instantiate(pickTargetPrefab, parent);
        target.index = index;
        var targetOffset = new Vector3(0, target.transform.position.y, 0);
        target.transform.position = targetOffset + position;
    }

    Peg CreatePeg(int size)
    {
        Peg create()
        {
            switch (size)
            {
                case 0: return GameObject.Instantiate(smallPegPrefab, parent);
                case 1: return GameObject.Instantiate(mediumPegPrefab, parent);
                case 2: return GameObject.Instantiate(largePegPrefab, parent);
            }

            throw new System.Exception(string.Format("Invalid peg size ({0})", size));
        }
        var peg = create();
        pegs.Add(peg);
        return peg;
    }

    public Peg SpawnPeg(int player, Game.Position position)
    {
        var peg = CreatePeg(position.size);
        peg.transform.localPosition = GetVectorForGamePosition(position);
        peg.Player = player;
        peg.gamePosition = position;
        return peg;
    }

    public void PlacePegAtBoardIndex(Peg peg, int index)
    {
        var position = GetVectorForBoardIndex(index);
        peg.transform.localPosition = position;
    }

    public void perform(Game.Move move)
    {
        var peg = pegs.Find((obj) => move.fromPosition.Equals(obj.gamePosition));
        if (peg == null)
        {
            Debug.LogWarningFormat("Could not find the peg at {0}", move.fromPosition);
            return;
        }

        peg.MoveTo(move.toPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Game.Position position in Game.Position.all)
        {
            if (position.isNest)
            {
                continue;
            }
            PlaceTargetAtPosition(GetVectorForGamePosition(position), position.xy);
        }
    }

    private void Awake()
    {
        var gameController = GameObject.FindObjectOfType<GameController>();
        parent = gameController.transform;
    }

}
