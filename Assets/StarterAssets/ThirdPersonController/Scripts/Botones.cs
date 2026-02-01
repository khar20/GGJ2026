using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class Botones : MonoBehaviour
    {
        private Font _defaultFont;
        private Sprite _roundedRectSprite;
        private RectTransform _smallZRect;
        private RectTransform _smallXRect;
        private Image _smallZImage;
        private Image _smallXImage;
        private float _zPressAmount;
        private float _xPressAmount;
        private Color _baseButtonColor;
        private Color _pressedButtonColor;

        private void Start()
        {
            _baseButtonColor = new Color(1f, 1f, 1f, 0.2f);
            _pressedButtonColor = new Color(0.7f, 0.7f, 0.7f, 0.9f);
            GenerateResources();
            StartCoroutine(InitializeControls());
        }

        private void Update()
        {
            bool zPressed = false;
            bool xPressed = false;
    #if ENABLE_INPUT_SYSTEM
                Keyboard keyboard = Keyboard.current;
                if (keyboard != null)
                {
                    zPressed = keyboard.zKey.isPressed;
                    xPressed = keyboard.xKey.isPressed;
                }
    #else
                zPressed = Input.GetKey(KeyCode.Z);
                xPressed = Input.GetKey(KeyCode.X);
    #endif
            UpdateButtonVisual(_smallZRect, _smallZImage, zPressed, ref _zPressAmount);
            UpdateButtonVisual(_smallXRect, _smallXImage, xPressed, ref _xPressAmount);
        }

        private IEnumerator InitializeControls()
        {
            // Esperar al final del frame para asegurar que SimpleHUD haya creado el canvas
            yield return new WaitForEndOfFrame();

            GameObject canvasObj = GameObject.Find("HUD_Canvas");
            if (canvasObj == null)
            {
                // Fallback por si no existe el canvas
                canvasObj = new GameObject("HUD_Canvas");
                Canvas canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            CreateControlsHUD(canvasObj.transform);
        }

        private void CreateControlsHUD(Transform parent)
        {
            Transform existing = parent.Find("ControlsHUD");
            if (existing != null) Destroy(existing.gameObject);

            GameObject controlsHud = CreateUIObject("ControlsHUD", parent);
            RectTransform rect = controlsHud.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(40, -40);
            rect.sizeDelta = new Vector2(200, 120);

            CreateKeyDisplay("KeyZ", controlsHud.transform, new Vector2(0, 0), "Z");
            CreateKeyDisplay("KeyX", controlsHud.transform, new Vector2(100, 0), "X");
        }

        private void CreateKeyDisplay(string name, Transform parent, Vector2 pos, string keyText)
        {
            GameObject keyObj = CreateUIObject(name, parent);
            RectTransform keyRect = keyObj.GetComponent<RectTransform>();
            keyRect.anchorMin = new Vector2(0, 1);
            keyRect.anchorMax = new Vector2(0, 1);
            keyRect.pivot = new Vector2(0, 1);
            keyRect.anchoredPosition = pos;
            keyRect.sizeDelta = new Vector2(80, 110);

            GameObject bigCircle = CreateImage("BigCircle", keyObj.transform, _roundedRectSprite, new Color(1f, 1f, 1f, 0.05f));
            RectTransform bigRect = bigCircle.GetComponent<RectTransform>();
            bigRect.anchorMin = new Vector2(0.5f, 1f);
            bigRect.anchorMax = new Vector2(0.5f, 1f);
            bigRect.pivot = new Vector2(0.5f, 1f);
            bigRect.anchoredPosition = new Vector2(0, -25);
            bigRect.sizeDelta = new Vector2(72, 72);
            Outline bigOutline = bigCircle.AddComponent<Outline>();
            bigOutline.effectColor = new Color(0f, 0f, 0f, 0.7f);
            bigOutline.effectDistance = new Vector2(2f, -2f);

            GameObject smallCircle = CreateImage("SmallCircle", keyObj.transform, _roundedRectSprite, _baseButtonColor);
            RectTransform smallRect = smallCircle.GetComponent<RectTransform>();
            smallRect.anchorMin = new Vector2(0.5f, 0f);
            smallRect.anchorMax = new Vector2(0.5f, 0f);
            smallRect.pivot = new Vector2(0.5f, 0.5f);
            smallRect.anchoredPosition = new Vector2(0, 8);
            smallRect.sizeDelta = new Vector2(32, 32);
            Outline smallOutline = smallCircle.AddComponent<Outline>();
            smallOutline.effectColor = new Color(0f, 0f, 0f, 0.7f);
            smallOutline.effectDistance = new Vector2(2f, -2f);

            Image smallImage = smallCircle.GetComponent<Image>();
            if (keyText == "Z")
            {
                _smallZRect = smallRect;
                _smallZImage = smallImage;
            }
            else if (keyText == "X")
            {
                _smallXRect = smallRect;
                _smallXImage = smallImage;
            }

            GameObject txt = CreateText("Text", smallCircle.transform, keyText, 18, Color.white, TextAnchor.MiddleCenter);
            RectTransform txtRect = txt.GetComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;
            txtRect.sizeDelta = Vector2.zero;
        }

        private void GenerateResources()
        {
            _roundedRectSprite = CreateRoundedBoxSprite(128, 32);
            
            if (_defaultFont == null) try { _defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); } catch { }
            if (_defaultFont == null) try { _defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf"); } catch { }
            if (_defaultFont == null) _defaultFont = Font.CreateDynamicFontFromOSFont("Segoe UI", 16);
        }

        private void UpdateButtonVisual(RectTransform rect, Image img, bool pressed, ref float pressAmount)
        {
            if (rect == null || img == null) return;
            float target = pressed ? 1f : 0f;
            pressAmount = Mathf.MoveTowards(pressAmount, target, Time.unscaledDeltaTime * 10f);
            float scale = Mathf.Lerp(1f, 0.9f, pressAmount);
            rect.localScale = Vector3.one * scale;
            img.color = Color.Lerp(_baseButtonColor, _pressedButtonColor, pressAmount);
        }

        // --- Helpers ---

        private GameObject CreateUIObject(string name, Transform parent)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            obj.AddComponent<RectTransform>();
            return obj;
        }

        private GameObject CreateImage(string name, Transform parent, Sprite sprite, Color color)
        {
            GameObject obj = CreateUIObject(name, parent);
            Image img = obj.AddComponent<Image>();
            img.sprite = sprite;
            img.color = color;
            if (sprite == _roundedRectSprite) img.type = Image.Type.Sliced;
            return obj;
        }

        private GameObject CreateText(string name, Transform parent, string content, int fontSize, Color color, TextAnchor align)
        {
            GameObject obj = CreateUIObject(name, parent);
            Text txt = obj.AddComponent<Text>();
            txt.text = content;
            txt.font = _defaultFont;
            txt.fontSize = fontSize;
            txt.color = color;
            txt.alignment = align;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
            return obj;
        }

        private Sprite CreateRoundedBoxSprite(int res, int radiusPx)
        {
            Texture2D tex = new Texture2D(res, res);
            tex.filterMode = FilterMode.Bilinear;
            Color[] colors = new Color[res * res];
            float r = radiusPx;
            
            for (int y = 0; y < res; y++)
            {
                for (int x = 0; x < res; x++)
                {
                    float dx = x < r ? r - x : (x > res - r ? x - (res - r) : 0);
                    float dy = y < r ? r - y : (y > res - r ? y - (res - r) : 0);
                    float d = Mathf.Sqrt(dx*dx + dy*dy);
                    
                    float alpha = Mathf.Clamp01(1.0f - (d - r));
                    colors[y * res + x] = new Color(1, 1, 1, alpha);
                }
            }
            tex.SetPixels(colors);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, res, res), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, new Vector4(radiusPx, radiusPx, radiusPx, radiusPx));
        }
    }
}
