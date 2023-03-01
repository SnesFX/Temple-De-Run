using System.Collections;
using System.Collections.Generic;

public class FlurryOffer
{
	public long id;

	public string name;

	public float price;

	public string description;

	public int imageWidth;

	public int imageHeight;

	public string imagePath;

	public FlurryOffer(Hashtable ht)
	{
		id = long.Parse(ht["Id"].ToString());
		name = ht["name"].ToString();
		price = float.Parse(ht["price"].ToString());
		description = ht["description"].ToString();
		imageWidth = int.Parse(ht["imageWidth"].ToString());
		imageHeight = int.Parse(ht["imageHeight"].ToString());
		imagePath = ht["imagePath"].ToString();
	}

	public static List<FlurryOffer> fromJSON(string json)
	{
		List<FlurryOffer> list = new List<FlurryOffer>();
		ArrayList arrayList = json.arrayListFromJson();
		foreach (Hashtable item in arrayList)
		{
			list.Add(new FlurryOffer(item));
		}
		return list;
	}

	public override string ToString()
	{
		return string.Format("<FlurryOffer> id: {0}, name: {1}, price: {2}, description: {3}, imagePath: {4}", id, name, price, description, imagePath);
	}
}
