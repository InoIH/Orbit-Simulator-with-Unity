using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

    [SerializeField] private Button oneBodyStartButton;
    [SerializeField] private Button twoBodyStartButton;
    [SerializeField] private Button threeBodyStartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oneBodyStartButton.onClick.AddListener(loadOneBody);
        twoBodyStartButton.onClick.AddListener(loadTwoBody);
        threeBodyStartButton.onClick.AddListener(loadThreeBody);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadOneBody()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void loadTwoBody()
    {
        SceneManager.LoadScene("Binary");
    }

    public void loadThreeBody()
    {
        SceneManager.LoadScene("Trinary");
    }

}
