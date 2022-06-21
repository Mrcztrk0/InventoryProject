using UnityEngine;

public class Links : MonoBehaviour
{
    [SerializeField] private GameObject Envanterpanel;
    [SerializeField] private GameObject Casepanel;
    [SerializeField] private GameObject EnvanterButton;
    [SerializeField] private GameObject EnvanterdenCıkısButton;
    //[SerializeField] private GameObject caseButton;
    //[SerializeField] private GameObject CaseCıkısButton;

    //public Animator animator;
    //private SCinventory slot;
    //test1
 
    public void OpenEnvanterPanel()
    {
        //animator.Play("EnvanterKamera");
        Envanterpanel.SetActive(true);
        EnvanterButton.SetActive(false);
        //caseButton.SetActive(false);
        EnvanterdenCıkısButton.SetActive(true);
    }

    public void ExitEnvanterPanel()
    {
        //animator.Play("AnaKamera");
        Envanterpanel.SetActive(false);
        EnvanterButton.SetActive(true);
        EnvanterdenCıkısButton.SetActive(false);
        //caseButton.SetActive(false);
    }

    public void OpenCasePanel()
    {
        //animator.Play("EnvanterKamera");
        Envanterpanel.SetActive(true);
        //Casepanel.SetActive(true);
        EnvanterButton.SetActive(false);
        EnvanterdenCıkısButton.SetActive(false);
        //caseButton.SetActive(false);
    } 

    public void ExitCasePanel()
    {
        //animator.Play("AnaKamera");
        Envanterpanel.SetActive(false);
        //Casepanel.SetActive(false);
        EnvanterButton.SetActive(true);
        //CaseCıkısButton.SetActive(true);
        //caseButton.SetActive(false);
    }          
}
