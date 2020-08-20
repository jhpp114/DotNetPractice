using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor.Pages.BookList
{
    public class indexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public indexModel(ApplicationDbContext db) 
        {
            _db = db;
        }

        public IEnumerable<Book> Books { get; set; }
        public async Task OnGet()
        {
            Books = await _db.Book.ToListAsync();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var deleteBook = await _db.Book.FindAsync(id);
            if (deleteBook == null)
            {
                return NotFound();
            }
            _db.Book.Remove(deleteBook);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}