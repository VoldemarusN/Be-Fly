using UnityEngine;

namespace Services
{
    public class CameraPositionService : IService
    {
        public Camera Camera => Camera.main;

        public Vector3 GetRandomRightBorderPosition(Vector2 offset)
        {
            float xCoord = GetCameraHalfWidth() + offset.x;
            float yCoord = Random.Range(0 + offset.y, Screen.height - offset.y);
            return Camera.ScreenToWorldPoint(new Vector2(xCoord, yCoord));
        }

        public float GetCameraHalfWidth() => Camera.orthographicSize * Camera.aspect;


        public Vector3 GetNormalizedRightBorderPosition(float normalizedValue)
        {
            float xCoord = Screen.width;
            float yCoord = Screen.height * normalizedValue;
            return Camera.ScreenToWorldPoint(new Vector2(xCoord, yCoord));
        }


        public Vector3 GetNormalizedLeftBorderPosition(float normalizedValue)
        {
            float xCoord = 0;
            float yCoord = Screen.height * normalizedValue;
            return Camera.ScreenToWorldPoint(new Vector2(xCoord, yCoord));
        }


        public Vector3 GetRandomLeftBorderPosition(Vector2 offset)
        {
            float xCoord = 0 + offset.x;
            float yCoord = Random.Range(0 + offset.y, Screen.height - offset.y);
            return Camera.ScreenToWorldPoint(new Vector2(xCoord, yCoord));
        }
    }
}