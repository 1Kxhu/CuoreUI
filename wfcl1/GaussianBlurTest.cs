using System;
using System.Drawing;
using System.Drawing.Imaging;

public static class GaussianBlur
{
    public static void Apply(ref Bitmap bitmap, int radius)
    {
        if (bitmap == null)
            throw new ArgumentNullException(nameof(bitmap));

        int width = bitmap.Width;
        int height = bitmap.Height;

        double[,] kernel = GenerateGaussianKernel(radius);

        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
        int stride = bmpData.Stride;

        unsafe
        {
            byte* ptr = (byte*)bmpData.Scan0;

            // Iterate over each pixel
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double[] rgba = { 0, 0, 0, 0 };

                    // Apply kernel
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int px = Math.Min(Math.Max(x + kx, 0), width - 1);
                            int py = Math.Min(Math.Max(y + ky, 0), height - 1);

                            byte* currentPixel = ptr + py * stride + px * bytesPerPixel;

                            for (int i = 0; i < 4; i++)
                            {
                                rgba[i] += kernel[ky + radius, kx + radius] * currentPixel[i];
                            }
                        }
                    }

                    // Update pixel
                    byte* pixel = ptr + y * stride + x * bytesPerPixel;
                    for (int i = 0; i < 4; i++)
                    {
                        pixel[i] = (byte)Math.Min(Math.Max(rgba[i], 0), 255);
                    }
                }
            }
        }

        bitmap.UnlockBits(bmpData);
    }

    private static double[,] GenerateGaussianKernel(int radius)
    {
        int size = 2 * radius + 1;
        double[,] kernel = new double[size, size];
        double sigma = radius / 20.0; // Adjust sigma as needed

        double sumTotal = 0;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                double distance = x * x + y * y;
                double val = Math.Exp(-distance / (2 * sigma * sigma)) / (2 * Math.PI * sigma * sigma);
                kernel[y + radius, x + radius] = val;
                sumTotal += val;
            }
        }
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                kernel[y, x] /= sumTotal;
            }
        }

        return kernel;
    }
}
