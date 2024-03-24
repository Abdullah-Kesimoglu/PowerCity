using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class City : MonoBehaviour
{
    [SerializeField] private int money;//paramız
    [SerializeField] private int day;//geçen gün
    [SerializeField] private int currentPopulation;//şuanki nüfüs
    [SerializeField] private int currentJobs;//şuanki çalışanlar
    [SerializeField] private int currentFood;//şuanki yemek
    [SerializeField] private int currentEnergy;//şuanki enerji
    [SerializeField] private int maxPopulation;//max nüfüs
    [SerializeField] private int maxJobs;//max çalışanlar
    [SerializeField] private int inComePerJob;//her turda gelcek şeyler
    
     [SerializeField] private TextMeshProUGUI _statsText;//alttaki paranın vb göründüğü yerler
    [SerializeField] public List<Building> buildings = new List<Building>();//binaların listesi

    public static City Instance;//başka scripten erişlmesini sağladık

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentEnergy = 100;
        UpdateStatText();
    }

    public void OnPlacesBuilding(Building building)
    {
        // Enerji kontrolü ekle
        if (currentEnergy - building.preset.spentEnergyPerTurn < 0) // Yeterli enerji yoksa
        {
            Debug.Log("Not enough energy to place this building.");
            return; // Bina eklemeyi durdur ve çık
        }

        // Yeterli enerji varsa, bina ekleme işlemini sürdür
        money -= building.preset.cost;
        maxPopulation += building.preset.population;
        maxJobs += building.preset.jobs;
        currentEnergy -= building.preset.spentEnergyPerTurn; // Binanın enerji tüketimini hesaba kat
        buildings.Add(building);
        UpdateStatText();
    }
    
    public void OnRemoveBuilding(Building building)
    {
        maxPopulation -= building.preset.population;
        maxJobs -= building.preset.jobs;
        buildings.Remove(building);
        Destroy(building.gameObject);
        UpdateStatText();
    }

    private void UpdateStatText()
    {
        _statsText.text = string.Format("Day: {0} Money: {1} Population: {2} / {3} Jobs: {4} / {5} Food: {6} Energy:{7}",
           day, money, currentPopulation, maxPopulation, currentJobs, maxJobs, currentFood,currentEnergy);
    }

    public void EndTurn()
    {
        CalculateEnergy();
        CalculateMoney();
        CalculatePopulation();
        CalculateJobs();
        CalculateFood();
        UpdateStatText();
    }

    private void CalculateFood()
    {
        currentFood = 0;
        foreach (Building building in buildings)
        {
            currentFood += building.preset.food;
        }
    }

    private void CalculateJobs()
    {
        currentJobs = Mathf.Min(currentPopulation, maxJobs);
    }
    private void CalculatePopulation()
    {
        if (currentFood >= currentPopulation)
        {
            // Burada, popülasyonunuzun artması için gıda miktarını kullanıyorsunuz.
            // Ancak, popülasyon artış hızınız (örneğin, currentFood / 4) fazla olabilir veya
            // popülasyonunuzun azalmasına neden olacak başka bir koşul olabilir.
            int potentialIncrease = currentFood / 4; // Örnek bir artış miktarı
            currentPopulation += potentialIncrease;
            currentPopulation = Mathf.Min(currentPopulation, maxPopulation); // Maksimum popülasyona ulaşma kontrolü
            currentFood -= potentialIncrease; // Artış için kullanılan gıdayı çıkarın
        }
        else
        {
            // Gıda yetersizse, popülasyon azalabilir.
            currentPopulation = Mathf.Max(0, currentFood); // Popülasyonu sıfırın altına düşürmemek için kontrol
        }
    }
    private void CalculateMoney()
    {
        // Enerji pozitifse, her bir iş için para ekleyin.
        if (currentEnergy > 0)
        {
            money += currentJobs * inComePerJob;
        }

        // Her bir bina için tur başına maliyeti çıkarın.
        foreach (Building building in buildings)
        {
            money -= building.preset.costPerTurn;

            // Eğer enerji 0 ise ve bina bir fabrikaysa, bu binanın para üretimini durdur.
            if (currentEnergy <= 0 && building.preset.prefab)
            {
                // Fabrika para üretimini burada durdurabilirsiniz.
                // Örneğin, fabrika tarafından üretilen parayı azaltabilir veya
                // para üretimi için ek bir kontrol ekleyebilirsiniz.
            }
        }
    }

    private void CalculateEnergy()
    {
        // Enerji hesaplaması için herhangi bir başlangıç sıfırlamasını kaldırın.
        // Bu şekilde, enerji birikimli olarak hesaplanır.

        foreach (Building building in buildings)
        {
            // Her binadan gelen enerjiyi ekleyin.
            currentEnergy += building.preset.energyPerTurn; // Her turda ve binada alınan enerji
            // Aynı zamanda her binanın harcadığı enerjiyi çıkarın.
            currentEnergy -= building.preset.spentEnergyPerTurn; // Bu da harcanan enerji
        }

        // Enerjinin negatife düşmemesi için kontrol ekleyebilirsiniz.
        currentEnergy = Mathf.Max(0, currentEnergy);
    }
    
        
} 
