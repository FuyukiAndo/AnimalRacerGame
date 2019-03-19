using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour {

    public int numberOfPlayer;
    public GameObject characterFrame;
    private void OnEnable()
    {
        characterFrame.SetActive(false);
    }
    public void SetSelectedCharacterFrame(Transform imageTransform)
    {
        switch (numberOfPlayer)
        {
            case 1:
                if(InformationManager.Instance.player1.character != null)
                {
                    characterFrame.SetActive(true);
                }
                characterFrame.transform.position = imageTransform.position;
                break;
            case 2:
                if (InformationManager.Instance.player2.character != null)
                {
                    characterFrame.SetActive(true);
                }
                characterFrame.transform.position = imageTransform.position;
                break;
            case 3:
                if (InformationManager.Instance.player3.character != null)
                {
                    characterFrame.SetActive(true);
                }
                characterFrame.transform.position = imageTransform.position;
                break;
            case 4:
                if (InformationManager.Instance.player4.character != null)
                {
                    characterFrame.SetActive(true);
                }
                characterFrame.transform.position = imageTransform.position;
                break;
        }
    }
}
