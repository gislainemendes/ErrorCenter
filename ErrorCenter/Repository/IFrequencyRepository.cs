namespace Central_De_Erros.Repository
{
    using Central_De_Erros.Models;

    public interface IFrequencyRepository
    {
        public Frequency SumToFrequency(string logDescription);

        public Frequency SubtractFrequency(string logDescription);
    }
}
