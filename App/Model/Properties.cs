namespace App.Model;

public class Properties
{
    public int id { get; set; }
    public string title { get; set; }
    public int price { get; set; }
    public string currency { get; set; }
    public int bedrooms { get; set; }
    public int bathrooms { get; set; }
    public int area { get; set; }
    public double lat { get; set; }
    public double lng { get; set; }
    public string city { get; set; }
    public string address { get; set; }
    public string[] images { get; set; }
}

