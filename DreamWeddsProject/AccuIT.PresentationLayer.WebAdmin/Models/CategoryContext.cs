using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImportToXLS.Models
{
    public class CategoryContext : DbContext
    {
        public DbSet<Category> categories { get; set; }
    }
}