namespace Apps.Cohere.Extensions;

public class ArrayExtensions
{
    public static string[] GenerateFormattedFloatArray(float start, float end, float step)
    {
        var length = (int)Math.Ceiling((end - start) / step) + 1;
        var result = new string[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = (start + i * step).ToString("0.0");
        }

        return result;
    }
}