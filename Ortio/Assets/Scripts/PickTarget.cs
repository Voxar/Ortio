using UnityEngine;

public class PickTarget : MonoBehaviour
{
    public int index;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnMouseUpAsButton()
    {
        gameController.PickTargetTouched(this);
    }
}
