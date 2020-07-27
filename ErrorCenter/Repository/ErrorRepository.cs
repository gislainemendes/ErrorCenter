using System.Collections.Generic;
using System.Linq;
using AutoMapper.Internal;
using Central_De_Erros.Models;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Remotion.Linq.Clauses;

namespace Central_De_Erros.Repository
{
    public class ErrorRepository : IErrorRepository
    {
        private CentralDeErrosContext context;

        public ErrorRepository(CentralDeErrosContext context) {

            this.context = context;
        }

        public List<Error> FindByLevel(string level)
        {
            return context.Errors
                .Include(error => error.frequency)
                .Where(error => EF.Functions.Like(error.Level, "%" + level + "%") && error.Archived == false)
                .ToList();
        }

        public List<Error> FindByLogDescription(string description)
        {
            return context.Errors
                .Include(error => error.frequency)
                .Where(error => EF.Functions.Like(error.LogDescription, "%" + description + "%") && error.Archived == false)
                .ToList();

        }

        public List<Error> FindByIpOrigin(string ipOrigin)
        {
            return context.Errors
                .Include(error => error.frequency)
                .Where(error => EF.Functions.Like(error.IpOrigin, "%" + ipOrigin + "%") && error.Archived == false)
                .ToList();
        }

        public Error FindById(int id)
        {
            return context.Errors
                .Include(error => error.frequency)
                .Where(error => error.Id == id)
                .FirstOrDefault();
        }

        public List<Error> FindByEnvironment(string environment)
        {
            return context.Errors
                .Include(error => error.frequency)
                .Where(error => error.Environment == environment && error.Archived == false)
                .ToList();

        }


        public List<Error> FindByEnvironmentAndOrderByLevel(string environment)
        {
           return context.Errors
                    .Include(error => error.frequency)
                    .Where(error => error.Environment == environment && error.Archived == false)
                    .OrderBy(error => error.Level)
                    .ToList();
            
        }

        public List<Error> FindByEnvironmentAndOrderByFrequency(string environment)
        {
            return context.Errors
                    .Include(error => error.frequency)
                    .Where(error => error.Environment == environment)
                    .OrderByDescending(error => error.frequency.NumberOfEvents)
                    .ToList();
        }

 

        public void DeleteError(int id)
        {
            context.Remove(context.Errors.Single(error => error.Id == id));
            context.SaveChanges();
        }

        public Error SaveError(Error error)
        {
            var state = error.Id == 0 ? EntityState.Added : EntityState.Modified;
            context.Entry(error).State = state;
            context.SaveChanges();
            return error;
        }

        public bool archiveErrorById(int id)
        {
            Error error = FindById(id);
            if(error == null)
            {
                return false;
            }

            error.Archived = true;
            context.SaveChanges();
            return true;
        }

        private void SetFrequencyInErrors(List<Error> errors)
        {
            IDictionary<string, Frequency> frequencies = context.Frequencies.ToDictionary(frequency => frequency.LogDescription);
            errors.ForEach(error => error.frequency = frequencies.GetOrDefault(error.LogDescription));
        }

        private void SetFrequencyInErrors(Error error)
        {
            IDictionary<string, Frequency> frequencies = context.Frequencies.ToDictionary(frequency => frequency.LogDescription);
             error.frequency = frequencies.GetOrDefault(error.LogDescription);

        }


    }
}
