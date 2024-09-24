using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Variables

    // Scripts
    [SerializeField] PreGameController preGameController;

    #endregion

    #region ButtonEventHandler

    // Method to capture while button is pressed
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine("SkipPreGame");
    }

    // Method to capture when button is released
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("SkipPreGame");
    }

    #endregion

    #region PreGameHandler

    // Coroutine to be called when button is pressed
    IEnumerator SkipPreGame()
    {
        // Wait for 2 seconds
        Debug.Log("0");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");

        // After 2 seconds, call ChooseCharacterPanel method
        preGameController.ChooseCharacterPanel();
    }

    #endregion


}
