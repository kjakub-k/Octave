using DG.Tweening;
using KJakub.Octave.Data;
using KJakub.Octave.Game.Lines;
using KJakub.Octave.Managers.JsonManager;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.UI.AlbumSelect;
using KJakub.Octave.UI.Core;
using KJakub.Octave.UI.LevelSelect;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Tutorial
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private Image imgBlocker; //for blocking input
        [SerializeField]
        private Image spotLight; //for showing spotlight on things in question
        [SerializeField]
        private TMP_Text continueBtnLabel;
        [Header("Dialogue")]
        [SerializeField]
        private TMP_Text dialogueLabel;
        [SerializeField]
        private Image progressBarDialogue;
        [SerializeField]
        private Button nextButton;
        [SerializeField]
        private float durationBetweenLetters = 0.05f;
        [SerializeField]
        private float maxProgress = 1f;
        [Header("UIs / Managers")]
        [SerializeField]
        private AlbumSelectUI albumSelectUI;
        [SerializeField]
        private LevelSelectUI levelSelectUI;
        [SerializeField]
        private LineManager lineManager;
        [Header("DOTween Settings")]
        [SerializeField]
        private float moveDuration = 1f;
        [SerializeField]
        private Ease moveEase = Ease.Linear;
        private float progress = 0f;
        private bool textFinishedGenerating = false;
        private void OnEnable()
        {
            Translate();
        }
        private void Translate()
        {
            continueBtnLabel.text = LanguageManager.GetTranslation("continue");
        }
        private void Update()
        {
            if (textFinishedGenerating)
            {
                if (progress < maxProgress)
                {
                    progress += Time.deltaTime;
                    progressBarDialogue.fillAmount = progress / maxProgress;
                }
                else
                    EnableBtn();                
            }
        }
        private void MoveSpotLight(Vector2 scale, Vector2 position)
        {
            Sequence seq = DOTween.Sequence();
            seq.Join(spotLight.gameObject.transform.DOScale(scale, moveDuration));
            seq.Join(spotLight.gameObject.transform.DOMove(position, moveDuration));
            seq.SetEase(moveEase);
            seq.Play();
        }
        private void AssignNextFuncToBtn(UnityAction call)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(call);
        }
        private void EnableBtn()
        {
            nextButton.interactable = true;
        }
        private void ShowDialogue(string message)
        {
            StartCoroutine(TypeOutMessage(message));
            progress = 0;
            progressBarDialogue.fillAmount = progress;
            nextButton.interactable = false;
        }
        private IEnumerator TypeOutMessage(string message)
        {
            textFinishedGenerating = false;
            dialogueLabel.text = "";

            char[] letters = message.ToCharArray();
            int index = 0;

            while (index < letters.Length)
            {
                dialogueLabel.text += letters[index];
                index++;
                yield return new WaitForSeconds(durationBetweenLetters);
            }

            textFinishedGenerating = true;
        }
        public void StartTutorial()
        {
            uiController.ShowTutorial();
            ShowDialogue(LanguageManager.GetTranslation("tutorial_1"));
            AssignNextFuncToBtn(Introduction);
        }
        public void Introduction()
        {
            MoveSpotLight(Vector2.one, new Vector2(694, 546));
            ShowDialogue(LanguageManager.GetTranslation("tutorial_2"));
            AssignNextFuncToBtn(ShowAlbums);
        }
        public void ShowAlbums()
        {
            MoveSpotLight(Vector2.one * 4, new Vector2(952, 515));
            AlbumData[] albums = JsonAlbumAndLevelsLoader.LoadAllAlbums();
            uiController.HideMainMenu();
            albumSelectUI.Initialize(albums);
            uiController.ShowAlbumSelectionMenu();
            ShowDialogue(LanguageManager.GetTranslation("tutorial_3"));
            AssignNextFuncToBtn(() => ShowLevels(albums[0]));
        }
        public void ShowLevels(AlbumData album)
        {
            MoveSpotLight(new Vector2(3, 4), new Vector2(1630, 613));
            uiController.HideAlbumSelectionMenu();
            levelSelectUI.Initialize(album);
            uiController.ShowLevelSelectionMenu();
            ShowDialogue(LanguageManager.GetTranslation("tutorial_4"));
            AssignNextFuncToBtn(PlayLevel);
        }
        public void PlayLevel()
        {
            MoveSpotLight(new Vector2(3, 1), new Vector2(821, 121));
            ShowDialogue(LanguageManager.GetTranslation("tutorial_5"));
            AssignNextFuncToBtn(ShowPlayingArea);
        }    
        public void ShowPlayingArea()
        {
            MoveSpotLight(new Vector2(3, 4), new Vector2(972, 529));
            lineManager.GenerateLines(4);
            uiController.ShowGame();
            uiController.HideLevelSelectionMenu();
            ShowDialogue(LanguageManager.GetTranslation("tutorial_6"));
            AssignNextFuncToBtn(ShowThermometer);
        }
        public void ShowThermometer()
        {
            MoveSpotLight(new Vector2(2, 3), new Vector2(270, 343));
            ShowDialogue(LanguageManager.GetTranslation("tutorial_7"));
            AssignNextFuncToBtn(ShowHealthBar);
        }
        public void ShowHealthBar()
        {
            MoveSpotLight(new Vector2(5, 1), new Vector2(973, 911));
            ShowDialogue(LanguageManager.GetTranslation("tutorial_8"));
            AssignNextFuncToBtn(ShowOffsets);
        }
        public void ShowOffsets()
        {
            MoveSpotLight(new Vector2(2, 1), new Vector2(216, 303));
            uiController.HideGame();
            uiController.ShowLevelSelectionMenu();
            ShowDialogue(LanguageManager.GetTranslation("tutorial_9"));
            AssignNextFuncToBtn(ThankYou);
        }
        public void ThankYou()
        {
            MoveSpotLight(Vector2.one, new Vector2(-1000, -1000));
            uiController.HideLevelSelectionMenu();
            uiController.ShowMainMenu();
            ShowDialogue(LanguageManager.GetTranslation("tutorial_10"));
            AssignNextFuncToBtn(EndTutorial);
        }
        public void EndTutorial()
        {
            uiController.HideTutorial();
        }
    }
}