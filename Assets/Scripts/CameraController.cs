using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;//kamera hızı
    [SerializeField] private float minXRotation;//x deki en düşük rotasyonu
    [SerializeField] private float maxXRotation;//x teki en yüksek rotasyonu
    [SerializeField] private float minZoom;//en düşük zoom 
    [SerializeField] private float maxZoom;//en yüksek zoom
    [SerializeField] private float zoomSpeed;//zoom hızı
    [SerializeField] private float rotationSpeed;//dönme hızı

    private float _currentZoom;//şuanki zoomu
    private float _currentXRotation;//x deki şaunki rotasyonu
    private Camera _camera;//kameramız


    private void Start()
    {
        _camera = Camera.main;//_kameramız main kamera olsun dedik
        _currentZoom = _camera.transform.localPosition.y; // şuanki zoom kameranın kendi poziyonundaki y değeri dedik
        _currentXRotation = -50;//mevcut rotasyonu başlangıçta - 50 al
    }

    private void Update()
    {
        _currentZoom += Input.GetAxis("Mouse ScrollWheel")*-zoomSpeed;//mausun orta topunu oynattıkça zoom yapsın 
        _currentZoom = Math.Clamp(_currentZoom, minZoom, maxZoom);//zoom değerlerini alırken min ve max değerine göre zoom yapsın
        _camera.transform.localPosition = Vector3.up * _currentZoom;//kameranın pozizyonunu y ekseninde yukarı doğru şuanki zoom kadar çıkart

        if (Input.GetMouseButton(1))//eğer mausun sağ butonuna basılırsa aşşağıdakileri yap
        {
            float x = Input.GetAxis("Mouse X");//mausun x deki hareketlerini aldık
            float y = Input.GetAxis("Mouse Y");//mausun y deki hareketlerini aldık

            _currentXRotation += -y * rotationSpeed;// Y eksenindeki girişe göre X ekseninde dönme miktarını güncelle.
            _currentXRotation = Mathf.Clamp(_currentXRotation, minXRotation, maxXRotation);// X eksenindeki dönme miktarını belirlenen minimum ve maksimum sınırlar arasında kısıtla.
            transform.eulerAngles = new Vector3(_currentXRotation, transform.eulerAngles.y + (x * rotationSpeed), 0.0f);// Kamera ya da nesnenin Euler açılarını kullanarak yeni rotasyonu ayarla. X ekseni için hesaplanan rotasyon, Y ekseni için girdiye bağlı dönüş, Z ekseni sabit.
            
            //movement 

            Vector3 foward = _camera.transform.forward;// Kameranın ileri vektörünü al ve y eksenini sıfırla, sonra vektörü normalleştir.
            foward.y = 0.0f;// Y ekseni bileşenini sıfırla, böylece sadece yatay düzlemde hareket eder.
            foward.Normalize();// Vektörü birim vektör haline getir, yönü koruyarak büyüklüğünü 1 yap.

            Vector3 right = _camera.transform.right;// Kameranın sağ yönlü vektörünü al, bu hareketin yönünü belirlemek için kullanılacak.

            float moveX = Input.GetAxisRaw("Horizontal");//yatay eksende hareket
            float moveZ = Input.GetAxisRaw("Vertical");//dikey eksende hareket

            Vector3 direction = foward * moveZ + right * moveX;//hareket yönünü oluştur
            direction.Normalize();//hareket eşit olması için hızlarını normalleştir

            direction *= moveSpeed * Time.deltaTime;//hareketi hızla ve framle çarp
            transform.position += direction;//hareketin poziyonuna eşitle



        }
    }
}
