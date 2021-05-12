using Realms;
using Realms.Sync;
using MongoDB.Bson;

public class PlayerProfile : RealmObject
{

    // [PrimaryKey]
    // [MapTo("_id")]
    // public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [PrimaryKey]
    [MapTo("_id")]
    public string UserId { get; set; }

    [MapTo("high_score")]
    public int HighScore { get; set; }

    [MapTo("spark_blaster_enabled")]
    public bool SparkBlasterEnabled { get; set; }

    [MapTo("cross_blaster_enabled")]
    public bool CrossBlasterEnabled { get; set; }

    public PlayerProfile() {}

    public PlayerProfile(string userId) {
        this.UserId = userId;
        this.HighScore = 0;
        this.SparkBlasterEnabled = false;
        this.CrossBlasterEnabled = false;
    }

}