using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAndStuff.Pages.TentsAndTrees
{
    public class IndexModel : PageModel
    {

        public class GridCell
        {
            bool isTent;    
        }

        [BindProperty]
        public List<List<GridCell>> Grid { get; set;}

        [BindProperty]
        public int Rows { get; set; }

        [BindProperty]
        public int Columns { get; set; }

        public List<SelectListItem> numbers;

        public void OnGet(int r = 4, int c = 4)
        {
            Rows = r;
            Columns = c;

            numbers = new List<SelectListItem>();
            for (int i = 4; i<8; i++)
            {
                numbers.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
        }

        public IActionResult OnPostGenerateGrid()
        {
            return Page();
        }
    }
}