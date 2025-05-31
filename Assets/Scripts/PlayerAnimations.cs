using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    private PlayerMovement PlayerMovement;
    private Animator PlayerAnimator;

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }


    void Update()
    {

        //Walking Animation
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        PlayerAnimator.SetBool("IsWalking", isMoving);
        //Walking Animation
        bool isJumping = Input.GetKey(KeyCode.Space);
        PlayerAnimator.SetBool("IsJumping", isJumping);

    }
   
}