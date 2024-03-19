using System;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var system = new LinearEquationSystem(matrix, freeMembers);
            return system.Solve();
        }
    }

    public class LinearEquationSystem
    {
        private const double Epsilon = 1e-6;
        private readonly double[][] matrix;
        private readonly double[] freeMembers;
        private readonly bool[][] dependentVars;

        public int Height => matrix.Length;
        public int Width => Height > 0 ? matrix[0].Length : 0;

        private int preparedColumnsCount;
        private int dependentVarsCount;

        public LinearEquationSystem(double[][] matrix, double[] freeMembers)
        {
            this.matrix = matrix;
            this.freeMembers = freeMembers;
            dependentVars = new bool[Height][];
            for (var row = 0; row < Height; row++)
                dependentVars[row] = new bool[Width];
        }

        public double[] Solve()
        {
            while (preparedColumnsCount < Width)
                PrepareColumn(dependentVarsCount, preparedColumnsCount);

            for (int row = 0; row < Height; row++)
                if (RowOnlyContainsZeros(row) && Math.Abs(freeMembers[row]) > Epsilon)
                    throw new NoSolutionException("Система уравнений не имеет решения.");

            var solution = new double[Width];
            var definedVars = new bool[Width];

            for (var row = Height - 1; row >= 0; row--)
            {
                if (RowOnlyContainsZeros(row)) continue;
                var encounteredDependentVar = false;
                for (var column = Width - 1; column >= 0 && !encounteredDependentVar; column--)
                {
                    if (dependentVars[row][column])
                    {
                        var sum = Enumerable
                            .Range(column + 1, Width - column - 1)
                            .Sum(i => matrix[row][i] * solution[i]);
                        solution[column] = (freeMembers[row] - sum) / matrix[row][column];
                        encounteredDependentVar = true;
                    }
                    else if (!definedVars[column])
                    {
                        solution[column] = 0;
                    }
                    definedVars[column] = true;
                }
            }
            return solution;
        }

        private bool RowOnlyContainsZeros(int row) => matrix[row].All(x => Math.Abs(x) < Epsilon);

        private void PrepareColumn(int rowIndex, int columnIndex)
        {
            if (preparedColumnsCount == Width) return;

            if (rowIndex >= Height)
            {
                preparedColumnsCount++;
                return;
            }

            var divider = matrix[rowIndex][columnIndex];
            if (Math.Abs(divider) < Epsilon)
            {
                PrepareColumn(rowIndex + 1, columnIndex);
                return;
            }

            for (var row = 0; row < Height; row++)
            {
                if (row == rowIndex) continue;
                var factor = matrix[row][columnIndex] / divider;
                for (var i = columnIndex; i < Width; i++)
                    matrix[row][i] -= factor * matrix[rowIndex][i];
                freeMembers[row] -= factor * freeMembers[rowIndex];
            }

            dependentVars[rowIndex][columnIndex] = true;
            if (rowIndex != preparedColumnsCount)
                SwitchLines(rowIndex, preparedColumnsCount);
            preparedColumnsCount++;
            dependentVarsCount++;
        }

        private void SwitchLines(int i, int j)
        {
            if (i < 0 || i >= Height || j < 0 || j >= Height) return;

            (matrix[i], matrix[j]) = (matrix[j], matrix[i]);
            (freeMembers[i], freeMembers[j]) = (freeMembers[j], freeMembers[i]);
            (dependentVars[i], dependentVars[j]) = (dependentVars[j], dependentVars[i]);
        }
    }
}
