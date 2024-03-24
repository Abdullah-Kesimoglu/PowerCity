using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
   public GameObject buildPanel;
   public GameObject naturePanel;

   private void Start()
   {
      buildPanel.SetActive(false);
      
   }

   public void BuildPanelPro()
   {
      if (!buildPanel.activeSelf)
      {
         buildPanel.SetActive(true);
      }
      else
      {
         buildPanel.SetActive(false);
      }
   }

   public void NaturePanelPro()
   {
      if (!naturePanel.activeSelf)
      {
         naturePanel.SetActive(true);
      }
      else
      {
         naturePanel.SetActive(false);
      }
   }
}
