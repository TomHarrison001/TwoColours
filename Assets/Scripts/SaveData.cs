[System.Serializable]
public class SaveData
{
    public int fewestDeaths;
    public int deaths;
    public int level;

    public SaveData(GameController controller)
    {
        fewestDeaths = controller.FewestDeaths;
        deaths = controller.Deaths;
        level = controller.Level;
    }
}
