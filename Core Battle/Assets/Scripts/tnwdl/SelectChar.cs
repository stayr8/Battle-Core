using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectChar : MonoBehaviour
{
    public GameObject SwordMan;
    public GameObject Archer;
    public GameObject SelectPanel;

    public Character character;
    public SelectChar[] chars;

    public void OnMouseUpAsBtn()
    {
        DataManager.instance.currentChar = character;
        OnSelect();

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != this) chars[i].OnDeSelect();
        }
    }
    public void GameStartBtn()
    {
        Debug.Log("게임 시작");
        LoadingSceneManager.LoadScene("TestScene");
    }
    private void OnSelect()
    {
        Debug.Log("선택한 캐릭터: " + character);

        if (character == Character.swordman)
        {
            SwordMan.SetActive(true);
        }
        else if (character == Character.archer)
        {
            Archer.SetActive(true);
        }
    }
    private void OnDeSelect()
    {
        if (character == Character.swordman)
        {
            SwordMan.SetActive(false);
        }
        else if (character == Character.archer)
        {
            Archer.SetActive(false);
        }
    }

    private void Start()
    {
        if (DataManager.instance.currentChar == character) OnSelect();
        else OnDeSelect();
    }
}
