using System;
using System.Collections;

namespace SolutionMethodsSLAE.Model.Data
{
	public class Matrix : ICloneable, IEnumerable
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
		}
		public Matrix(int size) : this(size, size)
		{ }
		#endregion

		#region Methods
		public bool IsConvergent()
		{
			for (int i = 0; i < RowCount; i++)
			{
				double sum = 0;

				for (int j = 0; j < ColumnCount; j++)
					if (j != i)
						sum += Math.Abs(this[i, j]);

				if (Math.Abs(this[i, i]) <= sum)
					return false;
			}
			return true;
		}
		public bool IsDiagonal(int offset)
		{
			for (int i = 0; i < RowCount; i++)
			{
				double sum = 0;

				for (int j = 0; j < ColumnCount; j++)
					if (j != i - offset && j != i + offset && j != i)
						sum += Math.Abs(this[i, j]);

				if (sum != 0)
					return false;
			}
			return true;
		}

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
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}

		public void AddColumn()
		{
			double[,] newMatrix = new double[RowCount, ColumnCount + 1];
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}

		public void RemoveRow()
		{
			double[,] newMatrix = new double[RowCount - 1, ColumnCount];
			for (int i = 0; i < RowCount - 1; i++)
				for (int j = 0; j < ColumnCount; j++)
					newMatrix[i, j] = _matrix[i, j];
			_matrix = newMatrix;
		}

		public void RemoveColumn()
		{
			double[,] newMatrix = new double[RowCount, ColumnCount - 1];
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount - 1; j++)
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
		{
			return _matrix.GetEnumerator();
		}

		public static Matrix operator +(Matrix first, Matrix second)
		{
			if (first.RowCount != second.RowCount ||
				first.ColumnCount != second.ColumnCount)
				throw new ArgumentException();

			Matrix res = new Matrix(first.RowCount);
			for (int i = 0; i < res.RowCount; i++)
			{
				for (int j = 0; j < res.ColumnCount; j++)
					res.SetValue(i, j, first[i, j] + second[i, j]);
			}
			return res;
		}

		public static Matrix operator -(Matrix first, Matrix second)
		{
			if (first.RowCount != second.RowCount ||
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
			if (first.ColumnCount != second.RowCount)
				throw new ApplicationException("Матрицы нельзя перемножить");

			Matrix rez = new Matrix(first.RowCount, second.ColumnCount);

			for (int i = 0; i < first.RowCount; i++)
			{
				for (int j = 0; j < second.ColumnCount; j++)
				{
					for (int k = 0; k < second.RowCount; k++)
					{
						rez[i, j] += first[i, k] * second[k, j];
					}
				}
			}
			return rez;
		}

		public static Matrix operator /(Matrix first, Matrix second)
			=> first * MatrixOperations.Inverse(second,0);

		public static Matrix operator *(Matrix first, double second)
		{
			Matrix rez = (Matrix)first.Clone();
			for (int i = 0; i < rez.ColumnCount; i++)
				for (int j = 0; j < rez.RowCount; j++)
					rez[i, j] *= second;
			return rez;
		}

		public static Matrix operator *(double first, Matrix second) =>
			second * first;

		public static Matrix operator /(Matrix first, double second)
		{
			Matrix rez = (Matrix)first.Clone();
			for (int i = 0; i < rez.ColumnCount; i++)
				for (int j = 0; j < rez.RowCount; j++)
					rez[i, j] /= second;
			return rez;
		}

		#endregion
	}
}
