﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domin.Entity;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infarstuructre.Domin
{
    public class BookingDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }
        public DbSet<VwUser> VwUsers { get; set; }
    }
}
