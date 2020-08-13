using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using KpBot.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace GamesAndStuff.Pages.TentsAndTrees
{
    public class IndexModel : PageModel
    {
        private DiscordContext gameContext;

        public IndexModel(DiscordContext context)
        {
            gameContext = context;
        }

        public enum Status
        {
            None,
            Grass,
            Tree,
            Tent
        };

        public class GridCell : ICloneable
        {
            public Status Status { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public decimal Probability { get; set; }

            public object Clone()
            {
                return new GridCell
                {
                    Row = Row,
                    Column = Column,
                    Status = Status,
                    Probability = Probability
                };
            }
        }

        public class Constraint : ICloneable
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public int NumberOfTents { get; set; }

            public object Clone()
            {
                return new Constraint
                {
                    Row = Row,
                    Column = Column,
                    NumberOfTents = NumberOfTents
                };
            }
        }

        [BindProperty]
        public List<GridCell> Grid { get; set; }

        [BindProperty]
        public List<Constraint> Constraints { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 7;

        public List<SelectListItem> numbers;

        public void OnGet()
        {

            var s = new Student();
            gameContext.Students.Add(s);
            gameContext.SaveChanges();


            InitiateNumberList();
            Grid = new List<GridCell>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid.Add(new GridCell
                    {
                        Row = i,
                        Column = j,
                        Status = Status.None
                    });
                }
            }
        }

        public IActionResult OnPost()
        {
            var backupGrid = Grid.Clone();
            var backupConstraints = Constraints.Clone();
            PlantGrassOnZeroConstraints(Grid, Constraints);
            PlantGrassOnCellsWithNoAdjacentTree(Grid);
            UpdateProbability(Grid, Constraints);
            Grid = Solve(Grid, Constraints, 0);

            if (Grid == null)
            {
                Grid = backupGrid;
                Constraints = backupConstraints;
            }
            InitiateNumberList();
            return Page();
        }

        public void InitiateNumberList()
        {
            numbers = new List<SelectListItem>();
            for (int i = 4; i < 30; i++)
            {
                numbers.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
        }

        public List<GridCell> Solve(List<GridCell> grid, List<Constraint> constraints, int iteration)
        {
            if (grid.Count(c => c.Status == Status.None) == 0)
            {
                return null;
            }

            //Step 1: Clone the all the lists
            List<GridCell> clonedGrid = grid.Clone();
            List<Constraint> clonedConstraints = constraints.Clone();

            //Step 2: Find the slot with highest probability to have a tent
            GridCell cell = clonedGrid.Where(c => c.Status == Status.None).OrderBy(c => c.Probability).Last();

            //Step 3: Put a tent on it
            if (AssignTent(cell, clonedGrid, clonedConstraints)) //valid
            {
                //Puzzle solved
                if (IsComplete(clonedGrid, clonedConstraints))
                {
                    return clonedGrid;
                }
                if (clonedGrid.Count(c => c.Status == Status.None) != 0) // if not filled but incomplete
                {
                    //Keep going
                    var t = Solve(clonedGrid, clonedConstraints, iteration += 1);
                    if (t != null)
                    {
                        return t;
                    }
                }
            }

            //Find the original cell and use grass
            GridCell originalCell = grid.First(c => c.Row == cell.Row && c.Column == cell.Column);
            originalCell.Status = Status.Grass;

            return Solve(grid, constraints, iteration);
        }

        public void UpdateProbability(List<GridCell> grid, List<Constraint> constraints)
        {
            for (int index = 0; index < grid.Count(); index++)
            {
                decimal rowProbability = 0;
                decimal columnProbability = 0;

                Constraint rowConstraint = constraints.FirstOrDefault(c => c.Row == grid[index].Row);
                Constraint columnConstraint = constraints.FirstOrDefault(c => c.Column == grid[index].Column);

                int numberOfEmptySlotsInRow = grid.Where(c => c.Row == grid[index].Row && c.Status == Status.None).Count();
                int numberOfEmptySlotsInColumn = grid.Where(c => c.Column == grid[index].Column && c.Status == Status.None).Count();
                if (grid[index].Status == Status.None)
                {
                    if (rowConstraint.NumberOfTents == 0)
                    {
                        rowProbability = 0;
                    }
                    else
                    {
                        rowProbability = ((decimal)PermutationsAndCombinations.Combination(numberOfEmptySlotsInRow - 1, rowConstraint.NumberOfTents - 1)) / PermutationsAndCombinations.Combination(numberOfEmptySlotsInRow, rowConstraint.NumberOfTents);
                    }

                    if (columnConstraint.NumberOfTents == 0)
                    {
                        columnProbability = 0;
                    }
                    else
                    {
                        columnProbability = ((decimal)PermutationsAndCombinations.Combination(numberOfEmptySlotsInColumn - 1, columnConstraint.NumberOfTents - 1)) / PermutationsAndCombinations.Combination(numberOfEmptySlotsInColumn, columnConstraint.NumberOfTents);
                    }
                }

                if (rowProbability == 0 || columnProbability == 0)
                {
                    grid[index].Probability = 0;
                }
                else if (rowProbability == 1 || columnProbability == 1)
                {
                    grid[index].Probability = 1;
                }
                else
                {
                    grid[index].Probability = rowProbability * columnProbability;
                }
            }
        }

        //Plant grass if there are rows/columns with 0 tents
        public void PlantGrassOnZeroConstraints(List<GridCell> grid, List<Constraint> constraints)
        {
            foreach (Constraint constraint in constraints)
            {
                if (constraint.NumberOfTents == 0)
                {
                    foreach (GridCell cell in grid.Where(c => (c.Row == constraint.Row || c.Column == constraint.Column) && c.Status == Status.None))
                    {
                        cell.Status = Status.Grass;
                    }
                }
            }
        }

        //Some cells cannot have tents because no trees around
        public void PlantGrassOnCellsWithNoAdjacentTree(List<GridCell> grid)
        {
            for (int i = 0; i < grid.Count(); i++)
            {
                if (grid[i].Status != Status.None)
                {
                    continue;
                }

                if (GetAdjacentCells(grid[i], grid, false).FirstOrDefault(c => c.Status == Status.Tree) == null)
                {
                    grid[i].Status = Status.Grass;
                }
            }
        }

        public List<GridCell> GetAdjacentCells(GridCell cell, List<GridCell> grid, bool diagonal)
        {
            int rowIndex = cell.Row;
            int columnIndex = cell.Column;

            var adjacentCells = grid.Where(c => (Math.Abs(c.Row - rowIndex) <= 1 && Math.Abs(c.Column - columnIndex) <= 1) && c != cell);
            if (!diagonal)
            {
                adjacentCells = adjacentCells.Where(c => !(Math.Abs(c.Row - rowIndex) == 1 && Math.Abs(c.Column - columnIndex) == 1));
            }

            return adjacentCells.ToList();
        }

        public bool AssignTent(GridCell cell, List<GridCell> grid, List<Constraint> constraints)
        {
            //Assign tent
            cell.Status = Status.Tent;

            //Update the associated constraints
            foreach (Constraint constraint in constraints.Where(c => c.Row == cell.Row || c.Column == cell.Column))
            {
                constraint.NumberOfTents -= 1;
            }

            //Update grid
            PlantGrassOnZeroConstraints(grid, constraints);
            PlantGrassAroundTents(grid);
            if (!IsValid(grid, constraints))
            {
                return false;
            }

            UpdateProbability(grid, constraints);
            return true;
        }

        public void PlantGrassAroundTents(List<GridCell> grid)
        {
            foreach (GridCell cell in grid.Where(c => c.Status == Status.None && GetAdjacentCells(c, grid, true).Any(c2 => c2.Status == Status.Tent)))
            {
                cell.Status = Status.Grass;
            }
        }
        public bool IsComplete(List<GridCell> grid, List<Constraint> constraints)
        {
            if (grid.Count(c => c.Status == Status.Tree) != grid.Count(c => c.Status == Status.Tree))
            {
                return false;
            }

            if (grid.Any(c => c.Status == Status.None))
            {
                return false;
            }

            List<List<GridCell>> groups = new List<List<GridCell>>();
            foreach (GridCell cell in grid)
            {
                if (!groups.Any(g => g.Contains(cell)) && (cell.Status == Status.Tent || cell.Status == Status.Tree))
                {
                    groups.Add(GetGroup(cell, grid, new List<GridCell> { cell }));
                }
            }
            //Any group with different number of trees and tents
            if (groups.Any(g => g.Count(c => c.Status == Status.Tree) != g.Count(c => c.Status == Status.Tent)))
            {
                return false;
            }
            return true;
        }

        public List<GridCell> GetGroup(GridCell cell, List<GridCell> grid, List<GridCell> currentGroup)
        {

            List<GridCell> queue = GetAdjacentCells(cell, grid, false).Where(c => c.Status == Status.Tent || c.Status == Status.Tree).ToList();
            foreach (GridCell c in queue)
            {
                if (!currentGroup.Any(x => x.Row == c.Row && x.Column == c.Column))
                {
                    currentGroup.Add(c);
                    GetGroup(c, grid, currentGroup);
                }
            }
            return currentGroup;
        }

        public bool IsValid(List<GridCell> grid, List<Constraint> constraints)
        {
            //Check if constraints are still valid
            foreach (Constraint constraint in constraints)
            {
                int rCells = grid.Where(c => c.Row == constraint.Row && c.Status == Status.None).Count();
                int cCells = grid.Where(c => c.Column == constraint.Column && c.Status == Status.None).Count();

                if (rCells + cCells - constraint.NumberOfTents < 0)
                {
                    return false;
                }
            }

            //Check if tents are adjacent to each other         
            if (grid.Where(c => c.Status == Status.Tent).Any(c => GetAdjacentCells(c, grid, false).Any(c2 => c2.Status == Status.Tent)))
            {
                return false;
            }
            return true;
        }
    }

    public static class PermutationsAndCombinations
    {
        public static long Combination(int n, int r)
        {
            // naive: return Factorial(n) / (Factorial(r) * Factorial(n - r));
            return Permutation(n, r) / Factorial(r);
        }

        public static long Permutation(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }

        private static long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static long Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
    }

    static class Extensions
    {
        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}