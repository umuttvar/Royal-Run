using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float xClamp = 5f;
    [SerializeField] float zClamp = 5f;

    Vector2 movement;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody> ();
    }
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

    }

    void FixedUpdate()
    {

        HandleMovement();

    }
    
    void HandleMovement()
    {
        //oyuncu hareketi rigidbody üzerinden olacağı için önce rigidbodynin dünyadaki konumunu baz aldık
        // (rigidbodynin konumu bağlı olduğu gameobjecte göre şekillenir.)
        Vector3 currentPosition = rb.position;

        //hareket yönünü hesapladık. Input üzerinde Vector2 kullandık yani sadece x ve y düzlemi var. ama bizim oyuncumuz zıplamayacağı ve x ve z ekseninde hareket edeceği için Vector3 kullanmamız gerekiyor. bu yüzden x  ekseninde olduğu gibi kalır, y ekseninde oyuncumuz zıplamayacağı için 0 verilir, z ekseninde de Input değerinden gelen y ekseni gelir. bu sayede klavyeden w ve s tuşuna basıldığında y ekseni olarak algılanır ama yazdığımız kod sayesinde y ekseni z ekseni olarak davranıp ileri geri hareket eder.
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);

        //oyuncumuzun yeni konumunu hesapladık. 
        //currentPosition şu anki konumu,
        //moveDirection hareket edeceği yön,
        //movementSpeed hareket hızı,
        //Time.fixedDeltaTime oyuncunun framedeki yeri.
        //bu sayede oyuncumuzun bulunduğu konumdan ileri gitmesi için işlemler yaptık.
        Vector3 newPosition = currentPosition + moveDirection * (movementSpeed * Time.fixedDeltaTime);

        //Oluşturlan yeni pozisyon değerinin max ve min alabileceği değerleri sınırlandırmak için Clamp fonksiyonunu kullanıyoruz.
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

        //MovePosition metodu rigidBody sınıfına ait bir metoddur.
        //Bu metot sayesinde Unity'e "beni bu pozisyona taşı" denir.
        //tranform.position kullanılsaydı direkt olarak oraya ışınlanırdık.
        rb.MovePosition(newPosition);

        
    }

}
