using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    
    public GameObject[] pages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == index)
            {
                tabImages[i].color = Color.white;
                pages[i].SetActive(true);
            }
            else
            {
                tabImages[i].color = Color.gray;
                pages[i].SetActive(false);
            }
        }
    }
}
