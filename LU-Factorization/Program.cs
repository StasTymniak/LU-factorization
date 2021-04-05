using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LU_Factorization
{

    public class Matrix
    {
        public double[,] data;
        public double[,] matrixA;
        static private int numofswap;
        public double[] vectorB;

        public double det = 1;
        public int NumOfswap()
        {
            return numofswap;
        }
        public int NbRows
        {
            get;
            private set;
        }

        public int NbCols
        {
            get;
            private set;
        }

        public double this[int i, int j]
        {
            get
            {
                return data[i, j];
            }
            set
            {
                data[i, j] = value;
            }
        }

        public Matrix(int m, int n)
        {

            NbRows = m;
            NbCols = n;

            data = new double[m, n];
            vectorB = new double[n];
            matrixA = new double[m, m];

        }

        public double[,] FillMatrix()
        {
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    Console.Write($"Input m[{i}][{j}]: ");
                    data[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            return data;
        }
        public double[,] FillMatrixZero()
        {
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    data[i, j] = 0;
                }
            }
            return data;
        }
        public double[,] FillMatrixDiagolanOne()
        {
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    if (i == j)
                        data[i, j] = 1;
                }
            }
            return data;
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    Console.Write(String.Format("{0,5}", data[i, j]));
                }
                Console.Write("\n");
            }
        }

        public double[,] SwapRow(int k, int i)
        {
            for (int l = 0; l < NbCols; l++)
            {
                double temp = data[i, l];
                data[i, l] = data[k, l];
                data[k, l] = temp;
            }
            numofswap++;
            return data;
        }

        public int MaxInColum(int index)
        {
            int matr_MaxIndex = index;
            double Max = data[index, index];
            for (int i = index; i < NbRows; i++)
            {
                if (Math.Abs(data[i, index]) > Math.Abs(Max))
                {
                    Max = data[i, index];
                    matr_MaxIndex = i;
                }
            }
            return matr_MaxIndex;
        }

        public Matrix MultiplyMatrix(Matrix A, Matrix B)
        {
            int rA = A.NbRows;
            int cA = A.NbCols;
            int rB = B.NbRows;
            int cB = B.NbCols;
            double temp = 0;
            Matrix multedMatrix = new Matrix(rA, cB);
            if (cA != rB)
            {
                Console.WriteLine("matrik can't be multiplied !!");
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        multedMatrix[i, j] = temp;
                    }
                }
            }
            return multedMatrix;
        }

    }

    
    class Program
    {

        public static void PrintVector(double[] vector)
        {
            for(int i = 0; i < vector.Count(); i++)
            {
                Console.Write($" {vector[i]}");
            }
        }

     
        static void Main(string[] args)
        {
            double detL = 1, detU = 1;
            bool isEqual = true;
            Console.Write("N: ");
            int n = Convert.ToInt32(Console.ReadLine());
            Matrix matrixA = new Matrix(n, n);
            Matrix matrixL = new Matrix(n, n);
            Matrix matrixU = new Matrix(n, n);
            double[] vectorB = new double[n];
            double[] vectorX = new double[n];
            double[] vectorY = new double[n];
            Matrix multedMatrix = new Matrix(n, n);
            Console.WriteLine("Input matrix:");
            matrixA.FillMatrix();
            
           matrixL.FillMatrixDiagolanOne();

            Console.WriteLine("Input vectorB:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Input vectorB[{i}] : ");
                vectorB[i] = Convert.ToInt32(Console.ReadLine());
            }
            //LU- factorization
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    double sumLU = 0;
                    string sum = "";
                    for (int k = 0; k <= j - 1; k++)
                    {
                        sumLU += (matrixL[i, k] * matrixU[k, j]);
                    }
                    matrixL[i, j] = (matrixA[i, j] - sumLU) / matrixU[j, j];
                    Console.Write($"L[{i+1},{j+1}] = {matrixL[i, j]} ");

                }
                Console.WriteLine();
                for (int j = i; j < n; j++)
                {
                    double sumLU = 0;
                    for (int k = 0; k <= j - 1; k++)
                    {
                        sumLU += (matrixL[i, k] * matrixU[k, j]);
                    }
                    matrixU[i, j] = matrixA[i, j] - sumLU;
                    Console.Write($"U[{i + 1},{j + 1}] = {matrixU[i, j]} ");
                }
                //check if matrix is singular
                if (matrixL[i, i] == 0)
                    throw new Exception("Diagonal matrix Elem is 0");
                if (matrixU[i, i] == 0)
                    throw new Exception("Diagonal matrix Elem is 0");
            }

            //solve first SLE(System of linear equations) to find vector Y
            for (int i = 0; i < n; i++)
            {
                double sumLY = 0;
                for (int k = 0; k <= i - 1; k++)
                {
                    sumLY += (matrixL[i, k] * vectorY[k]);
                }
                vectorY[i] = vectorB[i] - sumLY;
            }

            //solve second SLE(System of linear equations) to find vector X
            for (int i = n-1; i >= 0; i--)
            {
                double sumUX = 0;
                for (int k = i+1; k <n; k++)
                {
                    sumUX += (matrixU[i, k] * vectorX[k]);
                }
                vectorX[i] = (vectorY[i] - sumUX)/matrixU[i,i];
            }

            //Check if L*U equal to matrix A
            
            multedMatrix= multedMatrix.MultiplyMatrix(matrixL, matrixU);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrixA[i, j] != multedMatrix[i, j])
                        isEqual = false;
                }
            }
            if (isEqual)
                Console.WriteLine("LU = A");
            else
                Console.WriteLine("LU!=A");

            //Check final result Ax=B
            for (int i = 0; i < n; i++)
            {
                double temp_res = 0;
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"({matrixA[i, j]}*{vectorX[j]})");
                    temp_res += matrixA[i, j] * vectorX[j];
                    if (j != n - 1)
                    {
                        Console.Write("+");
                    }
                }

                Console.Write($"={temp_res}\t");
                if (temp_res != vectorB[i])
                {
                    Console.WriteLine("False");
                }
                else
                {
                    Console.WriteLine("True");
                }    
            }

            //Count determinant of our matrix using LU factorization
            for (int i = 0; i < n; i++)
            {
                detL = detL * matrixL[i, i];
                detU = detU * matrixU[i, i];
            }

            //print information on Console
            Console.WriteLine("A:");
            matrixA.PrintMatrix();
            Console.WriteLine();
            Console.WriteLine("L:");
            matrixL.PrintMatrix();
            Console.WriteLine();
            Console.WriteLine("U:");
            matrixU.PrintMatrix();
            Console.WriteLine();
            Console.WriteLine("Vector Y:");
            PrintVector(vectorY);
            Console.WriteLine();
            Console.WriteLine("Vector X:");
            PrintVector(vectorX);
            Console.WriteLine();
            Console.WriteLine($"Det using LU: detL = {detL} detU = {detU} => detA = {detL*detU}");
        }
    }
}
