using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TesteBlog.Model;
using Xamarin.Essentials;
using TesteBlog.Utils;
using TesteBlog.IService;
using TesteBlog.Service;
using Xamarin.Forms;
using TesteBlog.LocalData;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace TesteBlog.ViewModel
{
  public class PostsViewModel : BaseViewModel
  {
    public List<Post> PostsList;
    ObservableCollection<PostModel> _collectionPosts;
    public ObservableCollection<PostModel> CollectionPosts
    {
      set
      {
        _collectionPosts = value;
        OnPropertyChanged(nameof(CollectionPosts));
      }
      get
      {
        return _collectionPosts;
      }
    }

    public PostsViewModel()
    {
      PopulaPosts();
    }

    private void PopulaPosts()
    {
      if (CheckConexao())
      {
        IEnumerable<PostModel> posts = GetPosts();
        if (posts != null)
        {
          PopulaObservableCollection(posts);
          PopulaLocalDBPosts(posts);
        }
      }
      else
      {
        Task task = null;
        task = Task.Run(async () =>
        {
          await GetPostsLocalDBAsync();
        });
        task.Wait();

        if (PostsList != null)
        {
          List<PostModel> posts = GetPostsDBLocal();
          PopulaObservableCollection(posts);
        }
      }
    }

    //Populo a collection, caso necessário posso fazer alguma modificação em algum item   
    private void PopulaObservableCollection(IEnumerable<PostModel> posts)
    {
      CollectionPosts = new ObservableCollection<PostModel>();
      foreach (PostModel item in posts)
      {
        CollectionPosts.Add(item);
      }
    }

    private IEnumerable<PostModel> GetPosts()
    {
      IEnumerable<PostModel> post = null;

      try
      {
        IWebApiService<IEnumerable<PostModel>> webApiService = new WebApiService<IEnumerable<PostModel>>();
        post = webApiService.Get(Consts.UrlPosts);
      }
      catch (Exception ex)
      {
        Device.BeginInvokeOnMainThread(() =>
        {
          MessagingCenter.Send(new ArgumentException(ex.Message), "FalhaUnauthorizedException");
        });
        throw;
      }
      return post;
    }

    /// Verifica conectividade do dispositivo
    public bool CheckConexao()
    {
      var current = Connectivity.NetworkAccess;
      if (current == NetworkAccess.Internet) return true;
      return false;
    }

    /// Instancia do banco de dados
    public static DataBase DB => _dataBase ?? (_dataBase = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Posts.db3")));
    private static DataBase _dataBase;

    //Verifico se não estou inserindo itens duplicados na base local
    public bool ValidateInsertDB(int id)
    {
      if (PostsList != null)
      {
        if (!PostsList.Any(n => n.id == id))
        {
          return true;
        }
        else return false;
      }
      {
        return true;
      }
    }

    public async Task GetPostsLocalDBAsync()
    {
      DB.CreateTable<Post>();
      PostsList = await DB.GetToListAsync<Post>();
    }

    //Populo a base local quando tem conexão 
    async void PopulaLocalDBPosts(IEnumerable<PostModel> posts)
    {
      await GetPostsLocalDBAsync();
      foreach (PostModel post in posts)
      {
        Post postsLocalDB = new Post
        {
          body = post.body,
          id = post.id,
          userId = post.userId,
          title = post.title
        };

        if (ValidateInsertDB(post.id))
        {
          await DB.InsertAsync(postsLocalDB);
        }
        else
        {
          await DB.UpdateAsync(postsLocalDB);
        }
      }
    }

    private List<PostModel> GetPostsDBLocal()
    {
      List<PostModel> posts = new List<PostModel>();
      foreach (Post item in PostsList)
      {
        PostModel post = new PostModel
        {
          body = item.body,
          id = item.id,
          title = item.title,
          userId = item.userId
        };
        posts.Add(post);
      }
      return posts;
    }
  }
}
