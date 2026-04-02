using DG.Tweening;
using KJakub.Octave.UI.BlackScreen;
using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument editorLayout;
        [SerializeField]
        private Canvas gameLayout;
        [SerializeField]
        private Canvas albumSelectionLayout;
        [SerializeField]
        private Canvas levelSelectionLayout;
        [SerializeField]
        private Canvas resultsLayout;
        [SerializeField]
        private Canvas mainMenuLayout;
        [SerializeField]
        private BlackScreenUI blackScreen;
        [SerializeField]
        private Canvas settingsLayout;
        [SerializeField]
        private Canvas practiceModeLayout;
        [SerializeField]
        private Canvas achievementLayout;
        [SerializeField]
        private Canvas tutorialLayout;
        [SerializeField]
        private Canvas profileLayout;
        private void Awake()
        {
            editorLayout.gameObject.SetActive(true);
            ShowMainMenu();
            ShowGame();
            ShowAlbumSelectionMenu();
            ShowLevelSelectionMenu();
            ShowResults();
            ShowPracticeMode();
            ShowSettings();
            ShowAchievements();
            ShowTutorial();
            ShowProfile();

            HideSettings();
            HideGame();
            HideEditor();
            HideLevelSelectionMenu();
            HideAlbumSelectionMenu();
            HideResults();
            HidePracticeMode();
            HideAchievements();
            HideTutorial();
            HideProfile();
        }
        public void Transition(Action changeUI)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(blackScreen.Show());
            sequence.AppendCallback(() =>
            {
                changeUI?.Invoke();
            });
            sequence.AppendInterval(1f);
            sequence.Append(blackScreen.Hide());
        }
        public void ShowProfile()
        {
            profileLayout.gameObject.SetActive(true);
        }
        public void HideProfile()
        {
            profileLayout.gameObject.SetActive(false);
        }
        public void ShowTutorial()
        {
            tutorialLayout.gameObject.SetActive(true);
        }
        public void HideTutorial()
        {
            tutorialLayout.gameObject.SetActive(false);
        }
        public void ShowAchievements()
        {
            achievementLayout.gameObject.SetActive(true);
        }
        public void HideAchievements()
        {
            achievementLayout.gameObject.SetActive(false);
        }
        public void ShowSettings()
        {
            settingsLayout.gameObject.SetActive(true);
        }
        public void HideSettings()
        {
            settingsLayout.gameObject.SetActive(false);
        }
        public void ShowResults()
        {
            resultsLayout.gameObject.SetActive(true);
        }
        public void ShowPracticeMode()
        {
            practiceModeLayout.gameObject.SetActive(true);
        }
        public void HidePracticeMode()
        {
            practiceModeLayout.gameObject.SetActive(false);
        }
        public void ShowMainMenu()
        {
            mainMenuLayout.gameObject.SetActive(true);
        }
        public void HideMainMenu()
        {
            mainMenuLayout.gameObject.SetActive(false);
        }
        public void ShowEditor()
        {
            editorLayout.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        public void HideEditor()
        {
            editorLayout.rootVisualElement.style.display = DisplayStyle.None;
        }
        public void ShowGame()
        {
            gameLayout.gameObject.SetActive(true);
        }
        public void HideGame()
        {
            gameLayout.gameObject.SetActive(false);
        }
        public void HideResults()
        {
            resultsLayout.gameObject.SetActive(false);
        }
        public void ShowAlbumSelectionMenu()
        {
            albumSelectionLayout.gameObject.SetActive(true);
        }
        public void HideAlbumSelectionMenu()
        {
            albumSelectionLayout.gameObject.SetActive(false);
        }
        public void ShowLevelSelectionMenu()
        {
            levelSelectionLayout.gameObject.SetActive(true);
        }
        public void HideLevelSelectionMenu()
        {
            levelSelectionLayout.gameObject.SetActive(false);
        }
    }
}