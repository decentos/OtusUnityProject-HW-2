using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public CanvasGroup buttonsCanvasGroup;
    public CanvasGroup pauseMenuCanvasGroup;
    public CanvasGroup pauseButtonCanvasGroup;
    public CanvasGroup playerWinCanvasGroup;
    public CanvasGroup playerLoseCanvasGroup;
    public Button switchButton;
    [SerializeField] private Character[] playerCharacters = default;
    [SerializeField] private Character[] enemyCharacters = default;
    Character currentTarget;
    bool waitingForInput;
    bool isPause;

    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        Utility.SetCanvasGroupEnabled(playerWinCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(playerLoseCanvasGroup, false);
        switchButton.onClick.AddListener(NextTarget);
        StartCoroutine(GameLoop());
    }

    public void PlayerAttack()
    {
        waitingForInput = false;
    }

    public void PauseMenu()
    {
        isPause = true;
        Utility.SetCanvasGroupEnabled(pauseButtonCanvasGroup, false);
    }

    public void ResumeGame()
    {
        isPause = false;
        Utility.SetCanvasGroupEnabled(pauseMenuCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(pauseButtonCanvasGroup, true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Utility.SetCanvasGroupEnabled(pauseButtonCanvasGroup, true);
    }

    public void ExitToMainMenu()
    {
        isPause = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void NextTarget()
    {
        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            // Найти текущего персонажа (i = индекс текущего)
            if (enemyCharacters[i] == currentTarget)
            {
                int start = i;
                ++i;
                // Идем в сторону конца массива и ищем живого персонажа
                for (; i < enemyCharacters.Length; i++)
                {
                    if (enemyCharacters[i].IsDead())
                        continue;

                    // Нашли живого, меняем currentTarget
                    currentTarget.targetIndicator.gameObject.SetActive(false);
                    currentTarget = enemyCharacters[i];
                    currentTarget.targetIndicator.gameObject.SetActive(true);

                    return;
                }
                // Идем от начала массива до текущего и смотрим, если там кто живой
                for (i = 0; i < start; i++)
                {
                    if (enemyCharacters[i].IsDead())
                        continue;

                    // Нашли живого, меняем currentTarget
                    currentTarget.targetIndicator.gameObject.SetActive(false);
                    currentTarget = enemyCharacters[i];
                    currentTarget.targetIndicator.gameObject.SetActive(true);

                    return;
                }
                // Живых больше не осталось, не меняем currentTarget
                return;
            }
        }
    }

    Character FirstAliveCharacter(Character[] characters)
    {
        foreach (var character in characters)
        {
            if (!character.IsDead())
                return character;
        }
        return null;
    }

    void PlayerWon()
    {
        Debug.Log("Player won");
        Utility.SetCanvasGroupEnabled(playerWinCanvasGroup, true);
    }

    void PlayerLost()
    {
        Debug.Log("Player lost");
        Utility.SetCanvasGroupEnabled(playerLoseCanvasGroup, true);
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacters) == null)
        {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacters) == null)
        {
            PlayerWon();
            return true;
        }

        return false;
    }

    IEnumerator GameLoop()
    {
        Utility.SetCanvasGroupEnabled(buttonsCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(pauseMenuCanvasGroup, false);

        while (!CheckEndGame())
        {
            while (isPause)
            {
                Utility.SetCanvasGroupEnabled(buttonsCanvasGroup, false);
                Utility.SetCanvasGroupEnabled(pauseMenuCanvasGroup, true);
                yield return null;
            }

            foreach (var player in playerCharacters)
            {
                if (player.IsDead())
                    continue;

                currentTarget = FirstAliveCharacter(enemyCharacters);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);

                Utility.SetCanvasGroupEnabled(buttonsCanvasGroup, true);
                waitingForInput = true;
                while (waitingForInput)
                {
                    if (isPause)
                    {
                        waitingForInput = false;
                        Utility.SetCanvasGroupEnabled(pauseMenuCanvasGroup, true);
                        yield return null;
                    }
                    yield return null;
                }
                Utility.SetCanvasGroupEnabled(buttonsCanvasGroup, false);

                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.target = currentTarget.transform;
                player.AttackEnemy();
                while (!player.IsIdle())
                {
                    if (isPause)
                    {
                        player.SetState(Character.State.Idle);
                        yield return null;
                    }
                    yield return null;
                }
            }

            foreach (var enemy in enemyCharacters)
            {
                if (enemy.IsDead())
                    continue;

                Character target = FirstAliveCharacter(playerCharacters);
                if (target == null)
                    break;

                enemy.target = target.transform;
                enemy.AttackEnemy();
                while (!enemy.IsIdle())
                {
                    if (isPause)
                    {
                        enemy.SetState(Character.State.Idle);
                        Utility.SetCanvasGroupEnabled(pauseMenuCanvasGroup, true);
                        yield return null;
                    }
                    yield return null;
                }
            }
        }
    }
}
