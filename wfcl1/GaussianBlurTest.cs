using System;
using System.Drawing;
using System.Drawing.Imaging;

public static class GaussianBlur
{
    public unsafe static void Apply(ref Bitmap bitmap, int radius)
    {
        if (bitmap == null)
            throw new ArgumentNullException(nameof(bitmap));

        int width = bitmap.Width;
        int height = bitmap.Height;

        double[,] kernel = GenerateGaussianKernel(radius);

        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
        int stride = bmpData.Stride;

        byte* ptr = (byte*)bmpData.Scan0;

        int[] yOffset = new int[2 * radius + 1];
        for (int ky = -radius; ky <= radius; ky++)
        {
            yOffset[ky + radius] = ky * stride;
        }

        for (int y = 0; y < height; y++)
        {
            byte* pixel = ptr + y * stride;

            for (int x = 0; x < width; x++)
            {
                double[] rgba = { 0, 0, 0, 0 };

                for (int ky = -radius; ky <= radius; ky++)
                {
                    int offsetY = yOffset[ky + radius];
                    byte* currentPixel = pixel + offsetY;

                    for (int kx = -radius; kx <= radius; kx++)
                    {
                        int px = x + kx;
                        int py = y + ky;

                        if (px >= 0 && px < width && py >= 0 && py < height)
                        {
                            currentPixel = pixel + offsetY + kx * bytesPerPixel;

                            for (int i = 0; i < 4; i++)
                            {
                                rgba[i] += kernel[ky + radius, kx + radius] * currentPixel[i];
                            }
                        }
                    }
                }

                byte* targetPixel = pixel;
                for (int i = 0; i < 4; i++)
                {
                    targetPixel[i] = (byte)(rgba[i] < 0 ? 0 : (rgba[i] > 255 ? 255 : rgba[i]));
                }

                pixel += bytesPerPixel;
            }
        }
        bitmap.UnlockBits(bmpData);
    }

    public unsafe static void ApplyBox(ref Bitmap bitmap, int radius)
    {
        if (bitmap == null)
            throw new ArgumentNullException(nameof(bitmap));

        int width = bitmap.Width;
        int height = bitmap.Height;

        double[,] kernel = GenerateBoxKernel(radius);

        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 7;
        int stride = bmpData.Stride;

        byte* ptr = (byte*)bmpData.Scan0;

        int[] yOffset = new int[2 * radius + 1];
        for (int ky = -radius, idx = 0; ky <= radius; ky++, idx++)
        {
            yOffset[idx] = ky * stride;
        }

        for (int y = 0; y < height; y++)
        {
            byte* pixel = ptr + y * stride;

            for (int x = 0; x < width; x++)
            {
                double red = 0, green = 0, blue = 0, alpha = 0;

                for (int ky = -radius, kyIdx = 0; ky <= radius; ky++, kyIdx++)
                {
                    int offsetY = yOffset[kyIdx];
                    byte* currentPixel = pixel + offsetY;

                    for (int kx = -radius; kx <= radius; kx++)
                    {
                        int px = x + kx;
                        int py = y + ky;

                        if (px >= 0 && px < width && py >= 0 && py < height)
                        {
                            currentPixel = pixel + offsetY + kx * bytesPerPixel;

                            double kernelValue = kernel[kyIdx, kx + radius];

                            red += kernelValue * currentPixel[0];
                            green += kernelValue * currentPixel[1];
                            blue += kernelValue * currentPixel[2];
                            alpha += kernelValue * currentPixel[3];
                        }
                    }
                }

                byte* targetPixel = pixel;
                targetPixel[0] = (byte)(red < 0 ? 0 : (red > 255 ? 255 : red));
                targetPixel[1] = (byte)(green < 0 ? 0 : (green > 255 ? 255 : green));
                targetPixel[2] = (byte)(blue < 0 ? 0 : (blue > 255 ? 255 : blue));
                targetPixel[3] = (byte)(alpha < 0 ? 0 : (alpha > 255 ? 255 : alpha));

                pixel += bytesPerPixel;
            }
        }
        bitmap.UnlockBits(bmpData);
    }

    private static unsafe double[,] GenerateBoxKernel(int radius)
    {
        int size = radius * 2 + 1;
        double[,] kernel = new double[size, size];
        double value = 1.0 / (size * size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                kernel[i, j] = value;
            }
        }

        return kernel;
    }



    private static unsafe double[,] GenerateGaussianKernel(int radius)
    {
        int size = 2 * radius + 1;
        double[,] kernel = new double[size, size];
        double sigma = radius / 5.0;
        double twoSigmaSquared = 2 * sigma * sigma;
        double constant = 1.0 / (2 * Math.PI * sigma * sigma);
        double sumTotal = 0;
        double invTwoSigmaSquared = 1.0 / twoSigmaSquared;
        double invSumTotal;

        fixed (double* kernelPtr = &kernel[0, 0])
        {
            double* ptr = kernelPtr;

            double invTwoSigmaSquaredTimesConstant = invTwoSigmaSquared * constant;

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    double val = Math.Exp(-(x * x + y * y) * invTwoSigmaSquared) * constant;

                    *ptr = val;
                    sumTotal += val;
                    ptr++;
                }
            }

            invSumTotal = 1.0 / sumTotal;

            for (int i = 0; i < size * size; i++)
            {
                kernelPtr[i] *= invSumTotal;
            }
        }

        return kernel;
    }
}
