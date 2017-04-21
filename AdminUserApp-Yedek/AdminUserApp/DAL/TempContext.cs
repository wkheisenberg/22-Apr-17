﻿using AdminUserApp.Models;
using System.Data.Entity;

namespace AdminUserApp.DAL
{
    public class TempContext : DbContext
    {
        public TempContext() : base("MystringConn") { }
        public DbSet<Role> roller { get; set; }
        public DbSet<User> kullar { get; set; }
    }
}