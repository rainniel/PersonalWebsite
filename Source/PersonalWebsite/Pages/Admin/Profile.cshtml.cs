using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Helpers;
using PersonalWebsite.Models;
using PersonalWebsite.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class ProfileModel(IDataCacheService<SiteSetting> siteSetting, IDataCacheService<SocialMedia> socialMedia) : PageModel
    {
        private readonly IDataCacheService<SiteSetting> _siteSetting = siteSetting;
        private readonly IDataCacheService<SocialMedia> _socialMedia = socialMedia;

        #region Bindings
        [BindProperty]
        public bool RemovePicture { get; set; }

        [BindProperty]
        public IFormFile? PictureFile { get; set; }

        [BindProperty]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        public SocialMedia SMFacebook { get; set; } = new();

        [BindProperty]
        public SocialMedia SMLinkedIn { get; set; } = new();

        [BindProperty]
        public SocialMedia SMXTwitter { get; set; } = new();

        [BindProperty]
        public SocialMedia SMGitHub { get; set; } = new();
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            FullName = (await _siteSetting.GetLatestAsync(SettingNames.OwnerName)).Value ?? "";

            SMFacebook = await _socialMedia.GetLatestAsync(SocialMediaNames.Facebook);
            SMLinkedIn = await _socialMedia.GetLatestAsync(SocialMediaNames.LinkedIn);
            SMXTwitter = await _socialMedia.GetLatestAsync(SocialMediaNames.XTwitter);
            SMGitHub = await _socialMedia.GetLatestAsync(SocialMediaNames.GitHub);

            return Page();
        }

        public async Task<IActionResult> OnPostFormGeneral()
        {
            if (PictureFile != null || RemovePicture)
            {
                const string defaultPicture = "profile.webp";
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                FileHelper.PrepareDirectory(imageDirectory);

                if (RemovePicture)
                {
                    var oldProfilePicture = (await _siteSetting.GetLatestAsync(SettingNames.ProfilePicture)).Value;

                    if (!string.IsNullOrWhiteSpace(oldProfilePicture))
                    {
                        await _siteSetting.SaveAsync(SettingNames.ProfilePicture, new SiteSetting());

                        if (!oldProfilePicture.Equals(defaultPicture, StringComparison.OrdinalIgnoreCase))
                        {
                            FileHelper.DeleteFile(Path.Combine(imageDirectory, oldProfilePicture));
                        }
                    }
                }
                else if (PictureFile != null && PictureFile.Length > 0)
                {
                    using var image = await Image.LoadAsync(PictureFile.OpenReadStream());
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(300, 300),
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center
                    }));

                    image.Metadata.ExifProfile = null;
                    image.Metadata.IptcProfile = null;
                    image.Metadata.XmpProfile = null;

                    var imageFile = $"profile_{TextHelper.GenerateRandomHash(8).ToLower()}.webp";
                    await image.SaveAsync(Path.Combine(imageDirectory, imageFile), new WebpEncoder { Quality = 75 });

                    var oldProfilePicture = (await _siteSetting.GetLatestAsync(SettingNames.ProfilePicture)).Value;

                    await _siteSetting.SaveAsync(SettingNames.ProfilePicture, new SiteSetting(imageFile));

                    if (!string.IsNullOrWhiteSpace(oldProfilePicture)
                        && !oldProfilePicture.Equals(defaultPicture, StringComparison.OrdinalIgnoreCase))
                    {
                        FileHelper.DeleteFile(Path.Combine(imageDirectory, oldProfilePicture));
                    }
                }
            }

            await _siteSetting.SaveAsync(SettingNames.OwnerName, new SiteSetting(FullName));
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFormSocialMedia()
        {
            await _socialMedia.SaveAsync(SocialMediaNames.Facebook, SMFacebook);
            await _socialMedia.SaveAsync(SocialMediaNames.LinkedIn, SMLinkedIn);
            await _socialMedia.SaveAsync(SocialMediaNames.XTwitter, SMXTwitter);
            await _socialMedia.SaveAsync(SocialMediaNames.GitHub, SMGitHub);

            return Redirect($"{Routes.Admin.Profile}#social-media");
        }
    }
}
