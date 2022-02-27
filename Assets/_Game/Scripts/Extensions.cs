using System.Collections.Generic;

public static class Extensions
{
    // check if a value is inside an object, i miss javascript
    public static bool isInside<T>(this T value, T[] array)
    {
        bool itIs = false;
        for (int i = 0; i < array.Length; i++)
            if (EqualityComparer<T>.Default.Equals(array[i], value))
                itIs = true;
        return itIs;
    }

}