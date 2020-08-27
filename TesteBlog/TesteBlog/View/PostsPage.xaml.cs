using System;
using System.Threading.Tasks;
using TesteBlog.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TesteBlog.View
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PostsPage : ContentPage
  {
    private PostsViewModel _postsViewModel { get; set; }
    public PostsPage()
    {
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      Task<PostsViewModel> task = Task.Run(() =>
      {
        return new PostsViewModel();
      });
      task.Wait();
      if (task.Status == TaskStatus.RanToCompletion)
      {
        _postsViewModel = task.Result;
      }
      PostsList.ItemsSource = _postsViewModel.CollectionPosts;
    }

    private void PostTapped(object sender, EventArgs e)
    {

    }
  }
}