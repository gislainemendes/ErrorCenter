using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Central_De_Erros.Models;

namespace Central_De_Erros.Repository
{
    public interface IErrorRepository
    {

        List<Error> FindByLevel(string level);
        List<Error> FindByLogDescription(string description);
        List<Error> FindByIpOrigin(string ipOrigin);
        List<Error> FindByEnvironmentAndOrderByLevel(string environment);
        List<Error> FindByEnvironmentAndOrderByFrequency(string environment);
        void DeleteError(int id);
        Error SaveError(Error error);
        Error FindById(int id);
        List<Error> FindByEnvironment(string environment);
        bool archiveErrorById(int id);





    }
}
