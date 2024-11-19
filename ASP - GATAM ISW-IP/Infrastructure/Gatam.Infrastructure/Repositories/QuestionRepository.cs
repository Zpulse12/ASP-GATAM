using Gatam.Application.Interfaces;
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
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Question> GetQuestionAndAnswers(string Id)
        {
            return await _context.Questions.Include(q => q.Answers)
                 .FirstOrDefaultAsync(q => q.Id == Id);


        }

        public  async Task UpdateQuestionAndAnswers(Question entity)
        {
            Question existingQuestion = await GetQuestionAndAnswers(entity.Id);

            if (existingQuestion != null)
            {
                _context.Entry(existingQuestion).CurrentValues.SetValues(entity);

                foreach (var existingAnswer in existingQuestion.Answers.ToList())
                {
                    if (!entity.Answers.Any(a => a.Id == existingAnswer.Id))
                    {
                        _context.Answers.Remove(existingAnswer);
                    }
                }
            }

            foreach (var newAnswer in entity.Answers)
            {
                var existingAnswer = existingQuestion.Answers.FirstOrDefault(a => a.Id == newAnswer.Id);
                if (existingAnswer == null)
                {
                    var answersList = existingQuestion.Answers.ToList();
                    answersList.Add(newAnswer);
                    existingQuestion.Answers = answersList;
                }
                else
                {
                    _context.Entry(existingAnswer).CurrentValues.SetValues(newAnswer);
                }
            }
           
        }
    }
}
