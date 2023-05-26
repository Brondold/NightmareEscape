using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float moveSpeedManiv = 1f;
    public float circleRadius = 1f;

    private bool isMovingManiv = false;
    private float angle = 0f;
    private Vector3 startPosition;

    public bool readyManiv = false;

    public porte plafondF;

    private void Start()
    {
        startPosition = transform.position + new Vector3(0f, -0.2f, 0f);
    }

    private void Update()
    {
        if (readyManiv)
        {

            
            if (Input.GetKeyDown(KeyCode.F))
            {
                isMovingManiv = true;
            }


            
            else if (Input.GetKeyUp(KeyCode.F))
            {
                isMovingManiv = false;
            }

        if (isMovingManiv)
        {
            if (plafondF.plafond)
                {
                    angle += moveSpeedManiv * Time.deltaTime;
                    float y = Mathf.Cos(angle) * circleRadius;
                    float z = Mathf.Sin(angle) * circleRadius;
                    transform.position = startPosition + new Vector3(0f, y, z);
                }
            else
                {
                    isMovingManiv = false;
                }
        }
        }
        else
        {
            isMovingManiv = false;
        }
    }
}
