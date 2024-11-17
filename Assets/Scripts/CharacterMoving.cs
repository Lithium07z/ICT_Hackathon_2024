using UnityEngine;

public class CharacterOscillation : MonoBehaviour
{
    public float oscillationAngle = 30f;
    public float oscillationSpeed = 50f;

    private float currentAngle = 0f;
    private bool isRotatingRight = true;
    private Quaternion initialRotation; // 초기 회전을 저장

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        currentAngle += (isRotatingRight ? oscillationSpeed : -oscillationSpeed) * Time.deltaTime;

        if (currentAngle >= oscillationAngle)
        {
            isRotatingRight = false;
        }
        else if (currentAngle <= -oscillationAngle)
        {
            isRotatingRight = true;
        }

        // 초기 회전값을 기준으로 회전시키기
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, currentAngle);
    }
}
