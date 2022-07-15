namespace HackedDesign
{
    [System.Serializable]
    public class MechData
    {
        public int scrap = 0;
        public float armour = 100.0f;
        public float shield = 0.0f;
        public float heat = 0.0f;
        public float coolant = 100.0f;
 

        public void Reset(MechSettings settings)
        {
            scrap = settings.startingScrap;
            armour = settings.startingArmour;
            shield = settings.startingShield;
            heat = settings.startingHeat;
            coolant = settings.startingCoolant;
        }
    }
}