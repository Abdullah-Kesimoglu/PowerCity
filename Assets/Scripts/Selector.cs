using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//her yerde kullanmak için ekledik

public class Selector : MonoBehaviour
{
    public static Selector İnstance;//bu scripti statik yaptık ve her yerde erişmesini sağladık
    private Camera _camera;//kamerayı tanımladık


    private void Awake()
    {
        İnstance = this;
    }
    private void Start()
    {
        _camera = Camera.main;
    }

    public Vector3 GetCurrentTilePosition()//şuanki zemindeki pozisyona erişmesi için gerekli metor
    {
        if (EventSystem.current.IsPointerOverGameObject())//eğer uı'a değdiyse aşşadakileri yap yani kanvasa eriştiyse 
        {
            return new Vector3(0, -99, 0);
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);//düz bir zemin oluşturduk
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//kameradan ray atarak masusun tıkladığı yeri belirttik
        float rayOut = 0f;//zemin

        if (plane.Raycast(ray,out rayOut))//eğer attığımız ray  çıktı olarak zemine vuruyorsa
        {
            Vector3 newPosition = ray.GetPoint(rayOut) - new Vector3(0.05f,0f,0.05f);//zeminde dokunduğum yerin poziyonunu al ve verilen vectore at
            newPosition = new Vector3(Mathf.CeilToInt(newPosition.x), 0,Mathf.CeilToInt(newPosition.z));//tıkladığımız yere tam otursun diye poziyon verdik
            return newPosition;
        }
        
        return new Vector3(0,-99,0);
    }
}
