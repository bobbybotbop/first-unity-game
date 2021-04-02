using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform player;

    public Vector3 VsmoothSpeed;
    public float smoothSpeed = .125f;
    public Vector3 offset;
    private Vector3 playerX;
    bool isGood;
    float leftCap = -17.88f;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        IsGoodMethod();


            Vector3 disiredPosition = new Vector3(0f, player.position.y , 0f) + offset + playerX;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, disiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothPosition;


        /*if (isGood)
        {
            Vector3 disiredPosition = player.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, disiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothPosition;
        }*/
    }

    private void IsGoodMethod()
    {
        if (player.transform.position.x < leftCap)
        {
            isGood = false;
        }

        else
        {
            isGood = true;
            
        }

        if (isGood)
        {
            playerX = new Vector3(player.position.x, 0f, 0f);
        }
        else
        {
            playerX = new Vector3(leftCap, 0f, 0f); // locks the camera to the left
        }
    }

}
