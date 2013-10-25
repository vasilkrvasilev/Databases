using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Word
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Translation { get; set; }

    [BsonConstructor]
    public Word(string name, string translation)
    {
        this.Name = name;
        this.Translation = translation;
    }
}