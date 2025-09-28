using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float minFOV = 20f; //kameranın min görüş  değeri
    [SerializeField] float maxFOV = 120f; //kameranın max görüş değeri
    [SerializeField] float zoomDuration = 1f; //zoomuun kaç saniye süreceği
    [SerializeField] float zoomSpeedModifer = 5f; //kameranın zoom hızını ayarlamak için
    [SerializeField] ParticleSystem speedUpParticleSystem;

    CinemachineCamera cinemachineCamera;

    void Awake() //Awake() oyun başlar başlamaz çalışır
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void ChangeCameraFOV(float speedAmount)
    {
        StopAllCoroutines(); //Diğer Corotuineleri durdurur, eğer ChangeFOVRoutine çalışıyorsa tekrar çalışmasını engeller.
        StartCoroutine(ChangeFOVRoutine(speedAmount));

        if (speedAmount > 0)
        {
            speedUpParticleSystem.Play();
        }
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView ; //cinemachine kameranın mevcut görüş açısı
        float targetFOV = Mathf.Clamp(startFOV + speedAmount * zoomSpeedModifer , minFOV, maxFOV); //hedef görüş açısı
        //startFOV + speedAmount * zoomSpeedModifer 
        // 60 + 5 * 1 = 65, kamera açısı 65 olacak

        float elapsedTime = 0f; // zoom animasyonunda geçen süre

        while (elapsedTime < zoomDuration) //eğer animasyon süresi belirlediğimiz zoomDuration değerini geçerse döngüden çıkacak
        {
            float t = elapsedTime / zoomDuration; //t değeri 0 ile 1 arasında bir değer olacağı için gelen elapsedtime değerini mevcut zoomDuration değerine bölüyoruz. 
            //elapsedTime değeri zaten hep 0 o yüzden zoomDuration değeri de 1 bu yüzden döngüye giriyor.

            elapsedTime += Time.deltaTime; //burada elapsedTime Time.deltaTime ile oyunda geçen zamanda frame olarak arttığı için sonsuz döngü olmamış oluyor.

            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t); //eğer t değeri 0 ise startFOV döner, eğer 1 ise targetFOV döner. t değeri 1 veya 1'den büyük olursa döngüden çıkar.
            yield return null;
        }

        cinemachineCamera.Lens.FieldOfView = targetFOV; //max değere ulaşması için eğer t değeri 1 veya 1'den büyük olursa kamera açısı max değere göre ayarlanıyor.
    }
}
