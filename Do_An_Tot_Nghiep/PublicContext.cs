using Do_An_Tot_Nghiep.Models;
using Microsoft.EntityFrameworkCore;

namespace Do_An_Tot_Nghiep;

public class PublicContext : DbContext
{
    public  DbSet<User> Users { get; set; }
    public  DbSet<Grammar>Grammars  { get; set; }
    public  DbSet<ExamTip>ExamTips  { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostReact> PostReacts { get; set; }
    public DbSet<QuestionToeic> QuestionToeics { get; set; }
    public DbSet<AnswerToeic> AnswerToeics { get; set; }
    public DbSet<GroupQuestion> GroupQuestions { get; set; }
    private const string connectionString = @"User ID=minhpd;Password=123qwe;Host=103.124.95.246;Port=5432;Database=minh1;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Lifetime=0;";
    protected override void  OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(connectionString);
    }
}