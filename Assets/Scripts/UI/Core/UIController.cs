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
        private UIDocument mainMenuLayout;
        [SerializeField]
        private BlackScreenUI blackScreen;
        [SerializeField]
        private Canvas settingsLayout;
        [SerializeField]
        private Canvas practiceModeLayout;
        private void Awake()
        {
            editorLayout.gameObject.SetActive(true);
            mainMenuLayout.gameObject.SetActive(true);
            ShowGame();
            ShowAlbumSelectionMenu();
            ShowLevelSelectionMenu();
            ShowResults();
            ShowPracticeMode();
            ShowSettings();

            ShowMainMenu();
            HideSettings();
            HideGame();
            HideEditor();
            HideLevelSelectionMenu();
            HideAlbumSelectionMenu();
            HideResults();
            HidePracticeMode();
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
            mainMenuLayout.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        public void HideMainMenu()
        {
            mainMenuLayout.rootVisualElement.style.display = DisplayStyle.None;
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