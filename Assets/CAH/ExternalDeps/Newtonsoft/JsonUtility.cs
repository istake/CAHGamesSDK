 
public static class JsonUtility
{
    public static string ToJson(this object target)
    { 
        return Newtonsoft.Json.JsonConvert.SerializeObject(target);
    }

    public static T JsonToObject<T>(this string jsonString)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
    }
}