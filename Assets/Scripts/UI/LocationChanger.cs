using System.Collections.Generic;
using UnityEngine;

public class LocationChanger : MonoBehaviour
{
   [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();
   [SerializeField] private SpriteRenderer background;
   [SerializeField] private Stats itchi;
   public void ChangeBackground(int i)
   {
      background.sprite = backgrounds[i];
      
      Stats.Background bg = (Stats.Background)i;
      itchi.ChangeDecayRates(bg);
   }
}
