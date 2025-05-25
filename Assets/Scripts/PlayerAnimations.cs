using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    private PlayerMovement PlayerMovement;
    private Animator Bearnimator;

    private void Start()
    {
        Bearnimator = GetComponent<Animator>();
    }


    void Update()
    {

        //Walking Animation
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        Bearnimator.SetBool("IsWalking", isMoving);
        //Walking Animation
        bool isJumping = Input.GetKey(KeyCode.Space);
        Bearnimator.SetBool("IsJumping", isJumping);

    }
   
}