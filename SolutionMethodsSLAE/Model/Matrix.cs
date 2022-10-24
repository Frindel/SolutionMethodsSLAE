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
		{
			return _matrix.GetEnumerator();
		}

		//TODO: реализовать перегрузку операторов для сложения, вычетания, умножения и деления матриц

		#endregion
	}
}
