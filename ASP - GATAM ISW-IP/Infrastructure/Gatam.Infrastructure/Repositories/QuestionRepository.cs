using Gatam.Domain;
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
        public QuestionRepository(DbContext context) : base(context)
        {
        }
    }
}
