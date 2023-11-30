using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("PlayerStats")]
    public float speed;
    [SerializeField] private Rigidbody rb;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMovement();
    }
    
    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        rb.MovePosition(transform.position + move * speed * Time.deltaTime);
    }
}

