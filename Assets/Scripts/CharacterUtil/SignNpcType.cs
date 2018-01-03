public enum NpcTypeInMainCityEnum
{
    T_OpenShop,
}
public class SignNpcType : UnityEngine.MonoBehaviour
{

    // Use this for initialization
    public NpcTypeInMainCityEnum NpcType = NpcTypeInMainCityEnum.T_OpenShop;

    public NpcTypeInMainCityEnum GetNpcType()
    {
        return NpcType;
    }
}
