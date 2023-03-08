﻿using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.FundooDBContext
{
   public class FunContext : DbContext
    {
        public FunContext(DbContextOptions dbContextOptions):base(dbContextOptions) 
        { }

        public DbSet<UserEntity> User { get; set; }
    }
}
