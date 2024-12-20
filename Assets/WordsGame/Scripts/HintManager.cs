using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject keyboard;
    private KeyboardKey[] keys;

    [Header(" Text Elements ")]
    [SerializeField] private TextMeshProUGUI keyboardPriceText;
    [SerializeField] private TextMeshProUGUI letterPriceText;

    [Header(" Settings ")]
    [SerializeField] private int keyboardHintPrice;
    [SerializeField] private int letterHintPrice;

    private bool shouldReset;

    private void Awake()
    {
        keys = keyboard.GetComponentsInChildren<KeyboardKey>();
    }
    // Start is called before the first frame update
    void Start()
    {
        keyboardPriceText.text = keyboardHintPrice.ToString();
        letterPriceText.text = letterHintPrice.ToString();

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:

                break;

            case GameState.Game:

                if (shouldReset)
                {
                    LetterHintGivenIndices.Clear();
                    shouldReset = false;
                }

                break;

            case GameState.LevelComplete:

                shouldReset = true;
                break;

            case GameState.GameOver:

                shouldReset = true;
                break;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyboardHint()
    {
        if (DataManager.instance.GetCoins() < keyboardHintPrice)
            return;

        string secretWord = WordManager.instance.GetSecretWord();

        List<KeyboardKey> untouchedKey = new List<KeyboardKey>();

        for (int i = 0; i < keys.Length; i++)
            if (keys[i].IsUntouched())
                untouchedKey.Add(keys[i]);

        List<KeyboardKey> t_untouchedKey = new List<KeyboardKey>(untouchedKey);

        for (int i = 0; i < untouchedKey.Count; i++)
            if (secretWord.Contains(untouchedKey[i].GetLetter()))
                t_untouchedKey.Remove(untouchedKey[i]);

        if (t_untouchedKey.Count == 0)
            return;

        int randomKeyIndex = Random.Range(0, t_untouchedKey.Count);
        t_untouchedKey[randomKeyIndex].SetInvalid();

        DataManager.instance.RemoveCoins(keyboardHintPrice);

    }

    List<int> LetterHintGivenIndices = new List<int>();
    public void LetterHint()
    {
        if (DataManager.instance.GetCoins() < letterHintPrice)
            return;

        if (LetterHintGivenIndices.Count >= 5)
        {
            Debug.Log("All hints");
            return;
        }

        List<int> LetterHintNotGivenIndices = new List<int>();

        for (int i = 0; i < 5; i++)
            if (!LetterHintGivenIndices.Contains(i))
                LetterHintNotGivenIndices.Add(i);

        WordContainer currentWordContainer = InputManager.instance.GetCurrentWordContainer();

        string secretWord = WordManager.instance.GetSecretWord();

        int randomIndex = LetterHintNotGivenIndices[Random.Range(0, LetterHintNotGivenIndices.Count)];
        LetterHintGivenIndices.Add(randomIndex);

        currentWordContainer.AddAsHint(randomIndex, secretWord[randomIndex]);

        DataManager.instance.RemoveCoins(letterHintPrice);
    }
}
