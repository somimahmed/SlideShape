using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    string websiteLink = "https://mukstargame.netlify.app/";
    public void OpenWebsite()
    {
        Application.OpenURL(websiteLink);
    } 
}
