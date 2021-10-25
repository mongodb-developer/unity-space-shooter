using Realms;
using Realms.Sync;

public class PlayerProfile : RealmObject {

    [PrimaryKey]
    [MapTo("_id")]
    public string UserId { get; set; }

    [MapTo("name")]
    public string Name { get; set; }

    [MapTo("email")]
    public string Email { get; set; }

    [MapTo("high_score")]
    public int HighScore { get; set; }

    [MapTo("score")]
    public int Score { get; set; }

    [MapTo("spark_blaster_enabled")]
    public bool SparkBlasterEnabled { get; set; }

    [MapTo("cross_blaster_enabled")]
    public bool CrossBlasterEnabled { get; set; }

    public PlayerProfile() {}

    public PlayerProfile(string userId, string email) {
        this.UserId = userId;
        this.Name = "";
        this.Email = email;
        this.HighScore = 0;
        this.Score = 0;
        this.SparkBlasterEnabled = false;
        this.CrossBlasterEnabled = false;
    }

    public PlayerProfile(string userId, string name, string email) {
        this.UserId = userId;
        this.Name = name;
        this.Email = email;
        this.HighScore = 0;
        this.Score = 0;
        this.SparkBlasterEnabled = false;
        this.CrossBlasterEnabled = false;
    }

}