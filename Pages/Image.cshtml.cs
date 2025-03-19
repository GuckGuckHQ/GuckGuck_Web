using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

public class DisplayImageModel : PageModel
{
	public string ImageUrl { get; private set; }
	public string Id { get; private set; }

	public async Task<IActionResult> OnGetAsync(string id)
	{
		Id = id;
		var baseUrl = $"{Request.Scheme}://{Request.Host}";
		ImageUrl = $"{baseUrl}/image/{id}";

		using (var httpClient = new HttpClient())
		{
			try
			{
				var response = await httpClient.GetAsync(ImageUrl);
				if (!response.IsSuccessStatusCode)
				{
					return RedirectToPage("/Error404"); // Redirect to a 404 page
				}
			}
			catch
			{
				return RedirectToPage("/Error404"); // Redirect to a 404 page
			}
		}

		return Page();
	}
}