using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

    [SerializeField] private Button oneBodyStartButton;
    [SerializeField] private Button twoBodyStartButton;
    [SerializeField] private Button threeBodyStartButton;
    [SerializeField] private Button verletOneBody;
    [SerializeField] private Button verletThreeBody;
    [SerializeField] private Button verletFourBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oneBodyStartButton.onClick.AddListener(loadOneBody);
        twoBodyStartButton.onClick.AddListener(loadTwoBody);
        threeBodyStartButton.onClick.AddListener(loadThreeBody);
        verletOneBody.onClick.AddListener(loadVerletOneBody);
        verletThreeBody.onClick.AddListener(loadVerletThreeBody);
        verletFourBody.onClick.AddListener(loadVerletFourBody);
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

    public void loadVerletOneBody()
    {
        SceneManager.LoadScene("Verlet");
    }
    public void loadVerletThreeBody()
    {
        SceneManager.LoadScene("3bodyVerlet");
    }
    public void loadVerletFourBody()
    {
        SceneManager.LoadScene("4bodyVerlet");
    }

}
