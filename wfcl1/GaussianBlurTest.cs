using Accord.Imaging.Filters;
using System;
using System.Drawing;

public static class GaussianBlur
{
    public static Bitmap Apply(Bitmap image, double sigma, int value)
    {
        int radius = CalculateRadius(sigma, value);

        Accord.Imaging.Filters.GaussianBlur filter = new Accord.Imaging.Filters.GaussianBlur(sigma, radius);
        Bitmap blurredImage = filter.Apply(image);

        return blurredImage;
    }

    private static int CalculateRadius(double sigma, int value)
    {
        const int minValue = 1;
        const int maxValue = 25;

        int radius = (int)Math.Round(sigma * 3);
        radius = Math.Max(radius, minValue);
        radius = Math.Min(radius, maxValue);

        return radius;
    }
}
