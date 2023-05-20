namespace BreachersAPI.Utils; 

public static class Extensions {
    public static bool HasValue(this string str) {
        return !string.IsNullOrEmpty(str);
    }
    
    public static bool HasValue(this object obj) {
        return obj != null;
    }
}