using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f; // Waktu antara serangan
    [SerializeField] private string attackTriggerName = "attack"; // Nama trigger animasi serangan
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0; // Tombol untuk menyerang (default: tombol mouse kiri)

    [Header("Attack Hitbox Settings")]
    [SerializeField] private float attackRadius = 1f; // Radius deteksi serangan
    [SerializeField] private LayerMask enemyLayer; // Layer untuk mendeteksi musuh
    [SerializeField] private float attackDamage = 20f; // Jumlah damage yang diberikan pada musuh

    [Header("Attack Visuals and Audio")]
    [SerializeField] private AudioClip attackSound; // Suara saat menyerang
    [SerializeField] private GameObject attackEffectPrefab; // Efek partikel saat menyerang

    private Animator anim; // Referensi komponen Animator
    private PlayerMovement playerMovement; // Referensi komponen PlayerMovement
    private float cooldownTimer = Mathf.Infinity; // Timer cooldown, dimulai dengan nilai tinggi

    private void Awake()
    {
        // Inisialisasi referensi komponen
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        HandleAttackInput();
        UpdateCooldownTimer();
    }

    // Menangani input serangan dari pemain
    private void HandleAttackInput()
    {
        if (CanAttack())
        {
            Attack();
        }
    }

    // Mengecek apakah pemain bisa menyerang (tombol serang ditekan dan cooldown selesai)
    private bool CanAttack()
    {
        return Input.GetKeyDown(attackKey) && cooldownTimer >= attackCooldown && playerMovement != null && playerMovement.canAttack();
    }

    // Melakukan serangan
    // Deteksi musuh dengan Raycast
    private void Attack()
    {
        // Memicu animasi serangan tanpa menghentikan pergerakan
        if (anim != null)
        {
            anim.SetTrigger(attackTriggerName);
        }

        // Memainkan suara serangan (jika ada)
        if (attackSound != null)
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }

        // Menginstansiasi efek visual serangan (jika ada)
        if (attackEffectPrefab != null)
        {
            Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        }

        // Menyusun raycast dari posisi pemain
        RaycastHit2D[] hitEnemies = Physics2D.RaycastAll(transform.position, transform.right, attackRadius, enemyLayer);
        foreach (var hit in hitEnemies)
        {
            // Menangani damage pada musuh
            var enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }

            // Jika musuh mengenai pemain, berikan damage pada pemain
            var playerHealth = hit.collider.GetComponent<Health>(); // Mengasumsikan enemy bisa merusak player
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // Memberikan damage pada player
            }
        }

        // Reset cooldown
        cooldownTimer = 0f;
    }


    // Mengupdate timer cooldown
    private void UpdateCooldownTimer()
    {
        cooldownTimer += Time.deltaTime;
    }

    // Opsional: Menggambar lingkaran debug di scene untuk deteksi serangan
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
