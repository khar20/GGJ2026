using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace StarterAssets
{
    public class GameOverHUD : MonoBehaviour
    {
        [Header("Colores")]
        public Color overlayColor = new Color(0f, 0f, 0f, 0.85f);
        public Color panelColor = new Color(0.08f, 0.09f, 0.12f, 1f);
        public Color accentColor = new Color(0.25f, 0.75f, 1f, 1f);
        public Color textPrimary = Color.white;
        public Color textSecondary = new Color(0.7f, 0.72f, 0.78f);

        private GameObject canvasObj;
        private CanvasGroup canvasGroup;
        private RectTransform panelRect;
        private Font font;
        private Sprite roundedSprite;

        private void Awake()
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            roundedSprite = CreateRoundedSprite(256, 32);

            BuildUI();
            Hide();
        }

        public void Show()
        {
            canvasObj.SetActive(true);
            
            // Desbloquear cursor y asegurar que no se vuelva a bloquear por el input system
            var inputs = FindFirstObjectByType<StarterAssetsInputs>();
            if (inputs != null)
            {
                inputs.cursorLocked = false;
                inputs.cursorInputForLook = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(AnimateIn());
        }

        public void Hide()
        {
            if (canvasObj != null)
                canvasObj.SetActive(false);
        }

        private void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // ================= UI =================

        private void BuildUI()
        {
            // Canvas
            canvasObj = new GameObject("GameOverCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasObj.AddComponent<GraphicRaycaster>();
            canvasGroup = canvasObj.AddComponent<CanvasGroup>();

            // Overlay
            GameObject overlay = CreateUIObject("Overlay", canvasObj.transform);
            Image overlayImg = overlay.AddComponent<Image>();
            overlayImg.color = overlayColor;
            Stretch(overlay.GetComponent<RectTransform>());

            // Panel
            GameObject panel = CreateUIObject("Panel", overlay.transform);
            panelRect = panel.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(520, 300);

            Image panelImg = panel.AddComponent<Image>();
            panelImg.sprite = roundedSprite;
            panelImg.type = Image.Type.Sliced;
            panelImg.color = panelColor;

            Shadow panelShadow = panel.AddComponent<Shadow>();
            panelShadow.effectColor = new Color(0, 0, 0, 0.6f);
            panelShadow.effectDistance = new Vector2(0, -6);

            // Title
            GameObject title = CreateText(
                "GAME OVER",
                54,
                textPrimary,
                FontStyle.Bold,
                panel.transform
            );
            SetAnchor(title, 0.65f, 0.9f);

            // Accent Line
            GameObject line = CreateUIObject("Accent", panel.transform);
            RectTransform lineRect = line.GetComponent<RectTransform>();
            lineRect.anchorMin = new Vector2(0.5f, 0.6f);
            lineRect.anchorMax = new Vector2(0.5f, 0.6f);
            lineRect.sizeDelta = new Vector2(80, 3);

            Image lineImg = line.AddComponent<Image>();
            lineImg.color = accentColor;

            // Subtitle
            GameObject subtitle = CreateText(
                "YA NO PUEDES CORRER\nERES UN FRACASADO",
                16,
                textSecondary,
                FontStyle.Normal,
                panel.transform
            );
            SetAnchor(subtitle, 0.38f, 0.55f);

            // Button
            GameObject buttonObj = CreateUIObject("RestartButton", panel.transform);
            RectTransform btnRect = buttonObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.18f);
            btnRect.anchorMax = new Vector2(0.5f, 0.18f);
            btnRect.sizeDelta = new Vector2(220, 50);

            Image btnImg = buttonObj.AddComponent<Image>();
            btnImg.sprite = roundedSprite;
            btnImg.type = Image.Type.Sliced;
            btnImg.color = accentColor;

            Button btn = buttonObj.AddComponent<Button>();
            btn.onClick.AddListener(Restart);

            ColorBlock cb = btn.colors;
            cb.normalColor = accentColor;
            cb.highlightedColor = accentColor * 1.1f;
            cb.pressedColor = accentColor * 0.9f;
            cb.fadeDuration = 0.08f;
            btn.colors = cb;

            GameObject btnText = CreateText(
                "REINTENTAR",
                18,
                Color.black,
                FontStyle.Bold,
                buttonObj.transform
            );
            Stretch(btnText.GetComponent<RectTransform>());
        }

        // ================= Anim =================

        private IEnumerator AnimateIn()
        {
            canvasGroup.alpha = 0;
            panelRect.localScale = Vector3.one * 0.9f;
            panelRect.anchoredPosition = new Vector2(0, -40);

            float t = 0f;
            float d = 0.45f;

            while (t < d)
            {
                t += Time.unscaledDeltaTime;
                float k = t / d;
                k = 1f - Mathf.Pow(1f - k, 3f);

                canvasGroup.alpha = k;
                panelRect.localScale = Vector3.Lerp(Vector3.one * 0.9f, Vector3.one, k);
                panelRect.anchoredPosition = Vector2.Lerp(
                    new Vector2(0, -40),
                    Vector2.zero,
                    k
                );

                yield return null;
            }

            canvasGroup.alpha = 1;
        }

        // ================= Helpers =================

        private GameObject CreateUIObject(string name, Transform parent)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            obj.AddComponent<RectTransform>();
            return obj;
        }

        private GameObject CreateText(string text, int size, Color color, FontStyle style, Transform parent)
        {
            GameObject obj = CreateUIObject("Text", parent);
            Text t = obj.AddComponent<Text>();
            t.text = text;
            t.font = font;
            t.fontSize = size;
            t.color = color;
            t.fontStyle = style;
            t.alignment = TextAnchor.MiddleCenter;
            return obj;
        }

        private void Stretch(RectTransform r)
        {
            r.anchorMin = Vector2.zero;
            r.anchorMax = Vector2.one;
            r.offsetMin = Vector2.zero;
            r.offsetMax = Vector2.zero;
        }

        private void SetAnchor(GameObject obj, float minY, float maxY)
        {
            RectTransform r = obj.GetComponent<RectTransform>();
            r.anchorMin = new Vector2(0, minY);
            r.anchorMax = new Vector2(1, maxY);
            r.offsetMin = new Vector2(30, 0);
            r.offsetMax = new Vector2(-30, 0);
        }

        private Sprite CreateRoundedSprite(int size, int radius)
        {
            Texture2D tex = new Texture2D(size, size);
            Color[] c = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = Mathf.Max(radius - x, 0, x - (size - radius));
                    float dy = Mathf.Max(radius - y, 0, y - (size - radius));
                    float a = dx * dx + dy * dy <= radius * radius ? 1 : 0;
                    c[y * size + x] = new Color(1, 1, 1, a);
                }
            }

            tex.SetPixels(c);
            tex.Apply();

            return Sprite.Create(
                tex,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f),
                100,
                0,
                SpriteMeshType.FullRect,
                new Vector4(radius, radius, radius, radius)
            );
        }
    }
}
