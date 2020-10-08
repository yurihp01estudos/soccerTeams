using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace soccerTeams
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        private void onClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
