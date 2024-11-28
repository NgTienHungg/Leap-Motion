using System.Collections;
using BaseSource;
using UnityEngine;
using UnityEngine.UI;

namespace NinjaFruit
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoSingleton<GameManager>
    {
        public UIMainGamePanel uiMainGamePanel;
        [SerializeField] private Blade blade;
        [SerializeField] private Spawner spawner;
        [SerializeField] private Image fadeImage;

        // public int score { get; private set; } = 0;

        protected override void OnAwake() { }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            Time.timeScale = 1f;

            ClearScene();

            blade.enabled = true;
            spawner.enabled = true;

            ScoreManager.Instance.ResetScore();
            uiMainGamePanel.Setup();
        }

        private void ClearScene()
        {
            Fruit[] fruits = FindObjectsOfType<Fruit>();
            foreach (var fruit in fruits)
            {
                Destroy(fruit.gameObject);
            }

            Bomb[] bombs = FindObjectsOfType<Bomb>();
            foreach (Bomb bomb in bombs)
            {
                Destroy(bomb.gameObject);
            }
        }

        public void Explode()
        {
            blade.enabled = false;
            spawner.enabled = false;

            StartCoroutine(ExplodeSequence());
        }

        private IEnumerator ExplodeSequence()
        {
            float elapsed = 0f;
            float duration = 0.5f;

            // Fade to white
            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

                Time.timeScale = 1f - t;
                elapsed += Time.unscaledDeltaTime;

                yield return null;
            }

            yield return new WaitForSecondsRealtime(1f);

            NewGame();

            elapsed = 0f;

            // Fade back in
            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

                elapsed += Time.unscaledDeltaTime;

                yield return null;
            }
        }
    }
}