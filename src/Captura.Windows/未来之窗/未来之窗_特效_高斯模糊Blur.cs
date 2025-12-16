using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captura.Windows.未来之窗
{
    public class 未来之窗_特效_高斯模糊Blur
    {
        public int Radius { get; set; } = 5;

        public ColorMatrix CreateColorMatrix()
        {
            float[][] matrix = new float[5][];
            for (int i = 0; i < 5; i++)
                matrix[i] = new float[5];

            float blurAmount = 1.0f / (Radius * Radius);
            for (int x = -Radius; x <= Radius; x++)
            {
                for (int y = -Radius; y <= Radius; y++)
                {
                    int indexX = x + Radius;
                    int indexY = y + Radius;
                    if (indexX >= 0 && indexX < 5 && indexY >= 0 && indexY < 5) // 简化的5x5矩阵
                        matrix[indexY][indexX] = blurAmount;
                }
            }
            // 为了保持亮度，对矩阵进行归一化
            float sum = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    sum += matrix[i][j];
            if (sum > 0)
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        matrix[i][j] /= sum;

            return new ColorMatrix(matrix);
        }
    }
}
