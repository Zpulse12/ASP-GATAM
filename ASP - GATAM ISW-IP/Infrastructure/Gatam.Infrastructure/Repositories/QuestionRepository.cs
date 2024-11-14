using Gatam.Domain;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
