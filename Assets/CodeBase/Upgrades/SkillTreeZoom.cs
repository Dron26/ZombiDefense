using UnityEngine;
using UnityEngine.EventSystems;

namespace Upgrades
{
    public class SkillTreeZoom : MonoBehaviour
    {
        [Header("Camera Movement Settings")]
        [SerializeField] private float moveSpeed = 500f; // Скорость перемещения камеры
        [SerializeField] private float zoomSpeed = 250f; // Скорость зума камеры
        [SerializeField] private float zoomSmoothTime = 0.2f; // Плавность изменения зума

        [Header("Camera Boundaries")]
        [SerializeField] private float minZoom = 250f;
        [SerializeField] private float maxZoom = 400f;

        [Header("Camera Movement Limits")]
        [SerializeField] private float minX = -450;
        [SerializeField] private float maxX = 800f;
        [SerializeField] private float minY = -500f;
        [SerializeField] private float maxY = 400f;

        private float currentZoom = 10f;
        private Vector3 lastMousePosition;
        private Vector3 currentVelocity;

        void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        // Обрабатываем движение камеры по оси X и Y
        private void HandleMovement()
        {
            if (Input.GetMouseButton(0)) // Для мыши: если левая кнопка зажата
            {
                Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(lastMousePosition);
                transform.position -= delta; // Двигаем камеру

                // Ограничиваем движение камеры в пределах заданных границ
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX),
                    Mathf.Clamp(transform.position.y, minY, maxY),
                    transform.position.z
                );
            }

            lastMousePosition = Input.mousePosition;

            // Управление через тач
            if (Input.touchCount == 1) // Один палец
            {
                Touch touch = Input.GetTouch(0);
                Vector3 delta = Camera.main.ScreenToWorldPoint(touch.position) - Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                transform.position -= delta;

                // Ограничиваем движение камеры в пределах заданных границ
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX),
                    Mathf.Clamp(transform.position.y, minY, maxY),
                    transform.position.z
                );
            }
        }

        // Обрабатываем увеличение и уменьшение камеры с помощью колесика мыши и тача
        private void HandleZoom()
        {
            // Проверка на колесико мыши
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            // if (scroll != 0)
            // {
            //     Debug.Log("Колесико мыши прокручено. Сдвиг зума: " + scroll);
            // }

            float zoomDelta = scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom - zoomDelta, minZoom, maxZoom);

            // Для тача
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float zoomDeltaTouch = (currentDistance - prevDistance) * 0.1f;

                // if (Mathf.Abs(zoomDeltaTouch) > 0.01f) // Для более точного отслеживания зума
                // {
                //     Debug.Log("Зум на таче: " + zoomDeltaTouch);
                // }

                currentZoom = Mathf.Clamp(currentZoom - zoomDeltaTouch, minZoom, maxZoom);
            }

            // Плавно изменяем orthographicSize
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, currentZoom, ref currentVelocity.z, zoomSmoothTime);

            // Выводим текущий zoom и позицию камеры для проверки
           // Debug.Log("Текущий z-zoom: " + currentZoom);
                //            Debug.Log("Текущая позиция камеры: " + Camera.main.transform.position);
        }

        // Выводим ограничения камеры
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0)); // Левая граница
            Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0)); // Правая граница
            Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0)); // Нижняя граница
            Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(maxX, maxY, 0)); // Верхняя граница
        }
    }
}
