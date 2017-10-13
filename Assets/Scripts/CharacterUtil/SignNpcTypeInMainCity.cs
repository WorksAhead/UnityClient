public enum NpcTypeInMainCityEnum
{
    T_OpenShop,
}
public class SignNpcTypeInMainCity : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    public NpcTypeInMainCityEnum NpcType = NpcTypeInMainCityEnum.T_OpenShop;

    public NpcTypeInMainCityEnum GetNpcType()
    {
        return NpcType;
    }
}
