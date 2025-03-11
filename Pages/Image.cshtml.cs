using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DisplayImageModel : PageModel
{
	public string ImageUrl { get; private set; }
	public string Id { get; private set; }

	public void OnGet(string id)
	{
		Id = id;
		var baseUrl = $"{Request.Scheme}://{Request.Host}";
		ImageUrl = $"{baseUrl}/image/{id}";
	}
}