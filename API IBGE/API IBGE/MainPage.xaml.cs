using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using API_IBGE.Model;
using System.Collections.ObjectModel;

namespace API_IBGE
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private const string Url = "http://jsonplaceholder.typicode.com/posts";
        private readonly HttpClient _client = new HttpClient();
        private ObservableCollection<Post> _posts;

        protected override async void OnAppearing()
        {
            string content = await _client.GetStringAsync(Url);
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(content);
            _posts = new ObservableCollection<Post>(posts);
            lvwDados.ItemsSource = _posts;
            base.OnAppearing();
        }

        private async void OnAdd(object sender, EventArgs e)
        {
            Post post = new Post { Title = $"Title: Timestamp {DateTime.UtcNow.Ticks}" };
            string content = JsonConvert.SerializeObject(post);
            await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            _posts.Insert(0, post);
        }

        private async void OnUpdate(object sender, EventArgs e)
        {
            Post post = _posts[1];
            post.Title += " [atualizado]";
            string content = JsonConvert.SerializeObject(post);
            await _client.PutAsync(Url + "/" + post.Id, new StringContent(content, Encoding.UTF8, "application/json"));
        }
        private async void OnDelete(object sender, EventArgs e)
        {
            Post post = _posts[0];
            await _client.DeleteAsync(Url + "/" + post.Id);
            _posts.Remove(post);
        }
    }
}
