using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


public class BuildingPlacement : MonoBehaviour
{
   private bool _currentlyPlacing;//orda bişi varmı yokmu bakıyoz
   private bool _currentlyDeleting;//ordaki silinmişmi diye bakcak
   
   [SerializeField] private float indicatorUpdateRate = 0.05f;//seçtiğimiz obje her framde bizimle gelmesin diye geçikme vericez
   [SerializeField] private float lastUpdateTime;

   private Vector3 _currentIndicatorPosition;//indikatörün şuanki poziyonu
   
   private BuildingPlasement _curretBuildingPreset;//bu scipteki şeyleri eklicez

   public GameObject placemnetIndicator;//game obje olarka indikatör
   public GameObject deleteIndicator;//game obje olarka indikatör

   public GameObject[] panels;

   public void SelectPanel(int index)
   {
      for (int i = 0; i < panels.Length; i++)
      {
         panels[i].transform.DOMoveX(-10, 1);//butona tıklandığunda seçili menüyü açıcak
      }

      panels[index].transform.DOMoveX(10, 1);
   }
   public void BeginNewBuildingPlacement(BuildingPlasement plasement)//yeni bir inşa yapmak için kullanacağımız metod plasement scriptinden gelicekler
   {
      /*if (City.instance.money < plasement.cost)//paraya bakıyoruz varmı yokmu diye
      {
         return;
      }*/
      _currentlyPlacing = true;//yerleşimi al
      _curretBuildingPreset = plasement;//seçtiğim yerleşim
      placemnetIndicator.SetActive(true);//indikatörü true yap ekranda görünsün
   }
   public void ToggleDelete()//silme işlemi 
   {
      _currentlyDeleting = !_currentlyDeleting;
      deleteIndicator.SetActive(_currentlyDeleting);
   }
   
   private void CancelBuildingPlacement()//inşa etmekten vazgeçince ne yapılcağı yazıcaz
   {
      _currentlyPlacing = false;//bişi koyma
      placemnetIndicator.SetActive(false);//yerleşim indikatörünü kapat
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))//eğer esc ye basılmışsa alttakini çalıştır
      {
         CancelBuildingPlacement();
      }

      if (Time.time-lastUpdateTime>indicatorUpdateRate)
      {
         lastUpdateTime = Time.time;
         
         _currentIndicatorPosition = Selector.İnstance.GetCurrentTilePosition();//yeri bulmak için çağırdık

         if (_currentlyPlacing)//şuan inşa edicekse
         {
            placemnetIndicator.transform.position = _currentIndicatorPosition; //inşa indikatörünün yerini şuanki indikatör poziyonuna eşitle
         }
         else if (_currentlyDeleting)//şuan yıkıcaksa
         {
            deleteIndicator.transform.position = _currentIndicatorPosition+new Vector3(0.5f,0,0.5f);//silme indikatörünün yerini şuanki indikatör poziyonuna eşitle
         }
      }

      if (Input.GetMouseButtonDown(0)&&_currentlyPlacing)
      {
         PlaceBuilding();
      }
      else if (Input.GetMouseButtonDown(0)&& _currentlyDeleting)
      {
         DeleteBuilding();
      }
   }

   private void PlaceBuilding()//inşa etme metodu
   {
      GameObject buildingObje =Instantiate(_curretBuildingPreset.prefab, _currentIndicatorPosition, Quaternion.identity);//game obje verdik instantiet et dedik
      Debug.Log(placemnetIndicator.transform.position);
      City.Instance.OnPlacesBuilding(buildingObje.GetComponent<Building>());
      CancelBuildingPlacement();
   }

   private void DeleteBuilding()
   {
      Building buildingToDeleted = City.Instance.buildings.Find(x => x.transform.position == _currentIndicatorPosition);
      if (buildingToDeleted != null)
      {
         City.Instance.OnRemoveBuilding(buildingToDeleted);
      }
     
   }
}
