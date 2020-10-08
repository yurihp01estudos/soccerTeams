using System;
using System.Linq;
using Xamarin.Forms;
using soccerTeams.Service;
using soccerTeams.Model;

namespace soccerTeams
{
    public partial class MainPage : ContentPage
    {
        FirebaseService firebaseService = new FirebaseService();

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var teams = await firebaseService.ObterTimes();
            lstTeams.ItemsSource = teams;
        }

        private async void BtnAdicionarClicked(object sender, EventArgs e)
        {
            if (int.TryParse(entryNumero.Text, out int numero) &&
                int.TryParse(entryTitulos.Text, out int titulos) &&
                entryNome.Text.Trim().Count() > 0)
            {
                await firebaseService.AdicionarTime(
                numero,
                entryNome.Text,
                titulos);

                await DisplayAlert("Sucesso", "Time Adicionado com Sucesso", "Ok");

                var teams = await firebaseService.ObterTimes();

                lstTeams.ItemsSource = teams;
            } else
            {
                await DisplayAlert("Erro", "Verifique o tipo dos campos e tente novamente", "Ok");
            }
            clearEntries();
        }

        private async void BtnObterClicked(object sender, EventArgs e)
        {
            if (int.TryParse(entryNumero.Text, out int value))
            {
                var team = await firebaseService.ObterTime(value);

                if (team != null)
                {
                    entryNumero.Text = team.Numero.ToString();
                    entryNome.Text = team.Nome;
                    entryTitulos.Text = team.Titulos.ToString();
                }
                else
                {
                    await DisplayAlert("Erro", "Time não encontrado!", "Ok");
                    clearEntries();
                }
            } else
            {
                await DisplayAlert("Erro", "Campo Número está vazio ou valor não é número", "Ok");
                clearEntries();
            }
        }

        private async void BtnAtualizarClicked(object sender, EventArgs e)
        {
            if (int.TryParse(entryNumero.Text, out int numero) &&
                int.TryParse(entryTitulos.Text, out int titulos) &&
                entryNome.Text.Trim().Count() > 0)
            {
                var response = await firebaseService.AtualizarTime(
                numero,
                titulos,
                entryNome.Text);

                if (response == null)
                {
                    await DisplayAlert("Sucesso", "Time Atualizado", "Ok");

                    lstTeams.ItemsSource = await firebaseService.ObterTimes();
                } else
                {
                    await DisplayAlert("Erro", response, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Verifique o tipo dos campos e tente novamente", "Ok");
            }
            clearEntries();
        }

        private async void BtnRemoverClicked(object sender, EventArgs e)
        {
            if (int.TryParse(entryNumero.Text, out int value))
            {
                var response = await firebaseService.ApagarTime(Convert.ToInt32(value));

                if (response == null)
                {
                    await DisplayAlert("Sucesso", "Time Excluído", "Ok");
                    lstTeams.ItemsSource = await firebaseService.ObterTimes();
                } else
                {
                    await DisplayAlert("Erro", response, "Ok");
                }
                
            } else
            {
                await DisplayAlert("Erro", "Campo Número está vazio ou valor não é número", "Ok");
            }
            clearEntries();
        }

        private void onSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var detailPage = new DetailPage
            {
                BindingContext = e.SelectedItem as Team
            };

            lstTeams.SelectedItem = null;

            Navigation.PushModalAsync(detailPage);

            
        }

        private void clearEntries()
        {
            entryNumero.Text = String.Empty;
            entryNome.Text = String.Empty;
            entryTitulos.Text = String.Empty;
        }
    }
}
