using UnityEngine;
public class GameController : MonoBehaviour
{
    public bool GameEnded { get; private set; }

    public void Win()
    {
        GameEnded = true;
        Debug.Log("Win");
    }

    public void Lose()
    {
        GameEnded = true;
        Debug.Log("Lose");
    }
}
