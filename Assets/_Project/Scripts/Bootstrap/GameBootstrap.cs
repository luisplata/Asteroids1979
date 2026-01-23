using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace _Project.Scripts.Bootstrap
{
    public class GameBootstrap : MonoBehaviour
    {
        static GameBootstrap _instance;

        /// <summary>
        /// Devuelve siempre una instancia válida de GameBootstrap.
        /// Si no existe en la escena, se crea un GameObject con este componente.
        /// </summary>
        public static GameBootstrap Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEngine.Object.FindFirstObjectByType<GameBootstrap>();
                    if (_instance == null)
                    {
                        var go = new GameObject("GameBootstrap");
                        _instance = go.AddComponent<GameBootstrap>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        bool _initialFixDone;
        bool _isIntentionalPause;
        float _lastObservedTimeScale = 1f;
        float _lastLogTime = -10f;
        const float LogCooldown = 5f;

        // Ejecutar lo más pronto posible: durante la subsistema registration
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeEarliest()
        {
            if (!Application.isPlaying) return;
            if (Mathf.Approximately(Time.timeScale, 0f))
            {
                Debug.LogWarning("GameBootstrap: Time.timeScale was 0 at SubsystemRegistration, forcing to 1.");
                Debug.Log(System.Environment.StackTrace);
                Time.timeScale = 1f;
            }
        }

        // Ejecutar lo más pronto posible: después de cargar assemblies
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void InitializeEvenEarlier()
        {
            if (!Application.isPlaying) return;
            if (Mathf.Approximately(Time.timeScale, 0f))
            {
                Debug.LogWarning("GameBootstrap: Time.timeScale was 0 at AfterAssembliesLoaded, forcing to 1.");
                Time.timeScale = 1f;
            }
        }

        // Se ejecuta antes de cargar la primera escena para asegurar que exista la instancia
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeEarly()
        {
            if (!Application.isPlaying) return;
            // Forzamos la propiedad Instance y corregimos timeScale si es necesario
            var bootstrap = Instance;
            bootstrap.EnsureTimeScale();
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Awake()
        {
            // Asegura el singleton y evita duplicados cuando hay múltiples GameObjects
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Inicializa observador
            _lastObservedTimeScale = Time.timeScale;

            // Primera corrección: intenta arreglar timeScale ahora
            EnsureTimeScale();

            // Si sigue en 0, registrar traza para depurar
            if (Mathf.Approximately(Time.timeScale, 0f))
            {
                Debug.LogWarning("GameBootstrap Awake: Time.timeScale STILL 0 after EnsureTimeScale. Stacktrace:");
                Debug.Log(System.Environment.StackTrace);
            }

            // Además corrección en los siguientes frames por si otro componente lo pisa en Awake/Start
            StartCoroutine(EnsureTimeScaleNextFrame());
        }

        void Update()
        {
            // Vigila cambios en timeScale; si alguien lo pone a 0 antes de que consideremos la inicialización completa,
            // lo corregimos y registramos traza de pila para depuración.
            if (!_initialFixDone)
            {
                float current = Time.timeScale;
                if (!Mathf.Approximately(_lastObservedTimeScale, current))
                {
                    // cambio detectado
                    if (Mathf.Approximately(current, 0f) && !_isIntentionalPause)
                    {
                        if (Time.realtimeSinceStartup - _lastLogTime > LogCooldown)
                        {
                            Debug.LogWarning("GameBootstrap: detected timeScale==0 during startup. Forcing to 1 and recording stacktrace.");
                            Debug.Log(System.Environment.StackTrace);
                            _lastLogTime = Time.realtimeSinceStartup;
                        }

                        Time.timeScale = 1f;
                        _initialFixDone = true;
                    }
                    _lastObservedTimeScale = current;
                }
            }
            else
            {
                // Si ya se completó la fase de inicialización, aún queremos proteger contra resets accidentales en runtime
                float current = Time.timeScale;
                if (Mathf.Approximately(current, 0f) && !_isIntentionalPause)
                {
                    if (Time.realtimeSinceStartup - _lastLogTime > LogCooldown)
                    {
                        Debug.LogWarning("GameBootstrap: detected Time.timeScale==0 at runtime post-init. Forcing to 1 and logging stacktrace.");
                        Debug.Log(System.Environment.StackTrace);
                        _lastLogTime = Time.realtimeSinceStartup;
                    }

                    Time.timeScale = 1f;
                }
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Si al cargar una escena el timeScale quedó 0, arreglarlo
            EnsureTimeScale();

            // y también en los siguientes frames (defensa adicional frente a scripts en Start)
            StartCoroutine(EnsureTimeScaleNextFrame());
        }

        IEnumerator EnsureTimeScaleNextFrame(int maxFrames = 5)
        {
            int attempts = 0;
            while (attempts < maxFrames)
            {
                // esperar un frame para permitir que otros Awake/Start se ejecuten
                yield return null;

                if (Mathf.Approximately(Time.timeScale, 0f) && !_isIntentionalPause)
                {
                    Time.timeScale = 1f;
                    yield break;
                }

                attempts++;
            }

            // Fallback: intenta al EndOfFrame una vez
            yield return new WaitForEndOfFrame();

            if (Mathf.Approximately(Time.timeScale, 0f) && !_isIntentionalPause)
            {
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Asegura que Time.timeScale no esté en 0 al iniciar.
        /// No sobreescribe valores positivos intencionados (solo corrige 0).
        /// </summary>
        public void EnsureTimeScale()
        {
            if (Mathf.Approximately(Time.timeScale, 0f) && !_isIntentionalPause)
            {
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Método auxiliar para cambiar el timeScale de forma explícita desde otros sistemas.
        /// También marca si la pausa es intencional.
        /// </summary>
        public void SetTimeScale(float scale)
        {
            _isIntentionalPause = Mathf.Approximately(scale, 0f);
            Time.timeScale = scale;
            _lastObservedTimeScale = scale;
        }

        /// <summary>
        /// Pausa de forma explícita y marca la pausa como intencional.
        /// </summary>
        public void Pause()
        {
            _isIntentionalPause = true;
            Time.timeScale = 0f;
            _lastObservedTimeScale = 0f;
        }

        /// <summary>
        /// Reanuda el juego y marca la pausa como no intencional.
        /// </summary>
        public void Resume()
        {
            _isIntentionalPause = false;
            Time.timeScale = 1f;
            _lastObservedTimeScale = 1f;
        }
    }
}