using UnityEngine;

public class PlayerHeadBob : MonoBehaviour
{

    [Header("Bob Settings")]
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.1f;
    public float runBobSpeed = 18f;
    public float runBobAmount = 0.2f;
    public float bobSmoothing = 4f;

    [Header("References")]
    public Transform player;
    public PlayerMovement playerMovement;

    private float defaultYPos;
    private float timer;

    void Start()
    {
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        if (playerMovement == null || player == null)
            return;

        Vector3 playerVelocity = player.GetComponent<Rigidbody>().linearVelocity;
        Vector2 horizontalVelocity = new Vector2(playerVelocity.x, playerVelocity.z);

        if (horizontalVelocity.magnitude > 0.1f && playerMovement.State != PlayerMovement.MovementState.Air)
        {
            timer += Time.deltaTime * (playerMovement.State == PlayerMovement.MovementState.Running ? runBobSpeed : walkBobSpeed);
            float bobAmount = playerMovement.State == PlayerMovement.MovementState.Running ? runBobAmount : walkBobAmount;

            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            Vector3 newPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * bobSmoothing);
        }
        else
        {
            // Return to default position
            Vector3 newPosition = new Vector3(transform.localPosition.x, defaultYPos, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * bobSmoothing);
        }
    }

}
