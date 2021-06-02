using UnityEngine;
public class GameController : MonoBehaviour
{
    public bool GameEnded { get; private set; }

    public void Win()
    {
        if (GameEnded) return;

        GameEnded = true;
        Debug.Log("Win");
    }

    public void Lose()
    {
        if (GameEnded) return;

        GameEnded = true;
        Debug.Log("Lose");
    }
}
