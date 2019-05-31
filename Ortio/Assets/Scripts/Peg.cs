using UnityEngine;

public class Peg : MonoBehaviour
{
    Color[] colors = { Color.red, Color.blue, Color.green, new Color(0.94f, 0.66f, 0.2f, 1f) };

    public int size;
    public bool IsSelected;
    public int _player;

    public int Player
    {
        get { return _player; }
        set
        {
            _player = value;

            var material = GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", colors[_player] * 0.15f);
        }
    }

    public Game.Position gamePosition;

    private void Start()
    {

    }

    private void OnMouseUpAsButton()
    {
        if (gamePosition.isNest)
        {
            FindObjectOfType<GameController>().SetSelected(gameObject);
        }
    }

    void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    public void MoveTo(Game.Position position)
    {
        this.gamePosition = position;
        Vector3 target = Board.GetVectorForGamePosition(position);
        targetPosition = target;
    }

    Vector3 targetPosition = Vector3.negativeInfinity, targetVelocity;

    private void FixedUpdate()
    {
        if (IsSelected)
        {
            transform.Rotate(Vector3.forward, 20 * Time.deltaTime);
        }

        if (targetPosition.Equals(Vector3.negativeInfinity) == false && transform.localPosition.Equals(targetPosition) == false)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref targetVelocity, 0.1f, Constant.defaultDampSpeed, Time.deltaTime);
        }
    }


}
