using Central_De_Erros.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Central_De_Erros.Repository
{
    public class FrequencyRepository : IFrequencyRepository
    {

        private CentralDeErrosContext context;

        public FrequencyRepository(CentralDeErrosContext context)
        {
            this.context = context;
        }

        private Frequency createFrequencyOfLogDescription(string logDescription)
        {
            Frequency frequency = new Frequency()
            {
                LogDescription = logDescription,
                NumberOfEvents = 1
            };

            var state = EntityState.Added;
            context.Entry(frequency).State = state;
            context.SaveChanges();

            return frequency;
        }

        public Frequency SumToFrequency(string logDescription)
        {
            Frequency frequency = context.Frequencies
                                    .Where(frequency => frequency.LogDescription == logDescription)
                                    .FirstOrDefault();
            if (frequency == null)
            {
                return createFrequencyOfLogDescription(logDescription);
            }
            else
            {
                frequency.NumberOfEvents++;
                context.SaveChanges();
                return frequency;
            }

        }

        public Frequency SubtractFrequency(string logDescription)
        {
            Frequency frequency = context.Frequencies
                                    .Where(frequency => frequency.LogDescription == logDescription)
                                    .FirstOrDefault();
            frequency.NumberOfEvents--;
            context.SaveChanges();

            return frequency;
        }


    }
}
