using Dreamteck.Splines;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winningUIPanel;
    public GameObject losingUIPanel;
    private bool gameEnded = false;
    public SplineComputer _spline;
    private float _score = 0;
    public GameObject _scoreObject;
    public void WinGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            winningUIPanel.SetActive(true);
            // Additional win game logic here
        }
    }
    public void addScore(int x)
    {
        if (_score > 4)
        _score += x;
    }
    private void Update()
    {
        if (!gameEnded)
        {
            _score += 7 * Time.deltaTime * 2;
            _scoreObject.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)(_score)).ToString();
        }
    }
    
    public void LoseGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            losingUIPanel.SetActive(true);
            Destroy(_spline.gameObject);
        }
    }
}
