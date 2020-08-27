using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TesteBlog.ViewModel;

namespace TesteBlog.Model
{
  public class PostModel : BaseViewModel
  {
    public int userId { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
  }
  public class PostApiModel
  {
    public List<PostModel> Posts { get; set; }
  }


  [Table("Posts")]
  public class Post : IModelBase
  {
    [Column("tableId"), PrimaryKey, AutoIncrement]
    public int tableId { get; set; }

    [Column("id")]
    public int id { get; set; }

    [Column("userId")]
    public int userId { get; set; }

    [Column("title")]
    public string title { get; set; }

    [Column("body")]
    public string body { get; set; }
  }

  public interface IModelBase
  {
    int tableId { get; set; }
  }
}

