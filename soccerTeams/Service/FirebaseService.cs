using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;
using soccerTeams.Model;

namespace soccerTeams.Service
{
    public class FirebaseService
    {
        FirebaseClient firebase = new FirebaseClient("https://soccer-teams-6cc28.firebaseio.com");

        public async Task<List<Team>> ObterTimes() => (await firebase
                .Child("Teams")
                .OnceAsync<Team>())
                .Select(time => new Team
                {
                    Nome = time.Object.Nome,
                    Numero = time.Object.Numero,
                    Titulos = time.Object.Titulos
                }).ToList();

        public async Task<Team> ObterTime(int numero)
        {
            var allTeams = await ObterTimes();

            await firebase.Child("Teams").OnceAsync<Team>();

            return allTeams.Where(team => team.Numero == numero).FirstOrDefault();
        }

        public async Task AdicionarTime(int numero, String nome, int titulos) =>
            await firebase
            .Child("Teams")
            .PostAsync(new Team
            {
                Numero = numero,
                Nome = nome,
                Titulos = titulos
            });

        public async Task<string> AtualizarTime(int numero, int titulos, String nome)
        {
            try
            {
                var updateTeam = (await firebase
                .Child("Teams")
                .OnceAsync<Team>())
                .Where(team => team.Object.Numero == numero)
                .FirstOrDefault();

                await firebase
                    .Child("Teams")
                    .Child(updateTeam.Key)
                    .PutAsync(new Team()
                    {
                        Numero = numero,
                        Nome = nome,
                        Titulos = titulos
                    });

                return null;
            }
            catch (Exception)
            {
                return "Time não encontrado";
            }
            
        }

        public async Task<string> ApagarTime(int numero)
        {
            try
            {
                var time = (await firebase
                .Child("Teams")
                .OnceAsync<Team>())
                .Where(team => team.Object.Numero == numero)
                .FirstOrDefault();

                await firebase
                    .Child("Teams")
                    .Child(time.Key)
                    .DeleteAsync();

                return null;
            } catch(Exception)
            {
                return "Time não encontrado";
            }
            
        }
    }
}
