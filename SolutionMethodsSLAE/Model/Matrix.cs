using System;
using System.Collections;
using System.Collections.Generic;

namespace SolutionMethodsSLAE.Model
{
	internal class Matrix : ICloneable, IEnumerable
	{
		private double[,] _matrix;

		#region Propertys
		public double this[int firstIndex, int lastIndex]
		{
			get => GetValue(firstIndex, lastIndex);
			set => SetValue(firstIndex, lastIndex, value);
		}

		public int RowCount { get => _matrix.GetLength(0); }
		public int ColumnCount { get => _matrix.GetLength(1); }
		#endregion

		#region Constructor

		public Matrix(int rowCount, int colCount)
		{
			_matrix = new double[rowCount, colCount];
			new List<int>().ToArray();
		}
		public Matrix(int size)
        {
			_matrix = new double[size, size];
			new List<int>().ToArray();
        }
		public Matrix(double[,] matrix)
        {
            _matrix = matrix;
        }

        #endregion

        #region Methods
        public double GetValue(int rowIndex, int colIndex)
		{
			return _matrix[rowIndex, colIndex];
		}
		public void SetValue(int rowIndex, int colIndex, double value)
		{
			_matrix[rowIndex, colIndex] = value;
		}
		public void AddRow()
		{
			double[,] newMatrix = new double[RowCount + 1, ColumnCount];
			for (int i =0; i<RowCount;i++)
				for (int j =0; j<ColumnCount;j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}

		public void AddColumn()
		{
			double[,] newMatrix = new double[RowCount, ColumnCount+1];
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}
		public void RemoveRow()
		{
			double[,] newMatrix = new double[RowCount - 1, ColumnCount];
			for (int i = 0; i < RowCount-1; i++)
				for (int j = 0; j < ColumnCount; j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}
		public void RemoveColumn()
		{
			double[,] newMatrix = new double[RowCount, ColumnCount-1];
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount-1; j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}
		public double[,] ToArray()
		{
			double[,] matrix = new double[RowCount, ColumnCount];
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					matrix[i, j] = this[i, j];
			return matrix;
		}

		public object Clone()
		{
			Matrix matrix = new Matrix(RowCount, ColumnCount);
			matrix._matrix = ToArray();
			return matrix;
		}

		public IEnumerator GetEnumerator()
			=> _matrix.GetEnumerator();

		public static Matrix operator +(Matrix first, Matrix second)
		{
			if (first.RowCount != second.RowCount &&
				first.ColumnCount != second.ColumnCount)
				throw new ArgumentException();

			Matrix res = new Matrix(first.RowCount);
			for (int i = 0; i < res.RowCount; i++)
			{
				for (int j = 0; j < res.ColumnCount; j++)
					res[i, j] = first[i, j] + second[i, j];
			}
			return res;
		}
		public static Matrix operator -(Matrix first, Matrix second)
		{
			if (first.RowCount != second.RowCount &&
				first.ColumnCount != second.ColumnCount)
				throw new ArgumentException();

			Matrix res = new Matrix(first.RowCount);
			for (int i = 0; i < res.RowCount; i++)
			{
				for (int j = 0; j < res.ColumnCount; j++)
					res.SetValue(i, j, first[i, j] - second[i, j]);
			}
			return res;
		}
		public static Matrix operator *(Matrix first, Matrix second)
		{
			if (first.RowCount != second.RowCount &&
				first.ColumnCount != second.ColumnCount)
				throw new ArgumentException();

			Matrix res = new Matrix(first.RowCount);
			for (int i = 0; i < first.RowCount; i++)
			{
				for (int j = 0; j < second.ColumnCount; j++)
				{
					for (int k = 0; k < second.RowCount; k++)
					{
						res[i, j] += first[i, k] * first[k, j];
					}
				}
			}
			return res;
		}
		public static Matrix operator /(Matrix first, Matrix second)
			=> first * MatrixOperations.Inverse(second, 0);

		#endregion
	}
}
