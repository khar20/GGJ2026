using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class SimpleHUD : MonoBehaviour
    {
        [Header("Configuración Visual")]
        [Tooltip("Color principal del tema (fondo oscuro)")]
        public Color primaryColor = new Color(0.05f, 0.05f, 0.08f, 0.9f);
        [Tooltip("Color de la barra de vida")]
        public Color healthColor = new Color(1.0f, 0.3f, 0.3f, 1.0f);
        [Tooltip("Color de la barra de energía")]
        public Color staminaColor = new Color(1.0f, 0.8f, 0.0f, 1.0f);
        [Tooltip("Color de acento para botones y destacados")]
        public Color accentColor = new Color(0.2f, 0.8f, 1.0f, 1.0f);

        private Font _defaultFont;
        private Sprite _roundedRectSprite;
        
        // Referencias para animación
        private Image _healthFill;
        private Image _staminaFill;
        private GameOverHUD _gameOverHUD;

        // Variables de juego
        private float _currentStamina = 0f;
        private float _maxStamina = 100f;
        private bool _isGameOver = false;

        private void Start()
        {
            // Limpiar HUD previo
            GameObject oldCanvas = GameObject.Find("HUD_Canvas");
            if (oldCanvas != null) DestroyImmediate(oldCanvas);

            GenerateResources();
            _gameOverHUD = gameObject.AddComponent<GameOverHUD>();
            CreateModernHUD();
            
            // Inicializar estamina en 0
            if (_staminaFill != null)
            {
                _staminaFill.rectTransform.anchorMax = new Vector2(0, 1);
            }
        }

        private void Update()
        {
            if (!_isGameOver)
            {
                UpdateStaminaLogic();
            }
            AnimateBars();
        }

        private void UpdateStaminaLogic()
        {
            // Subir estamina lentamente
            _currentStamina += Time.deltaTime * 1.5f; // Ajusta la velocidad aquí (1.5 unidades por segundo)

            // Actualizar barra visualmente
            if (_staminaFill != null)
            {
                float pct = Mathf.Clamp01(_currentStamina / _maxStamina);
                _staminaFill.rectTransform.anchorMax = new Vector2(pct, 1);
            }

            // Chequear Game Over
            if (_currentStamina >= _maxStamina)
            {
                TriggerGameOver();
            }
        }

        private void TriggerGameOver()
        {
            _isGameOver = true;
            if (_gameOverHUD != null) _gameOverHUD.Show();
            
            // Detener el tiempo
            Time.timeScale = 0f;
            
            // Liberar cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Añade una cantidad de estamina instantáneamente
        /// </summary>
        /// <param name="amount">Cantidad a añadir</param>
        public void AddStamina(float amount)
        {
            _currentStamina += amount;
            if (_currentStamina > _maxStamina) _currentStamina = _maxStamina;
        }

        private void GenerateResources()
        {
            _roundedRectSprite = CreateRoundedBoxSprite(128, 32);
            
            // Carga de fuentes robusta
            if (_defaultFont == null) try { _defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); } catch { }
            if (_defaultFont == null) try { _defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf"); } catch { }
            if (_defaultFont == null) _defaultFont = Font.CreateDynamicFontFromOSFont("Segoe UI", 16);
        }

        private void CreateModernHUD()
        {
            // 1. CANVAS
            GameObject canvasObj = new GameObject("HUD_Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            
            canvasObj.AddComponent<GraphicRaycaster>();

            // 2. CONTENEDOR SUPERIOR (Vida)
            GameObject topHud = CreateUIObject("TopHUD", canvas.transform);
            RectTransform topRect = topHud.GetComponent<RectTransform>();
            topRect.anchorMin = Vector2.one; // 1,1 = Arriba Derecha
            topRect.anchorMax = Vector2.one;
            topRect.pivot = Vector2.one;     // Pivot en la esquina superior derecha
            topRect.anchoredPosition = new Vector2(-40, -40); // Margen
            topRect.sizeDelta = new Vector2(300, 40); // Altura reducida solo para vida

            // Barra de Vida (Arriba Derecha)
            CreateBar("HealthBar", topHud.transform, Vector2.zero, Vector2.zero, healthColor, "HP", out _healthFill);
            
            // 3. CONTENEDOR INFERIOR (Estamina - A lo largo de la pantalla)
            GameObject bottomHud = CreateUIObject("BottomHUD", canvas.transform);
            RectTransform bottomRect = bottomHud.GetComponent<RectTransform>();
            bottomRect.anchorMin = new Vector2(0, 0); // Abajo Izquierda
            bottomRect.anchorMax = new Vector2(1, 0); // Abajo Derecha (Stretch X)
            bottomRect.pivot = new Vector2(0.5f, 0);  // Pivot Abajo Centro
            bottomRect.anchoredPosition = new Vector2(0, 20); // Margen desde abajo
            bottomRect.sizeDelta = new Vector2(-100, 20); // Restar 100px al ancho total (50px margen cada lado)

            // Barra de Energía (Abajo, larga)
            CreateBar("StaminaBar", bottomHud.transform, Vector2.zero, Vector2.zero, staminaColor, "", out _staminaFill);

            // 4. Iniciar script de Botones
            if (GetComponent<Botones>() == null)
            {
                gameObject.AddComponent<Botones>();
            }
        }



        private void CreateBar(string name, Transform parent, Vector2 posOffset, Vector2 sizeOffset, Color color, string label, out Image fillImage)
        {
            GameObject container = CreateUIObject(name, parent);
            RectTransform rect = container.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(1, 0.5f);
            rect.anchoredPosition = posOffset;
            rect.sizeDelta = new Vector2(0, 16); // Altura fija
            rect.offsetMin = new Vector2(0, rect.offsetMin.y);
            rect.offsetMax = new Vector2(0, rect.offsetMax.y);

            // Relleno
            GameObject fillObj = CreateImage("Fill", container.transform, _roundedRectSprite, color);
            fillImage = fillObj.GetComponent<Image>();
            fillImage.type = Image.Type.Sliced;
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = Vector2.zero;

            // Etiqueta pequeña
            GameObject textObj = CreateText("Label", container.transform, label, 14, new Color(1,1,1,0.9f), TextAnchor.MiddleRight);
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0.5f);
            textRect.anchorMax = new Vector2(0, 0.5f);
            textRect.anchoredPosition = new Vector2(-20, 0);
            
            // Sombra texto
            textObj.AddComponent<Shadow>().effectColor = new Color(0,0,0,0.8f);
        }

        private void AnimateBars()
        {
            // Simulación de "respiración" o movimiento en las barras para dar vida
            if (_healthFill != null)
            {
                // Ejemplo: Pulso suave en alpha si la vida es baja (simulado aquí con color)
                // En un juego real, conectaríamos esto con el PlayerHealth
                float pulse = Mathf.PingPong(Time.time, 1f) * 0.1f + 0.9f;
                // _healthFill.color = new Color(healthColor.r * pulse, healthColor.g * pulse, healthColor.b * pulse, 1f);
            }
        }

        // --- HELPERS Y GENERACIÓN ---

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
                    // Lógica para esquinas redondeadas
                    float dx = x < r ? r - x : (x > res - r ? x - (res - r) : 0);
                    float dy = y < r ? r - y : (y > res - r ? y - (res - r) : 0);
                    float d = Mathf.Sqrt(dx*dx + dy*dy);
                    
                    float alpha = Mathf.Clamp01(1.0f - (d - r));
                    colors[y * res + x] = new Color(1, 1, 1, alpha);
                }
            }
            tex.SetPixels(colors);
            tex.Apply();
            // Definir bordes 9-slice
            return Sprite.Create(tex, new Rect(0, 0, res, res), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, new Vector4(radiusPx, radiusPx, radiusPx, radiusPx));
        }
    }
}