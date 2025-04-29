using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Kecepatan gerak horizontal
    [SerializeField] private float jumpForce = 10f; // Kekuatan lompat
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private int jumpCount; // Variabel untuk melacak jumlah lompatan
    private Vector3 originalScale; // Variabel untuk menyimpan skala asli
    private float horizontalInput; // Tambahkan variabel untuk menyimpan input horizontal

    private void Awake()
    {
        // Ambil referensi untuk Rigidbody2D dan Animator dari objek
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale; // Simpan skala asli di awal
    }

    private void Update()
    {
        // Input gerakan horizontal
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Balik karakter saat bergerak ke kiri atau ke kanan
        if (horizontalInput > 0.01f)
            transform.localScale = originalScale; // Gunakan skala asli saat bergerak ke kanan
        if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z); // Balik skala hanya di sumbu x

        // Lompatan
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || jumpCount < 2))
        {
            Jump();
        }

        // Set parameter Animator
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce); // Gunakan jumpForce untuk lompat
        anim.SetTrigger("jump");
        grounded = false; // Karakter dianggap tidak menyentuh tanah setelah lompat
        jumpCount++; // Tambah jumlah lompatan
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek jika karakter menyentuh objek dengan tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true; // Set grounded menjadi true
            jumpCount = 0; // Reset jumlah lompatan saat menyentuh tanah
        }
    }

    // Memeriksa apakah karakter dapat menyerang
    public bool canAttack()
    {
        // Karakter hanya dapat menyerang jika tidak bergerak, berada di tanah, dan tidak menempel di dinding
        return horizontalInput == 0 && grounded && !onWall();
    }

    // Metode dummy untuk mendeteksi apakah karakter menempel pada dinding
    private bool onWall()
    {
        // Tambahkan logika mendeteksi dinding jika diperlukan
        return false;
    }
}
