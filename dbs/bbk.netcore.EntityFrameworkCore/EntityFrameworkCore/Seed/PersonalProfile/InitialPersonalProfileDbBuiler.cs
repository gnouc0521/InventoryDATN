using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.EntityFrameworkCore.Seed.PersonalProfile
{
    public class InitialPersonalProfileDbBuiler
    {
        private readonly netcoreDbContext _context;

        public InitialPersonalProfileDbBuiler(netcoreDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultPersonalProfileSeed(_context).Create();
            new DefaultLanguageVeaSeed(_context).Create();
            _context.SaveChanges();
        }

    }
}
